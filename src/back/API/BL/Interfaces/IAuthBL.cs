using API.Models.Dto;
using API.Models;
using API.Models.ViewModels;

namespace API.BL.Interfaces
{
    public interface IAuthBL
    {
        Response<LoginResponseViewModel> Login(LoginViewModel user);
        Response<RegisterResponseViewModel> RegisterUser(RegisterUserViewModel user);
    }
}
