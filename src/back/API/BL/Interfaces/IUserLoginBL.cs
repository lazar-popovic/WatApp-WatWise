using API.Models.Dto;
using API.Models;

namespace API.BL.Interfaces
{
    public interface IUserLoginBL : IBaseBL
    {
        Response<object> CheckForLoginCredentials(UserLoginDto user);
    }
}
