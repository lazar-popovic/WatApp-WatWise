using API.Models.Dto;
using API.Models.Entity;

namespace API.Obsolete;

public interface IDsoDAL
{
    bool LoginEmailDoesentExists(string email);
    User LoginUser(UserLoginDto user);
    User RegisterEmployee(UserRegisterDto user);
    bool EmailExists(string email);
}