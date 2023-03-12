using API.BL.Interfaces;
using API.Models.Dto;
using API.Models.Entity;
using API.Services.JWTCreation.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.API;

[ApiController]
[Route("api/dso")]
public class DsoController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IJWTCreator _jwtCreator;
    private readonly IDsoBL _dsoBl;
    
    public DsoController( IDsoBL dsoBl, IConfiguration configuration, IJWTCreator jwtCreator)
    {
        _dsoBl = dsoBl;
        _configuration = configuration;
        _jwtCreator = jwtCreator;
    }
    
    [HttpPost("login")]
    public IActionResult Login(UserLoginDto request)
    {
        string token;

        var response = _dsoBl.CheckForLoginCredentials(request);

        if (response.Success == false)
            return BadRequest(response);

        token = _jwtCreator.CreateToken((User)response.Data);
        return Ok(token);
    }
}