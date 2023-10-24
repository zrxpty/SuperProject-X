using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtAuthenticationManager
{
    public class AuthOptions
    {
        public const string ISSUER = "superproject";
        public const string AUDIENCE = "superprojectapi";
        public const string KEY = "jZMhmripoIMmmUTSpujMtCEwXDFibIBN";
        public const int LIFETIME = 7; // in days

        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}
