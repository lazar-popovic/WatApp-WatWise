using API.BL.Interfaces;
using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models;
using API.Models.Dto;
using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.E_mail.Interfaces;
using API.Services.JWTCreation.Interfaces;

namespace API.BL.Implementations
{
    public class AuthBL : IAuthBL
    {
        private readonly IAuthDAL _authDAL;
        private readonly IJWTCreator _jwtCreator;
        private readonly IMailService _mailService;

        public AuthBL(IAuthDAL authDAL, IJWTCreator jwtCreator, IMailService mailService)
        {
            _authDAL = authDAL;
            _jwtCreator = jwtCreator;
            _mailService = mailService;
        }

        public Response<LoginResponseViewModel> Login(LoginViewModel loginRequest)
        {
            var response = new Response<LoginResponseViewModel>();

            if (string.IsNullOrEmpty(loginRequest.Email.Trim()))
            {
                response.Errors.Add("Email is required");
                response.Success = false;
                return response;
            }

            var userWithRole = _authDAL.GetUserWithRoleForEmail(loginRequest.Email);

            response = ValidateUserWithRole(userWithRole);

            if (response.Success == false)
            {
                return response;
            }

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, userWithRole!.PasswordHash))
            {
                response.Errors.Add("Wrong password. Please check and try again");
                response.Success = false;
                return response;
            }
            response = GenerateToken(userWithRole!);
   
            return response;
        }

        public Response<RegisterResponseViewModel> RegisterUser(RegisterUserViewModel userRegisterRequest)
        {
            var response = new Response<RegisterResponseViewModel>();

            if (string.IsNullOrEmpty(userRegisterRequest.Email.Trim()))
            {
                response.Errors.Add("Email is required");
            }

            if (string.IsNullOrEmpty(userRegisterRequest.Firstname.Trim()))
            {
                response.Errors.Add("Firstname is required");
            }

            if (string.IsNullOrEmpty(userRegisterRequest.Lastname.Trim()))
            {
                response.Errors.Add("Lastname is required");
            }

            if (_authDAL.EmailExists(userRegisterRequest.Email))
                response.Errors.Add("User with this email already exists");

            response.Success = response.Errors.Count == 0;

            if (!response.Success)
            {
                return response;
            }


            User newUser = _authDAL.RegisterUser(userRegisterRequest);
            newUser.Role = new Role { Id = 3, RoleName = "User" };

            _mailService.sendTokenProsumer(newUser);

            response.Data = new RegisterResponseViewModel { Message = "Registration successful" };

            return response;

        }

        #region private

        private Response<LoginResponseViewModel> ValidateUserWithRole(User? userWithRole)
        {
            var response = new Response<LoginResponseViewModel>();

            if (userWithRole == null)
            {
                response.Errors.Add("User with this email does not exists");
            }
          
            if (userWithRole?.Verified == false)
            {
                response.Errors.Add("This user is not verified. Please check your e-mail.");
            }


            response.Success = response.Errors.Count == 0;
            return response;
        }

        private Response<LoginResponseViewModel> GenerateToken(User userWithRole)
        {
            var response = new Response<LoginResponseViewModel>();
            var token = _jwtCreator.CreateToken(userWithRole!);
            var refreshToken = _jwtCreator.GenerateRefreshToken();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
            {
                response.Errors.Add("Something went wrong. Please try again later.");
            }
            else
            {
                var responseToken = new LoginResponseViewModel { RefreshToken = refreshToken, Token = token };
                response.Data = responseToken;
            }

            response.Success = response.Errors.Count == 0;
            return response;
        }

        #endregion
    }
}
