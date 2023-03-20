using API.Models.Dto;
using API.Models.Entity;

namespace API.Obsolete;

public interface IProsumerDAL
{
    bool EmailExists(string email);
    User RegisterUser(UserRegisterDto user);
    bool LoginEmailDoesentExists(string email);
    User LoginUser(UserLoginDto user);
    User UserForGivenEmail(string email);
    void AddResetToken(ResetPasswordToken token);
    ResetPasswordToken GetResetTokenEntityFromBase(string type);
    User FindUserByIdFromTokenEntityFromBase(int id);
    void UpdateUserAfterPasswordReset(User user);
    void RemovePasswordResetTokenFromBase(ResetPasswordToken resetPasswordToken);
    User GetUserById(int id);
    void SaveRefreshTokenInBase(RefreshToken refreshToken);
    void DeactivateRefreshToken(int userId);
}