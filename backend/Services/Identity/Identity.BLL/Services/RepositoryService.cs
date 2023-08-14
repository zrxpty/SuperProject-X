using Identity.BLL.Inrefaces;
using Identity.BLL.Models.InputModels;
using Identity.BLL.Models.OutputModels;
using Identity.Data;
using Identity.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.BLL.Services
{
    public class RepositoryService : IRepositotyService
    {
        private readonly IdentityDbContext _db;
        private readonly IConfiguration _configuration;

        public RepositoryService(IdentityDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<RegisterOutputModel> Authenticate(AuthenticateInputModel input)
        {
            var user = await _db.UserAccounts
                                .Where(x => x.Email == input.Login || x.Login == input.Login)
                                .FirstOrDefaultAsync();

            if (user != null && DecodeFrom64(user.Password) == input.Password)
            {
                var token = await _db.Verifications.Where(x => x.UserAccountId == user.Id).FirstOrDefaultAsync();
                if (token != null)
                {
                    token.Token = await GenerateToken(user);
                }
                else
                {
                    token.Token = await GenerateToken(user);
                    token.UserAccountId = user.Id;

                }

                await _db.SaveChangesAsync();

                return new RegisterOutputModel
                {
                    Login = user.Login,
                    Email = user.Email,
                    Token = token.Token,
                    Role = (await _db.Roles.Where(x => x.UserAccountId == user.Id).FirstAsync()).Role
                };
            }
            else
            {
                return new RegisterOutputModel
                {
                    Email = "",
                    Login = "",
                    Token = "",
                    Role = "",
                };
            }
        }

        public async Task<RegisterOutputModel> CreateUser(RegisterInputModel input)
        {
            var userWithSameEmail = await _db.UserAccounts
                .FirstOrDefaultAsync(x => x.Email == input.Email);

            var userWithSameLogin = await _db.UserAccounts
                .FirstOrDefaultAsync(x => x.Login == input.Login);

            if (userWithSameEmail != null)
            {
                return new RegisterOutputModel
                {
                    Token = "",
                    Email = input.Email,
                    Login = input.Login,
                    Role = ""

                };
            }

            if (userWithSameLogin != null)
            {
                return new RegisterOutputModel
                {
                    Token = "",
                    Email = input.Email,
                    Login = input.Login,
                    Role = ""

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

            var profile = new Profile()
            {
                UserAccountId = user.Id,
                BirthDay = null,
                City = null,
                Sex = null,
                CreatedDate = DateTime.Now
            };

            await _db.Profiles.AddAsync(profile);

            var role = new Roles()
            {
                Role = "user",
                UserAccountId = user.Id
            };

            var verification = new Verification()
            {
                UserAccountId = user.Id,
                Token = await GenerateToken(user)
            };

            await _db.Roles.AddAsync(role);
            await _db.Verifications.AddAsync(verification);

            await _db.SaveChangesAsync();

            return new RegisterOutputModel
            {
                Token = verification.Token,
                Email = input.Email,
                Login = input.Login,
                Role = role.Role
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

        private async Task<string> GenerateToken(UserAccount input)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, input.Login) };

            var jwt = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:ISSUER"],
                audience: _configuration["AuthSettings:AUDIENCE"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:KEY"])), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        
    }
}
