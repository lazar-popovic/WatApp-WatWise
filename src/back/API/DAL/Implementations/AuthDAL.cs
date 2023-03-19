using API.DAL.Interfaces;
using API.Models.Entity;
using API.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API.DAL.Implementations
{
    public class AuthDAL : IAuthDAL
    {
        private readonly DataContext _context;

        public AuthDAL(DataContext context)
        {
            _context = context;
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public User? GetUserWithRoleForEmail(string email)
        {
            var query = from u in _context.Users
                        join r in _context.Roles
                        on u.RoleId equals r.Id
                        where u.Email == email
                        select new User {
                            Id = u.Id,
                            Email = u.Email,
                            PasswordHash = u.PasswordHash,
                            Verified = u.Verified,
                            Role = r};
        
            return query.AsNoTracking().FirstOrDefault();
        }

        public User RegisterEmployee(RegisterEmployeeViewModel user)
        {
            var newUser = new User
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                RoleId = 2,
                LocationId = null,
                Verified = false
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return newUser;
        }

        public User RegisterUser(RegisterUserViewModel user)
        {
            var newUser = new User
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                RoleId = 3,
                LocationId = null,
                Verified = false
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return newUser;
        }
    }
}
