using API.Models;
using API.Models.Dto;

namespace API.BL.Interfaces;

public interface IDsoBL
{
    Response<object> CheckForLoginCredentials(UserLoginDto user);
}