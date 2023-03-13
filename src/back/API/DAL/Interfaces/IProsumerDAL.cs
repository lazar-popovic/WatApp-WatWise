using API.Models.Dot;
using API.Models.Dto;
using API.Models.Entity;

namespace API.DAL.Interfaces;

public interface IProsumerDAL
{
    bool EmailExists(string email);
    User RegisterUser(UserRegisterDot user);
    bool LoginEmailDoesentExists(string email);
    User LoginUser(UserLoginDto user);
    User UserForGivenEmail(string email);
}