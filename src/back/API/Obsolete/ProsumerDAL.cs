using API.Models.Dto;
using API.Models.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace API.Obsolete;

public class ProsumerDAL : IProsumerDAL
{
    private readonly DataContext _context;

    public ProsumerDAL(DataContext context)
    {
        _context = context;
    }

    public bool EmailExists(string email)
    {
        return _context.Users.Any(u => u.Email == email);
    }

    public User UserForGivenEmail(string email)
    {
        var userFromBase = _context.Users.FirstOrDefault(user => user.Email == email);

        if (userFromBase == null)
            return null;

        return userFromBase;
    }

    public User RegisterUser(UserRegisterDto user)
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

    public void AddResetToken(ResetPasswordToken token)
    {
        _context.ResetPasswordTokens.Add(token);
        _context.SaveChanges();
    }

    public ResetPasswordToken GetResetTokenEntityFromBase(string type)
    {
        var token = _context.ResetPasswordTokens.SingleOrDefault(t => t.Token == type);

        if (token != null)
            return token;

        return null;
    }

    public User FindUserByIdFromTokenEntityFromBase(int id)
    {
        return _context.Users.Find(id);
    }

    public void UpdateUserAfterPasswordReset(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void RemovePasswordResetTokenFromBase(ResetPasswordToken resetPasswordToken)
    {
        _context.ResetPasswordTokens.Remove(resetPasswordToken);
        _context.SaveChanges();
    }

    public User GetUserById(int id)
    {
        return _context.Users.FirstOrDefault(user => user.Id == id);
    }

    public void SaveRefreshTokenInBase(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        _context.SaveChanges();
    }

    public void DeactivateRefreshToken(int userId)
    {
        var flags = _context.RefreshTokens.Where(f => f.UserId == userId);

        foreach (var flag in flags)
        {
            flag.IsActive = false;
            flag.IsExpired = true;
        }

        _context.SaveChanges();
    }
}