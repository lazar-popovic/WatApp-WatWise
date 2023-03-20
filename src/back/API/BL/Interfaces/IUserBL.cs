using API.Models;
using API.Models.Entity;

namespace API.BL.Interfaces
{
    public interface IUserBL
    {
        Response<Task<User>> GetByIdAsync(int id);
        Response<Task<List<User>>> GetUsersBasedOnRoleAsync(int id);
        Response<Task<List<User>>> GetUsers();
    }
}
