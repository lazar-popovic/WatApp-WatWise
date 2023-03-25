using API.Models;
using API.Models.Entity;

namespace API.BL.Interfaces
{
    public interface IUserBL
    {
        Task<Response<User>> GetByIdAsync(int id);
        Task<Response<List<User>>> GetUsersBasedOnRoleAsync(int id);
        Task<Response<List<User>>> GetUsers();
        Task<Response<List<User>>> GetUsersWithLocationId(int id);
    }
}
