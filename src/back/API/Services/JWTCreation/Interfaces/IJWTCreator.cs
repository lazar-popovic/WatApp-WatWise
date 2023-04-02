using API.Models.Entity;
using System.Security.Claims;

namespace API.Services.JWTCreation.Interfaces
{
    public interface IJWTCreator
    {
        string? CreateToken(User request);

        //string CreateVerificationToken(string email);
        string CreateVerificationToken(int userId);
        string? CreateResetToken(int userId, string userEmail, ResetPasswordToken resetToken);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        int GetUserIdFromToken(string token);
    }
}
