using API.BL.Interfaces;
using API.Models.Dot;
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
   
    public ProsumerController( IProsumerBL prosumerBl, IConfiguration configuration, IJWTCreator jwtCreator)
    {
        _prosumerBl = prosumerBl;
        _configuration = configuration;
        _jwtCreator = jwtCreator;
       
    }

    [HttpPost("register"),Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterProsumer(UserRegisterDot request)
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
        return Ok(token);
    }




}
