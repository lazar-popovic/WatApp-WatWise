﻿using API.BL.Interfaces;
using API.Models.Dto;
using API.Models.Entity;
using API.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.API
{
    [Route("api/auth")]
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

        [HttpPost("register-user"), Authorize(Roles = "Admin, Employee")]
        public IActionResult RegisterUser(RegisterUserViewModel request)
        {
            return Ok(_authBL.RegisterUser(request));
        }

        
        [HttpPost("register-employee"), Authorize(Roles = "Admin")]
        public IActionResult RegisterEmployee(RegisterEmployeeViewModel request)
        {
            return Ok(_authBL.RegisterEmployee(request));
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgottenPasswordViewModel request)
        {
            return Ok(_authBL.ForgotPassword(request));
        }
        
    }
}
