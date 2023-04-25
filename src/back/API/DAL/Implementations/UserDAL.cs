using API.DAL.Interfaces;
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


        public async Task<List<User>?> FindUser(int id, string search, string mail, int pageSize, int pageNum, string order)
        {
          
            var fullName = search?.Trim().ToLower().Split(" ");

            var users = await _dbContext.Users.Where( u => u.RoleId == id).Select(o => new User
                                                {
                                                    Id = o.Id,
                                                    Email = o.Email,
                                                    Firstname = o.Firstname,
                                                    Lastname = o.Lastname,
                                                    Verified = o.Verified,
                                                    LocationId = o.LocationId,
                                                    Location = o.Location

                                                }).ToListAsync();

            if (mail != null && id==3)
            {
                if (!string.IsNullOrEmpty(mail.Trim()))
                {
                    users = users.Where(o =>
                        ($"{o.Location?.Address} {o.Location?.AddressNumber}, {o.Location?.City}".ToLower())
                        .Contains(mail.ToLower())).ToList();
                }
            }

            if (search != null)
            {
                if (fullName.Length == 2)
                {
                    users = users.Where(o => o.Firstname.ToLower().Contains(fullName[0]) && o.Lastname.ToLower().Contains(fullName[1]))
                        .ToList();
                }
                else if (fullName.Length == 1)
                {
                    users = users.Where(o => o.Firstname.ToLower().Contains(fullName[0]) || o.Lastname.ToLower().Contains(fullName[0]))
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

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }
    }


}

