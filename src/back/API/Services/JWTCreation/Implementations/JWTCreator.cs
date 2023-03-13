using API.Models.Entity;
using API.Services.JWTCreation.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services.JWTCreation.Implementations
{
    public class JWTCreator : IJWTCreator
    {
        private readonly IConfiguration _configuration;

        public JWTCreator(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        public string CreateToken(User request)
        {

            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.Role, request.Role.RoleName),
                new Claim("UserID", request.Id.ToString()),
                new Claim("RoleID", request.RoleId.ToString())
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "login claims");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "http://localhost:5226",
                Audience = "http://localhost:5226",
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha512Signature)
            };

            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            token.Payload.AddClaims(principal.Claims);

            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }

        public string CreateVerificationToken(string email)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, email)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "verification claims");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "http://localhost:5226",
                Audience = "http://localhost:5226",
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha512Signature)
            };

            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            token.Payload.AddClaims(principal.Claims);

            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }
    }
}
