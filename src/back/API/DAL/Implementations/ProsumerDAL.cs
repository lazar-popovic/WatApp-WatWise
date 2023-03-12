using API.DAL.Interfaces;
using API.Models.Dot;
using API.Models.Entity;
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

    public async void RegisterUser(UserRegisterDot user)
    {
        var newUser = new User
        {
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            RoleId = 3,
            LocationId = null,
            Verified = false,
            Authenticated = false
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
    }
}