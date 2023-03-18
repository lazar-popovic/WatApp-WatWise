using API.DAL.Interfaces;
using API.Models;
using API.Models.Dto;
using API.Models.Entity;
using API.Services.JWTCreation.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        [Route("refresh")]
        public IActionResult Refresh(RefreshTokenDto refreshTokenDto)
        {
            var response = new Response<object>();
            ClaimsPrincipal claimsPrincipal;
            string emailClaim;

            if (refreshTokenDto is null)
                return BadRequest("Invalid client request");

            string accessToken = refreshTokenDto.Token;
            string refreshToken = refreshTokenDto.RefreshToken;
            
            try
            {
                claimsPrincipal = _jwtCreator.GetPrincipalFromExpiredToken(accessToken);
                emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
            }
            catch(SecurityTokenValidationException invalidTokenExc)
            {
                response.Errors.Add(invalidTokenExc.Message);
                response.Success = false;

                return Ok(response);
            }

            var user = _dataContext.Users.SingleOrDefault(u => u.Email == emailClaim);
            user.Role = _dataContext.Roles.First(r => r.Id == user.RoleId);

            var refreshTokenFromBase = _dataContext.RefreshTokens.SingleOrDefault(rt => rt.UserId == user.Id && rt.IsActive);

            if (user == null || refreshTokenFromBase.Token != refreshToken || refreshTokenFromBase.Expires <= DateTime.Now || (refreshTokenFromBase.IsActive == false))
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

        /*
        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var response = new Response<object>();
            ClaimsPrincipal claimsPrincipal;
            string emailClaim;

            try
            {
                claimsPrincipal = _jwtCreator.GetPrincipalFromExpiredToken(accessToken);
                emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
            }
            catch (SecurityTokenValidationException invalidTokenExc)
            {
                response.Errors.Add(invalidTokenExc.Message);
                response.Success = false;

                return Ok(response);
            }
        }
        */
    }
}
