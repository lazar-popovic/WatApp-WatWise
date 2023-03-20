using API.Models.Entity;
using API.Models.ViewModels;

namespace API.DAL.Interfaces
{
    public interface IAuthDAL
    {
        User? GetUserWithRoleForEmail(string email);
        User RegisterUser(RegisterUserViewModel user);
        User RegisterEmployee(RegisterEmployeeViewModel user);
        bool EmailExists(string email);
        void AddResetToken(ResetPasswordToken token);
        ResetPasswordToken GetResetTokenEntity(string type);
        User? FindUserById(int userId);
        void UpdateUserAfterPasswordReset(User user);
        void RemoveResetToken(ResetPasswordToken token);
        RefreshToken GetRefreshToken(int userID);
        void DeactivatePreviousRefreshTokensAndSaveNewToBase(int userId, string token);
    }
}
