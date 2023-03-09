using API.Models;
using API.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseAuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BaseAuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("login")]
        public ActionResult Login(UserDto request)
        {
            User user = new User();
            user.Email = "usermail";
            user.PasswordHash  = BCrypt.Net.BCrypt.HashPassword("userpass");

            if (user.Email != request.Email)
            {
                return BadRequest("User doesen't exist");
            }

           if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password");
            }

            string token = CreateToken(request);

            return Ok(token);
        }

        private string CreateToken(UserDto request) 
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.Role, "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
