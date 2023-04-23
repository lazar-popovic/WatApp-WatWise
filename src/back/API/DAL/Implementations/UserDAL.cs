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
                                       ProfileImage = u.ProfileImage
                                    ,
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
        var fullName = search.Trim().ToLower().Split(" ");
        var userListWithEnergyUsage = await _dbContext.Users
            .Where(u => u.RoleId == id &&
                   (mail == null || id != 3 || $"{u.Location.Address} {u.Location.AddressNumber}, {u.Location.City}".ToLower().Contains(mail.ToLower())) &&
                   (search == null ||
                        (fullName.Length == 2 && u.Firstname.ToLower().Contains(fullName[0]) && u.Lastname.ToLower().Contains(fullName[1])) ||
                        (fullName.Length == 1 && (u.Firstname.ToLower().Contains(fullName[0]) || u.Lastname.ToLower().Contains(fullName[0])))))
            .Select(u => new UserWithCurrentProdAndCons()
            {
                UserId = u.Id,
                FullName = $"{u.Firstname} {u.Lastname}",
                Location = $"{u.Location.Address} {u.Location.AddressNumber}, {u.Location.City}",
                LocationId = u.LocationId,
                Email = $"{u.Email}",
                Verified = u.Verified,
                CurrentConsumption = (from usage in _dbContext.DeviceEnergyUsage
                                      join device in _dbContext.Devices on usage.DeviceId equals device.Id
                                      join deviceType in _dbContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
                                      where device.DataShare && device.UserId == u.Id
                                      && deviceType.Category == -1
                                      && usage.Timestamp.Value.Date == currentDateTime.Date
                                      && usage.Timestamp.Value.TimeOfDay <= currentDateTime.TimeOfDay
                                      select usage.Value).Sum(),
                CurrentProduction = (from usage in _dbContext.DeviceEnergyUsage
                                     join device in _dbContext.Devices on usage.DeviceId equals device.Id
                                     join deviceType in _dbContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
                                     where device.DataShare && device.UserId == u.Id
                                     && deviceType.Category == 1
                                     && usage.Timestamp.Value.Date == currentDateTime.Date
                                     && usage.Timestamp.Value.TimeOfDay <= currentDateTime.TimeOfDay
                                     select usage.Value).Sum(),
            })
            .ToListAsync();

        /*
            switch (order)
            {
                case "asc":
                    userListWithEnergyUsage = userListWithEnergyUsage.OrderBy(o => o.FullName).Skip((pageNum - 1) * pageSize).Take(pageSize);
                    break;
                case "desc":
                    users = (List<User>)users.OrderByDescending(o => o.Lastname).Skip((pageNum - 1) * pageSize).Take(pageSize);
                    break;
                default:
                    users = (List<User>)users.Skip((pageNum - 1) * pageSize).Take(pageSize);
                    break;
            }
*/
            return userListWithEnergyUsage;
        }

        public void UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }
        public async Task<User> SaveProfilePictureAsync(int userId, [FromBody]byte[] profilePicture)
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

