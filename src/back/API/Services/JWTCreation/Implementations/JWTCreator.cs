using API.Models.Entity;
using API.Services.JWTCreation.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha512Signature)
            };

            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            token.Payload.AddClaims(principal.Claims);

            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }

        public string CreateVerificationToken(int userId)
        {
            List<Claim> claims = new List<Claim> {
                new Claim("userId", $"{userId}"),
                new Claim("purpose", "verification")
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Token expires in 1 hour
                SigningCredentials = signingCredentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }

        public string CreateResetToken(int userId, string userEmail, ResetPasswordToken resetToken)
        {
            List<Claim> claims = new List<Claim> {
                new Claim("userId", $"{userId}"),
                new Claim("userEmail", $"{userEmail}"),
                new Claim("purpose", "password reset"),
                new Claim("resetToken", resetToken.Token)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "reset claims");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "http://localhost:5226",
                Audience = "http://localhost:5226",
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha512Signature)
            };

            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            token.Payload.AddClaims(principal.Claims);

            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
