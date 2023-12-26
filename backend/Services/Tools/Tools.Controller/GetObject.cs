using System.IdentityModel.Tokens.Jwt;


namespace Tools.Controller
{
    public static class GetObject
    {
        public static int GetId(List<ClaimModel> models)
        {
            return Convert.ToInt32(models.First(x => x.Type == "Id").Value);
        }

        public static List<ClaimModel> GetClaimsFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var claims = jwtToken.Claims.Select(c => new ClaimModel { Type = c.Type, Value = c.Value }).ToArray();
                return claims.ToList();
            }
            catch (Exception ex)
            {
                // Обработка ошибок в случае невозможности десериализовать токен
                Console.WriteLine($"Ошибка при разборе токена: {ex.Message}");
                return new();
            }
        }

        public static UserClaims GetUser(List<ClaimModel> claims)
        {
            return new()
            {
                Id = GetId(claims),
                Name = claims.FirstOrDefault(c => c.Type == "name").Value,
            };
        }

        public record class UserClaims
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
