using API.Models.Dot;

namespace API.DAL.Interfaces;

public interface IProsumerDAL
{
    bool EmailExists(string email);
    void RegisterUser(UserRegisterDot user);
}