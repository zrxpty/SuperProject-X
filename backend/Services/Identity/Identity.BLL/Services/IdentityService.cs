using Identity.BLL.Inrefaces;
using Identity.BLL.Models.InputModels;
using Identity.BLL.Models.OutputModels;
using Identity.Data;
using Identity.Data.Models;
using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tools.GenericModels;

namespace Identity.BLL.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IdentityDbContext _db;
        private readonly IConfiguration _configuration;

        public IdentityService(IdentityDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        
        public async Task<ServiceResponse<AuthenticationOutputModel>> Authenticate(AuthenticateInputModel input)
        {
            var user = await _db.UserAccounts
                                .FirstOrDefaultAsync(x => x.Email == input.Login || x.Login == input.Login);
           

            if (user != null && DecodeFrom64(user.Password) == input.Password)
            {
                var role = await _db.Roles.FirstOrDefaultAsync(role => role.UserAccountId == user.Id);

                return new()
                {
                    Data = new AuthenticationOutputModel
                    {
                        Login = user.Login,
                        Email = user.Email,
                        Token = GenerateToken(user, role.Role),
                        Role = role.Role
                    },
                    Message = $"{user.Login} {user.Email}"
                };
               
            }
            else
            {
                return new()
                {
                    Code = 401,
                    Message = "Пошел ты кого взломать хочешь?"
                };
            }
        }

        public async Task<ServiceResponse<AuthenticationOutputModel>> Register(RegisterInputModel input)
        {
            var userWithSameEmail = await _db.UserAccounts
                .FirstOrDefaultAsync(x => x.Email == input.Email);

            var userWithSameLogin = await _db.UserAccounts
                .FirstOrDefaultAsync(x => x.Login == input.Login);

            if (userWithSameEmail != null)
            {
                return new()
                {
                    Code = 401,
                    Message = "Пошел ты кого взломать хочешь?"

                };
            }

            if (userWithSameLogin != null)
            {
                return new()
                {
                    Code = 401,
                    Message = "Пошел ты кого взломать хочешь?"
                };
            }

            var user = new UserAccount()
            {
                Email = input.Email,
                Login = input.Login,
                Password = EncryptPassword(input.Password),
            };

            await _db.UserAccounts.AddAsync(user);
            await _db.SaveChangesAsync();

            var role = new Roles()
            {
                Role = "user",
                UserAccountId = user.Id
            };
            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();

            return new() {
                Data = new AuthenticationOutputModel
                {
                    Token = GenerateToken(user, role.Role),
                    Email = input.Email,
                    Login = input.Login,
                    Role = role.Role
                }

            };
        }

        private string EncryptPassword(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        private string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        private string GenerateToken(UserAccount input, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthOptions.KEY);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(JwtRegisteredClaimNames.Name, input.Login),
            new Claim("Id", input.Id.ToString()),
            new Claim("Role", role)
                }),
                Issuer = AuthOptions.ISSUER,
                Audience = AuthOptions.AUDIENCE,
                Expires = DateTime.UtcNow.AddMinutes(999999),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



    }
}
