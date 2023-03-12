using API.BL.Interfaces;
using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models;
using API.Models.Dto;
using API.Models.Entity;

namespace API.BL.Implementations
{
    public class UserLoginBL : BaseBL, IUserLoginBL
    {
        private readonly IBaseDAL _baseDAL;
        private readonly ILoginDAL _loginDAL;
        public UserLoginBL(IBaseDAL baseDAL,ILoginDAL loginDAL)
        {
            _baseDAL = baseDAL;
            _loginDAL = loginDAL;
        }


        public Response<object> CheckForLoginCredentials(UserLoginDto user)
        {
            var response = new Response<object>();

            user.Email = user.Email.Trim();
            user.Password = user.Password.Trim();

            if (user.Email == "")
            {
                response.Errors.Add("Email is required");
            }
            if (_loginDAL.LoginEmailDoesentExists(user.Email))
            {
                response.Errors.Add("User with this email doesent exists");
            }

            response.Success = !response.Errors.Any();

            if (!response.Success)
                return response;

            var verifiedUser = _loginDAL.LoginUser(user);

            if ((bool)!verifiedUser.Verified)
            {
                response.Errors.Add("Your account is not verified");
                response.Success = !response.Errors.Any();
                return response;
            }

            if (verifiedUser.RoleId != 3)
            {
                response.Errors.Add("You are not authorized");
                response.Success = !response.Errors.Any();
                return response;
            }

            if ( !BCrypt.Net.BCrypt.Verify(user.Password, verifiedUser.PasswordHash))
            {
                response.Errors.Add("Sorry, we couldn't verify your password. Please check and try again");
                response.Success = !response.Errors.Any();
                return response;
            }

            response.Data = verifiedUser;
            response.Success = true;
            return response;
        }

    }
}

