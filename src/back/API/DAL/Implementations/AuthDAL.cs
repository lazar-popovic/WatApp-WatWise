﻿using API.DAL.Interfaces;
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

        
        public void AddResetToken(ResetPasswordToken token)
        {
            _context.ResetPasswordTokens.Add(token);
            _context.SaveChanges();
        }

        public ResetPasswordToken GetResetTokenEntity(string type)
        {
            var token = _context.ResetPasswordTokens.SingleOrDefault(t => t.Token == type);

            if (token != null)
                return token;

            return null;
        }

        public User? FindUserById(int userId)
        {

            return _context.Users.SingleOrDefault(u => u.Id == userId);
        }

        public void UpdateUserAfterPasswordReset(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void RemoveResetToken(ResetPasswordToken token)
        {
            _context.ResetPasswordTokens.Remove(token);
            _context.SaveChanges();
        }
    }
}
