using API.Models;
using API.Models.Dot;
using API.Models.Dto;

namespace API.BL.Interfaces;

public interface IDsoBL
{
    Response<object> CheckForLoginCredentials(UserLoginDto user);
    Response<object> RegisterEmployee(UserRegisterDot user);
}