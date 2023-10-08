﻿using Identity.BLL.Inrefaces;
using Identity.BLL.Models;
using Identity.BLL.Models.InputModels;
using Identity.BLL.Models.OutputModels;
using Identity.Data;
using Identity.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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

        public async Task<ServiceResponse<AuthenticationOutputModel>> Authenticate(AuthenticateInputModel input)
        {
            var user = await _db.UserAccounts
                                .Where(x => x.Email == input.Login || x.Login == input.Login)
                                .FirstOrDefaultAsync();

            if (user != null && DecodeFrom64(user.Password) == input.Password)
            {
                /*var token = await _db.Verifications.Where(x => x.UserAccountId == user.Id).FirstOrDefaultAsync();
                if (token != null)
                {
                    token.Token = await GenerateToken(user);
                }
                else
                {
                    token.Token = await GenerateToken(user);
                    token.UserAccountId = user.Id;

                }*/

                await _db.SaveChangesAsync();

                return new()
                {
                    Data = new AuthenticationOutputModel
                    {
                        Login = user.Login,
                        Email = user.Email,
                        Token = "",
                        Role = (await _db.Roles.Where(x => x.UserAccountId == user.Id).FirstAsync()).Role
                    }
                };
               
            }
            else
            {
                return new()
                {
                    Message = "Пошел ты кого взломать хочешь?"
                };
            }
        }

        public async Task<ServiceResponse<AuthenticationOutputModel>> CreateUser(RegisterInputModel input)
        {
            var userWithSameEmail = await _db.UserAccounts
                .FirstOrDefaultAsync(x => x.Email == input.Email);

            var userWithSameLogin = await _db.UserAccounts
                .FirstOrDefaultAsync(x => x.Login == input.Login);

            if (userWithSameEmail != null)
            {
                return new()
                {
                    Message = "Пошел ты кого взломать хочешь?"
                };
            }

            if (userWithSameLogin != null)
            {
                return new()
                {
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

            var profile = new Profile()
            {
                UserAccountId = user.Id,
                BirthDay = null,
                City = null,
                Sex = null,
                CreatedDate = DateTime.UtcNow
            };

            await _db.Profiles.AddAsync(profile);

            var role = new Roles()
            {
                Role = "user",
                UserAccountId = user.Id
            };

            await _db.SaveChangesAsync();

            return new() {
                Data = new AuthenticationOutputModel
                {
                    Token = await GenerateToken(user, role.Role),
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

        private async Task<string> GenerateToken(UserAccount input, string role)
        {
            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(20);
            var tokenKey = Encoding.ASCII.GetBytes(_configuration["AuthSettings:KEY"]);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, input.Login),
                new Claim("Id", input.Id.ToString()),
                new Claim("Role", role)
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return token;
        }

        
    }
}
