using API.Models;
using API.Models.Dto;
using API.Models.Entity;

namespace API.BL.Interfaces;

public interface IProsumerBL
{
    Response<object> RegisterProsumer(UserRegisterDto user);
    Response<object> CheckForLoginCredentials(UserLoginDto user);
    Response<object> CheckEmailForForgottenPassword(ForgottenPasswordRequestDto request);
    ResetPasswordToken GenerateNewResetPasswordToken(int userID);
    ResetPasswordToken GetResetTokenEntity(string type);
    User FindUserByIdFromTokenEntity(int id);
    void SetNewPasswordAfterResetting(User user, string password);
    void RemovePasswordResetToken(ResetPasswordToken resetToken);
    Response<object> CheckForOldPasswordWhenResettingPass(string oldPassword, User user);
}