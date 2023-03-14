using API.BL.Interfaces;
using API.Models.Dto;
using API.Models.Entity;
using API.Services.E_mail;
using API.Services.E_mail.Interfaces;
using API.Services.JWTCreation.Implementations;
using API.Services.JWTCreation.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.API;

[ApiController]
[Route("api/prosumer")]
public class ProsumerController : ControllerBase
{
    private readonly IProsumerBL _prosumerBl;
    private readonly IConfiguration _configuration;
    private readonly IJWTCreator _jwtCreator;
    private readonly IMailService _mailService;
   
    public ProsumerController( IProsumerBL prosumerBl, IConfiguration configuration, IJWTCreator jwtCreator, IMailService mailService)
    {
        _prosumerBl = prosumerBl;
        _configuration = configuration;
        _jwtCreator = jwtCreator;
        _mailService = mailService;
       
    }

    [HttpPost("register"),Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterProsumer(UserRegisterDto request)
    {
        var response = _prosumerBl.RegisterProsumer(request);

        if (response.Success)
        {
            return Ok(response);
        }
        else
        {
            return BadRequest(response);
        }
    }

    //dodati autorizaciju
    [HttpPost("login")]
    public IActionResult Login(UserLoginDto request)
    {
        string token;

        var response = _prosumerBl.CheckForLoginCredentials(request);

        if (response.Success == false)
            return BadRequest(response);

        token = _jwtCreator.CreateToken((User)response.Data);
        
        return Ok( new TokenDto{ Token = token});
    }

    [HttpPost]
    [Route("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgottenPasswordRequestDto request)
    {
        var response = _prosumerBl.CheckEmailForForgottenPassword(request);
        var user = ((User)response.Data);

        if (response == null || user == null)
            return BadRequest("User with this email doesen't exist!");

        var responseForFront = new Response<object>()
        {
            Errors = response.Errors,
            Success = response.Success
        };
            
        // Generate reset token
        var resetToken = _prosumerBl.GenerateNewResetPasswordToken(user.Id);

        var resetJwtToken = _jwtCreator.CreateResetToken(user.Id, user.Email,resetToken);

        //send token via email service
        _mailService.sendResetTokenProsumer(user, resetJwtToken);

        responseForFront.Success = !responseForFront.Errors.Any();

        return Ok(responseForFront);
    }

    [HttpPost]
    [Route("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto request)
    {
        var response = new Response<object>();

        if (request.NewPassword == "" || request.ConfirmedNewPassword == "")
        {
            //response.Errors.Add("Please enter new password and confirm it!");
            return BadRequest("Please enter new password and confirm it!");
        }

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(request.Token);

        var resetToken = token.Claims.FirstOrDefault(c => c.Type == "resetToken")?.Value;

        if (string.IsNullOrEmpty(resetToken))
        {
            //response.Errors.Add("Token is null or empty!");
            return BadRequest("Invalid token");
        }

        var tokenEntity = _prosumerBl.GetResetTokenEntity(resetToken);

        if (tokenEntity == null)
        {
            //response.Errors.Add("Invalid token!");
            return BadRequest("Invalid token");
        }

        if (DateTime.UtcNow > tokenEntity.ExpiryTime)
        {
            //response.Errors.Add("Token expired!");
            return BadRequest("Token expired");
        }

        var user = _prosumerBl.FindUserByIdFromTokenEntity((int)tokenEntity.UserId);

        response = _prosumerBl.CheckForOldPasswordWhenResettingPass(request.OldPassword, user);

        if (user == null)
        {
            //response.Errors.Add("User not found!");
            return BadRequest("User not found");
        }

        
        response.Success = !response.Errors.Any();

        if (response.Success == false)
        {
            response.Data = null;
            return BadRequest(response);
        }
            
        
        // Update password
        _prosumerBl.SetNewPasswordAfterResetting(user, request.NewPassword);

        // Delete reset token
        _prosumerBl.RemovePasswordResetToken(tokenEntity);

        return Ok("Password has been changed successfully");

    }

}
