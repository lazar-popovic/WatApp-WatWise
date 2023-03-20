using API.Models;
using API.Models.Dto;

namespace API.Obsolete;

public interface IDsoBL
{
    Response<object> CheckForLoginCredentials(UserLoginDto user);
    Response<object> RegisterEmployee(UserRegisterDto user);
}