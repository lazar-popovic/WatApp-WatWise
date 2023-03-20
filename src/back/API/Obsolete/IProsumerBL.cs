using API.Models;
using API.Models.Dto;
using API.Models.Entity;
using API.Models.ViewModels;

namespace API.Obsolete;

public interface IProsumerBL
{
    Response<object> RegisterProsumer(UserRegisterDto user);
    Response<object> CheckForLoginCredentials(UserLoginDto user);
    Response<object> CheckEmailForForgottenPassword(ForgottenPasswordViewModel request);
    ResetPasswordToken GenerateNewResetPasswordToken(int userID);
    ResetPasswordToken GetResetTokenEntity(string type);
    User FindUserByIdFromTokenEntity(int id);
    void SetNewPasswordAfterResetting(User user, string password);
    void RemovePasswordResetToken(ResetPasswordToken resetToken);
    Response<object> CheckForOldPasswordWhenResettingPass(string oldPassword, User user);
    void SetRefreshToken(RefreshToken refreshToken);
    void DeactivatePreviousRefreshTokensOnCreationOfNewRefreshTOken(int userId);
}