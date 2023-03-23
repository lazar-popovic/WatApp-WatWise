using API.Models.Entity;

namespace API.DAL.Interfaces
{
    public interface IUserDAL
    {
        Task<User?> GetByIdAsync(int id);
        Task<List<User>?> GetUsersBasedOnRoleAsync(int id);
        Task<List<User>?> GetUsers();
    }
}
