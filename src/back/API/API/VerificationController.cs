using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.API;

[ApiController]
[Route("api/auth")]
public class VerificationController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public VerificationController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    [Route("verify")]
    public async Task<IActionResult> VerifyToken( string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secretKey,
                ValidateIssuer = false,
                ValidateAudience = false,
            };
            SecurityToken validatedToken;
            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            var userIdClaim = claimsPrincipal.FindFirst("userId");
            var userId = userIdClaim?.Value;
            
            return Ok("Email verification successful");
        }
        catch (SecurityTokenException)
        {
            return BadRequest("Invalid token");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while verifying the email");
        }
    }
}