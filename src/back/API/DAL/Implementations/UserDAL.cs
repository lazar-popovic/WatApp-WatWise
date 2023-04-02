using API.BL.Implementations;
using API.DAL.Interfaces;
using API.Models.Entity;
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
            int numberUsers = _dbContext.Users.Count(u => u.RoleId == id);
            return numberUsers;
        }


        public async Task<List<User>?> FindUser(int id, string search, string mail, int pageSize, int pageNum, string order)
        {
          
            string[] fullName = search.Split(" ");

           

          
            List<User> users = _dbContext.Users.Select(o => new User
                                                {
                                                    Id = o.Id,
                                                    Email = o.Email,
                                                    Firstname = o.Firstname,
                                                    Lastname = o.Lastname,
                                                    Verified = o.Verified,
                                                    RoleId = o.RoleId,
                                                    Role = o.Role,
                                                    LocationId = o.LocationId,
                                                    Location = o.Location

                                                }).ToList();


            if (!string.IsNullOrEmpty(mail))
            {
                users = users.Where(o => o.Email.Contains(mail)).ToList();
            }

            
            if (fullName.Length == 2)
            {
                users = users.Where(o => o.Firstname.Contains(fullName[0]) && o.Lastname.Contains(fullName[1])).ToList();
            }
            else if (fullName.Length == 1)
            {
                users = users.Where(o => o.Firstname.Contains(fullName[0]) || o.Lastname.Contains(fullName[0])).ToList();
            }
            users = users.Where(o => o.RoleId == id).ToList();

            
            switch (order)
            {
                case "Up":
                    users = users.OrderBy(o => o.Firstname).Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "Down":
                    users = users.OrderByDescending(o => o.Firstname).Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
                    break;
                default:
                    users = users.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
                    break;
            }

            
            return users;
        }



    }


    }

