using System.IdentityModel.Tokens.Jwt;
using System.Text;
using API.Models;
using API.Models.Dto;
using API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.API;

[ApiController]
[Route("api/auth")]
public class VerificationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly DataContext _context;

    public VerificationController(IConfiguration configuration, DataContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost]
    [Route("validate-token")]
    public async Task<IActionResult> VerifyToken( string token)
    {
        var response = new Response<string>();
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

            var user = await _context.Users.FirstAsync(u => u.Id == Convert.ToInt32( userId));

            response.Success = true;
            response.Data = user.Email;
            
            return Ok(response);
            //return Ok("Email verification successful");
        }
        catch (SecurityTokenException)
        {
            response.Errors.Add("Invalid token");
            return BadRequest(response);
            //return BadRequest("Invalid token");
        }
        catch (Exception)
        {
            response.Errors.Add("An error occurred while verifying the email");
            return BadRequest(response);
            //return StatusCode(500, "An error occurred while verifying the email");
        }
    }

    [HttpPost]
    [Route("verify-account")]
    public async Task<IActionResult> VerifyAccount(VerifyAccountDto request)
    {
        var response = new Response<string>();

        var userId = this.GetUserIdFromToken(request.Token);
        
        request.Password1 = request.Password1.Trim();
        request.Password2 = request.Password2.Trim();
        if (string.IsNullOrEmpty(request.Password1))
        {
            response.Errors.Add("Password is required");
        }
        if (string.IsNullOrEmpty(request.Password2))
        {
            response.Errors.Add("Repeat password is required");
        }
        if (request.Password1 != request.Password2)
        {
            response.Errors.Add("Passwords need to match");
        }
        if (userId == null)
        {
            response.Errors.Add("Invalid token!");
        }
        
        response.Success = response.Errors.Count == 0;

        if (!response.Success)
        {
            return BadRequest(response.Errors);
        }

        var user = await _context.Users.FirstAsync(u => u.Id == userId);

        user.Verified = true;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password1);

        await _context.SaveChangesAsync();

        response.Data = "Verification successful";

        return Ok(response.Data);
    }
    
    private int? GetUserIdFromToken(string token)
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

            if (int.TryParse(userId, out int id))
            {
                return id;
            }
        }
        catch (SecurityTokenException)
        {
            // Invalid token
        }
        catch (Exception)
        {
            // Other error
        }
    
        return null;
    }
}