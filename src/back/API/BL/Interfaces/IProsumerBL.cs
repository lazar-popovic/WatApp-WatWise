using API.Models;
using API.Models.Dot;
using API.Models.Dto;

namespace API.BL.Interfaces;

public interface IProsumerBL
{
    Response<object> RegisterProsumer(UserRegisterDot user);
    Response<object> CheckForLoginCredentials(UserLoginDto user);
    Response<object> CheckEmailForForgottenPassword(ForgottenPasswordRequest request);
    ResetPasswordToken GenerateNewResetPasswordToken(int userID);
}