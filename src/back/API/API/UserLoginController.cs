using API.BL.Interfaces;
using API.Models.Dto;
using API.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace API.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserLoginBL _userLoginBL;
        public UserLoginController(IConfiguration configuration, IUserLoginBL userLoginBL)
        {
            _configuration = configuration;
            _userLoginBL = userLoginBL;
        }

        //dodati autorizaciju
        [HttpPost("login")]
        public IActionResult Login(UserLoginDto request)
        {
            string token;

            var response = _userLoginBL.CheckForLoginCredentials(request);

            if(response.Success == false)
                return BadRequest(response);


            token = CreateToken((User)response.Data);
            return Ok(token);
            
        }

        private string CreateToken(User request) 
        {
            List<Claim> claims = new List<Claim> {
                /*
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.Role, "User")
                */
                new Claim("UserID", request.Id.ToString()),
                new Claim("RoleID", request.RoleId.ToString()),
                new Claim("Rolename", request.Role.RoleName)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "login claims");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            /*
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            */
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "http://localhost:5226",
                Audience = "http://localhost:5226",
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha512Signature)
            };

            /*
            var token = new JwtSecurityToken(
                    claims: principal,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );*/

            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            token.Payload.AddClaims(principal.Claims);

            //var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            var jwt = tokenHandler.WriteToken(token);


            return jwt;
        }
    }
}
