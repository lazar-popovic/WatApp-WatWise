using API.DAL.Interfaces;
using API.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace API.DAL.Implementations
{
    public class UserDAL:IUserDAL
    {
        private readonly DataContext _dbContext;

        public UserDAL(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var users = await _dbContext.Users.Where(u => u.Id == id)
                                   .Select(u => new User
                                   {
                                       Id = u.Id,
                                       Email = u.Email,
                                       Firstname = u.Firstname,
                                       Lastname = u.Lastname,
                                       Verified = u.Verified,
                                       RoleId = u.RoleId,
                                       Role = u.Role,
                                       LocationId = u.LocationId,
                                       Location = u.Location

                                   }).SingleOrDefaultAsync();

            return users;
        }
        public async Task<List<User>> GetUsers()
        {
            var users = await _dbContext.Users
                                   .Select(u => new User
                                   {
                                       Id = u.Id,
                                       Email = u.Email,
                                       Firstname = u.Firstname,
                                       Lastname = u.Lastname,
                                       Verified = u.Verified,
                                       RoleId = u.RoleId,
                                       Role = u.Role,
                                       LocationId = u.LocationId,
                                       Location = u.Location

                                   }).ToListAsync();

            return users;
        }
        //Depend on RoleId that method return list of prosumer, 
        public async Task<List<User>> GetUsersByIdAsync(int id)
        {
            var users = await _dbContext.Users.Where(u => u.RoleId == id)
                                   .Select(u => new User
                                   {
                                       Id = u.Id,
                                       Email = u.Email,
                                       Firstname = u.Firstname,
                                       Lastname = u.Lastname, 
                                       Verified = u.Verified,
                                       RoleId = u.RoleId,
                                       Role = u.Role,
                                       LocationId = u.LocationId,
                                       Location = u.Location

                                   }).ToListAsync();

            return users;
        }

    }
}
