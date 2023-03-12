using API.Models.Dto;
using API.Models.Entity;

namespace API.DAL.Interfaces;

public interface IDsoDAL
{
    bool LoginEmailDoesentExists(string email);
    User LoginUser(UserLoginDto user);
}