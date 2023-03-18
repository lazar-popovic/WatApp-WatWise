using API.BL.Interfaces;
using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models;
using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.JWTCreation.Interfaces;

namespace API.BL.Implementations
{
    public class AuthBL : IAuthBL
    {
        private readonly IAuthDAL _authDAL;
        private readonly IJWTCreator _jwtCreator;

        public AuthBL(IAuthDAL authDAL, IJWTCreator jwtCreator)
        {
            _authDAL = authDAL;
            _jwtCreator = jwtCreator;
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
