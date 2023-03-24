using API.BL.Interfaces;
using API.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("login"),AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgottenPasswordViewModel request)
        {
            return Ok(_authBL.ForgotPassword(request));
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordViewModel request)
        {
            return Ok(_authBL.ResetPassword(request));
        }

        [HttpPost("refresh-token"),Authorize]
        public IActionResult RefreshToken(LoginResponseViewModel request)
        {
            return Ok(_authBL.RefreshToken(request));
        }
        
        [HttpPost("verify-token"),AllowAnonymous]
        public IActionResult VerifyToken(VerifyTokenViewModel request) 
        {
            return Ok(_authBL.VerifyToken(request));
        }

        [HttpPost("verify-account"),AllowAnonymous]
        public IActionResult VerifyAccount(VerifyAccountViewModel request)
        {
            return Ok(_authBL.VerifyAccount(request));
        }

        [HttpPost("resend-verify-email")]
        public IActionResult ResendVerifyEmail(ResendVerifyEmailViewModel request)
        {
            return Ok(_authBL.ResendVerifyEmail(request));
        }
    }
}
