﻿using Identity.BLL.Inrefaces;
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
                var role = await _db.Roles.FirstOrDefaultAsync(r => r.UserAccountId == user.Id);

                var token = GenerateToken(user, role.Role);

                var authenticationModel = new AuthenticationOutputModel
                {
                    Login = user.Login,
                    Email = user.Email,
                    Token = token,
                    Role = role.Role
                };

                return new ServiceResponse<AuthenticationOutputModel>
                {
                    Data = authenticationModel,
                    Message = $"{user.Login} {user.Email}"
                };
            }
            else
            {
                return new ServiceResponse<AuthenticationOutputModel>
                {
                    Code = 401,
                    Message = "Неверные учетные данные"
                };
            }
        }

        public async Task<ServiceResponse<AuthenticationOutputModel>> Register(RegisterInputModel input)
        {
            var userWithSameEmail = await _db.UserAccounts.FirstOrDefaultAsync(x => x.Email == input.Email);
            var userWithSameLogin = await _db.UserAccounts.FirstOrDefaultAsync(x => x.Login == input.Login);

            if (userWithSameEmail != null || userWithSameLogin != null)
            {
                return new ServiceResponse<AuthenticationOutputModel>
                {
                    Code = 401,
                    Message = "Пользователь с такими данными уже существует"
                };
            }

            var encryptedPassword = EncryptPassword(input.Password);

            var newUser = new UserAccount
            {
                Email = input.Email,
                Login = input.Login,
                Password = encryptedPassword,
            };

            await _db.UserAccounts.AddAsync(newUser);
            await _db.SaveChangesAsync();

            var role = new Roles
            {
                Role = "user",
                UserAccountId = newUser.Id
            };

            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();

            var token = GenerateToken(newUser, role.Role);

            var authenticationModel = new AuthenticationOutputModel
            {
                Token = token,
                Email = input.Email,
                Login = input.Login,
                Role = role.Role
            };

            return new ServiceResponse<AuthenticationOutputModel>
            {
                Data = authenticationModel
            };
        }
        #region private
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

        #endregion

    }
}
