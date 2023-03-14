using API.Models.Entity;

namespace API.DAL.Interfaces
{
    public interface IUserDAL
    {
        Task<User> GetByIdAsync(int id);
        Task<List<User>> GetUsersByIdAsync(int id);
        Task<List<User>> GetUsers();
    }
}
