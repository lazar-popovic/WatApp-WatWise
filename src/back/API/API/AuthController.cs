using API.BL.Interfaces;
using API.Models.Dto;
using API.Models.Entity;
using API.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthBL _authBL;

        public AuthController(IAuthBL authBL)
        {
            _authBL = authBL;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginViewModel request)
        {
            return Ok(_authBL.Login(request));
        }
    }
}
