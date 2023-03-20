using API.Models;
using API.Models.ViewModels;

namespace API.BL.Interfaces
{
    public interface IAuthBL
    {
        Response<LoginResponseViewModel> Login(LoginViewModel user);
        Response<RegisterResponseViewModel> RegisterUser(RegisterUserViewModel user);
        Response<RegisterResponseViewModel> RegisterEmployee(RegisterEmployeeViewModel user);
        Response<RegisterResponseViewModel> ForgotPassword(ForgottenPasswordViewModel request);
        Response<RegisterResponseViewModel> ResetPassword(ResetPasswordViewModel request);
        Response<LoginResponseViewModel> RefreshToken(LoginResponseViewModel request);
        Response<RegisterResponseViewModel> VerifyToken(VerifyTokenViewModel request);
        Response<RegisterResponseViewModel> VerifyAccount(VerifyAccountViewModel request);

    }
}
