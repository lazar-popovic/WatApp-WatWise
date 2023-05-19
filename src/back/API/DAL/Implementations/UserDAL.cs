using API.Common;
using API.DAL.Interfaces;
using API.Models.DTOs;
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


        public async Task<object> FindUser(int id, string search, string mail, int pageSize, int pageNum, string order)
        {
          
            var fullName = search?.Trim().ToLower().Split(" ");
            var users = await ProsumersWithConsumptionProductionAndNumberOfWorkingDevices();

            if (mail != null && id==3)
            {
                if (!string.IsNullOrEmpty(mail.Trim()))
                {
                    users = users.Where(o =>
                        ($"{o.Location!.Address} {o.Location!.AddressNumber}, {o.Location!.City}".ToLower())
                        .Contains(mail.ToLower())).ToList();
                }
            }

            if (search != null)
            {
                if (fullName!.Length == 2)
                {
                    users = users.Where(o => o.Firstname!.ToLower().Contains(fullName[0]) && o.Lastname!.ToLower().Contains(fullName[1]))
                        .ToList();
                }
                else if (fullName.Length == 1)
                {
                    users = users.Where(o => o.Firstname!.ToLower().Contains(fullName[0]) || o.Lastname!.ToLower().Contains(fullName[0]))
                        .ToList();
                }
            }

            switch (order)
            {
                case "asc":
                    users = users.OrderBy(o => o.Lastname).Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "desc":
                    users = users.OrderByDescending(o => o.Lastname).Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
                    break;
                default:
                    users = users.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
                    break;
            }

            return users;
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

        public async Task DeleteUser(User user)
        {
            await using var context = _dbContext;
            var userToDelete = await context.Users.Include(u => u.Devices)!
                .ThenInclude(d => d.DeviceEnergyUsages)
                .SingleOrDefaultAsync(u => u.Id == user.Id);

            if (userToDelete != null)
            {
                context.DeviceEnergyUsage.RemoveRange(userToDelete.Devices.SelectMany(d => d.DeviceEnergyUsages));
                context.Devices.RemoveRange(userToDelete.Devices);
                
                var refreshTokensToDelete = context.RefreshTokens.Where(rt => rt.UserId == user.Id);
                context.RefreshTokens.RemoveRange(refreshTokensToDelete);
                
                context.Users.Remove(userToDelete);
                await context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User?> DeleteProfilePictureAsync(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user != null)
            {
                
                string defaultImagePath = Path.Combine("Images", "defaultavatar.jpg");
                var defaultImageData = await File.ReadAllBytesAsync(defaultImagePath);

              
                user.ProfileImage = defaultImageData;

               
                await _dbContext.SaveChangesAsync();
            }

            return user;
        }
        public async Task<List<AllProsumersWithConsumptionProductionDTO>> ProsumersWithConsumptionProductionAndNumberOfWorkingDevices()
        {
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

            var userDTOs = await _dbContext.Users
                .Where(u => u.RoleId == (int?)RoleEnum.Role.User)
                .Select(u => new
                {
                    User = u,
                    Devices = u.Devices.Where(d => d.ActivityStatus == true && d.DataShare == true),
                    EnergyUsage = u.Devices
                        .SelectMany(d => d.DeviceEnergyUsages)
                        .Where(eu => eu.Timestamp == now)
                })
                .Select(u => new AllProsumersWithConsumptionProductionDTO()
                {
                    Id = u.User.Id,
                    Email = u.User.Email,
                    Firstname = u.User.Firstname,
                    Lastname = u.User.Lastname,
                    Verified = u.User.Verified,
                    LocationId = u.User.LocationId,
                    Location = u.User.Location,
                    CurrentConsumption = u.EnergyUsage
                        .Where(eu => eu.Device!.DeviceType!.Category == -1)
                        .Sum(eu => eu.Value),
                    PredictedCurrentConsumption = u.EnergyUsage
                        .Where(eu => eu.Device!.DeviceType!.Category == -1)
                        .Sum(eu => eu.PredictedValue),
                    CurrentProduction = u.EnergyUsage
                        .Where(eu => eu.Device!.DeviceType!.Category == 1)
                        .Sum(eu => eu.Value),
                    PredictedCurrentProduction = u.EnergyUsage
                        .Where(eu => eu.Device!.DeviceType!.Category == 1)
                        .Sum(eu => eu.PredictedValue),
                    ConsumingDevicesTurnedOn = u.Devices
                        .Where(d => d.DeviceType!.Category == -1 && d.ActivityStatus == true)
                        .Count(),
                    ProducingDevicesTurnedOn = u.Devices
                        .Where(d => d.DeviceType!.Category == -1 && d.ActivityStatus == true)
                        .Count()
                })
                .OrderByDescending(u => u.CurrentConsumption)
                .ThenByDescending(u => u.CurrentProduction)
                .ToListAsync();

            return userDTOs;

        }
    }


}

