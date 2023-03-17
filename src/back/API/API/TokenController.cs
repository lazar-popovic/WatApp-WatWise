using API.DAL.Interfaces;
using API.Models;
using API.Models.Dto;
using API.Models.Entity;
using API.Services.JWTCreation.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.API
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJWTCreator _jwtCreator;
        private readonly DataContext _dataContext;
        public TokenController(IJWTCreator jwtCreator, DataContext dataContext)
        {
            _jwtCreator = jwtCreator;
            _dataContext = dataContext;
        }

        [HttpPost]
        public IActionResult Refresh(RefreshTokenDto refreshTokenDto)
        {
            var response = new Response<object>();

            if (refreshTokenDto is null)
                return BadRequest("Invalid client request");

            string accessToken = refreshTokenDto.Token;
            string refreshToken = refreshTokenDto.RefreshToken;

            var claimsPrincipal = _jwtCreator.GetPrincipalFromExpiredToken(accessToken);
            var emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;



            var user = _dataContext.Users.SingleOrDefault(u => u.Email == emailClaim);
            var refreshTokenFromBase = _dataContext.RefreshTokens.SingleOrDefault(rt => rt.UserId == user.Id);

            if (user is null || refreshTokenFromBase.Token != refreshToken || refreshTokenFromBase.Expires <= DateTime.Now || (refreshTokenFromBase.IsActive == false))
            {
                response.Errors.Add("Token is invalid!");
                response.Success = false;

                return Ok(response);
            }    
                

            var newAccessToken = _jwtCreator.CreateToken(user);
            var newRefreshToken = _jwtCreator.GenerateRefreshToken();

           refreshTokenFromBase.Token = newRefreshToken;
            _dataContext.SaveChanges();

            return Ok(new RefreshTokenDto()
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
