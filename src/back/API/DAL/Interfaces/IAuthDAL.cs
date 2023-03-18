using API.Models.Entity;

namespace API.DAL.Interfaces
{
    public interface IAuthDAL
    {
        User? GetUserWithRoleForEmail(string email);
        bool EmailExists(string email);
    }
}
