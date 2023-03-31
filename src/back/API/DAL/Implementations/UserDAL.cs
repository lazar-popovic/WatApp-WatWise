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

        public async Task<User?> GetByIdAsync(int id)
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

                                   }).AsNoTracking().SingleOrDefaultAsync();

            return users;
        }
        public async Task<List<User>?> GetUsers()
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

                                   }).AsNoTracking().ToListAsync();

            return users;
        }
        //Depend on RoleId that method return list of prosumer, 
        public async Task<List<User>?> GetUsersBasedOnRoleAsync(int id, int pageSize, int pageNumber)
        {
            var users = await _dbContext.Users.Where(u => u.RoleId == id)
                                   .OrderBy(u => u.Id)
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
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
        public async Task<List<User>?> GetUsersWithLocationId(int id)
        {
            var users = await _dbContext.Users.Where(u => u.LocationId == id && u.Verified == true)
                                   .Select(u => new User
                                   {
                                       Id = u.Id,
                                       Email = u.Email,
                                       Firstname = u.Firstname,
                                       Lastname = u.Lastname
                                   }).ToListAsync();

            return users;
        }

        public async Task<int> getNumberOfProsumersOrEmployees(int id)
        {
            int numberUsers = _dbContext.Users.Count(u => u.RoleId == id);
            return numberUsers;
        }
            


    }
}
