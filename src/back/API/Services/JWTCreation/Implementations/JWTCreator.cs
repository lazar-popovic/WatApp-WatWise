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

        public string? CreateToken(User request)
        {

            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, request.Email!),
                new Claim(ClaimTypes.Role, request.Role!.RoleName),
                new Claim("UserID", request.Id.ToString()),
                new Claim("RoleID", request.Role.Id.ToString())
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "login claims");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "http://localhost:5226",
                Audience = "http://localhost:5226",
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)), SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            token.Payload.AddClaims(principal.Claims);
 
            var jwt = tokenHandler.WriteToken(token);

            if (jwt != null)
                return jwt;
            else
                return null;
        }

        public string CreateVerificationToken(int userId)
        {
            List<Claim> claims = new List<Claim> {
                new Claim("userId", $"{userId}"),
                new Claim("purpose", "verification")
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = signingCredentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }

        public string? CreateResetToken(int userId, string userEmail, ResetPasswordToken resetToken)
        {
            List<Claim> claims = new List<Claim> {
                new Claim("userId", $"{userId}"),
                new Claim("userEmail", $"{userEmail}"),
                new Claim("purpose", "password reset"),
                new Claim("resetToken", resetToken.Token!)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "reset claims");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "http://localhost:5226",
                Audience = "http://localhost:5226",
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)), SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            token.Payload.AddClaims(principal.Claims);
            var jwt = tokenHandler.WriteToken(token);

            if (jwt != null)
                return jwt;
            else
                return null;
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
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                ValidateLifetime = false 
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenValidationException("Invalid token");

            return principal;
        }

        public int GetUserIdFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = secretKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                var userIdClaim = claimsPrincipal.FindFirst("userId");
                var userId = userIdClaim?.Value;

                if (int.TryParse(userId, out int id))
                {
                    return id;
                }
            }
            catch (SecurityTokenException se)
            {
                throw se;
            }
            catch
            {
                throw;
            }

            return -1;
        }
    }
}
