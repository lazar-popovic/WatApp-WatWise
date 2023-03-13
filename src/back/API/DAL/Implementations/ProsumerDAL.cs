using API.DAL.Interfaces;
using API.Models.Dot;
using API.Models.Dto;
using API.Models.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace API.DAL.Implementations;

public class ProsumerDAL : IProsumerDAL
{
    private readonly DataContext _context;

    public ProsumerDAL( DataContext context)
    {
        _context = context;
    }

    public bool EmailExists(string email)
    {
        return _context.Users.Any(u => u.Email == email);
    }

    public User RegisterUser(UserRegisterDot user)
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

        var newUserId = _context.Entry(newUser).Property(x => x.Id).CurrentValue;
        newUser.Id = newUserId;

        return newUser;
    }

    public bool LoginEmailDoesentExists(string email)
    {
        bool emailExists = _context.Users.Any(u => u.Email == email);

        if (!emailExists)
        {
            return true;
        }
        else
            return false;
    }

    public User LoginUser(UserLoginDto user)
    {
        var userFromBase = _context.Users.First(u => u.Email == user.Email);
        userFromBase.Role = _context.Roles.First(r => r.Id == userFromBase.RoleId);
        return userFromBase;
    }
}