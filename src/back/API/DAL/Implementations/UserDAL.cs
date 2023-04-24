using API.DAL.Interfaces;
using API.Models;
using API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.DAL.Implementations
{
    public class UserDAL : IUserDAL
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
                    Location = u.Location,
                    ProfileImage = u.ProfileImage
                }).AsNoTracking().SingleOrDefaultAsync();

            return users;
        }

        public async Task<User?> GetByIdWithPasswordAsync(int id)
        {
            var user = await _dbContext.Users.Where(u => u.Id == id)
                .Select(u => new User
                {
                    Id = u.Id,
                    Email = u.Email,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    PasswordHash = u.PasswordHash,
                    Verified = u.Verified,
                    RoleId = u.RoleId,
                    Role = u.Role,
                    LocationId = u.LocationId,
                    Location = u.Location,
                    ProfileImage = u.ProfileImage,
                }).AsNoTracking().SingleOrDefaultAsync();

            return user;
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
                    Location = u.Location,
                    ProfileImage = u.ProfileImage
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
                }).AsNoTracking().ToListAsync();

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
                }).AsNoTracking().ToListAsync();

            return users;
        }

        public async Task<int> getNumberOfProsumersOrEmployees(int id)
        {
            int numberUsers = await _dbContext.Users.AsNoTracking().CountAsync(u => u.RoleId == id);
            return numberUsers;
        }


        public async Task<List<UserWithCurrentProdAndCons?>> FindUser(int id, string search, string mail, int pageSize,
            int pageNum, string order)
        {
            var currentDateTime = DateTime.Now;
            var query = _dbContext.Users
                .Where(u => u.RoleId == id)
                .Select(u => new UserWithCurrentProdAndCons
                {
                    UserId = u.Id,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Location = u.Location,
                    LocationId = u.LocationId,
                    Email = u.Email,
                    Verified = u.Verified,
                    CurrentConsumption = _dbContext.DeviceEnergyUsage
                        .Where(usage => usage.Device.DataShare && usage.Device.UserId == u.Id
                                                               && usage.Device.DeviceType.Category == -1
                                                               && usage.Timestamp.Value.Date == currentDateTime.Date
                                                               && usage.Timestamp.Value.TimeOfDay <=
                                                               currentDateTime.TimeOfDay)
                        .Sum(usage => usage.Value),
                    CurrentProduction = _dbContext.DeviceEnergyUsage
                        .Where(usage => usage.Device.DataShare && usage.Device.UserId == u.Id
                                                               && usage.Device.DeviceType.Category == 1
                                                               && usage.Timestamp.Value.Date == currentDateTime.Date
                                                               && usage.Timestamp.Value.TimeOfDay <=
                                                               currentDateTime.TimeOfDay)
                        .Sum(usage => usage.Value)
                });

            if (mail != null && id == 3 && !string.IsNullOrEmpty(mail.Trim()))
            {
                query = query.Where(u => ($"{u.Location}".ToLower())
                    .Contains(mail.ToLower()));
            }

            if (search != null)
            {   /*
                var fullName = search.Trim().ToLower().Split(" ");
                if (fullName.Length == 2)
                {
                    query = query.Where(u => u.FullName.ToLower().Contains(fullName[0])
                                             && u.FullName.ToLower().Contains(fullName[1]));
                }
                else if (fullName.Length == 1)
                {
                    query = query.Where(u => u.FullName.ToLower().Contains(fullName[0])
                                             || u.FullName.ToLower().Contains(fullName[0]));
                }
                */
            }
            
            /*
            switch (order)
            {
                case "asc":
                    query = query.OrderBy(u => u.FullName);
                    break;
                case "desc":
                    query = query.OrderByDescending(u => u.FullName);
                    break;
            }
            */
            var pagedQuery = query.Skip((pageNum - 1) * pageSize).Take(pageSize);

            var result = await pagedQuery.ToListAsync();

            return result;
        }

        public void UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }

        public async Task<User> SaveProfilePictureAsync(int userId, [FromBody] byte[] profilePicture)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            user.ProfileImage = profilePicture;
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}