using API.Models.Entity;
using API.Models.ViewModels;

namespace API.DAL.Interfaces
{
    public interface IAuthDAL
    {
        User? GetUserWithRoleForEmail(string email);
        User RegisterUser(RegisterUserViewModel user);
        bool EmailExists(string email);
    }
}
