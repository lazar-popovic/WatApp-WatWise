using API.DAL.Interfaces;
using API.Models.Dto;
using API.Models.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace API.DAL.Implementations;

public class DsoDAL : IDsoDAL
{
    private readonly DataContext _context;

    public DsoDAL( DataContext context)
    {
        _context = context;
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
        /*var userFromBase = _context.Users.First(u => u.Email == user.Email);
        return userFromBase;*/
        using (var command = new SqliteCommand())
        {
            command.Connection = (SqliteConnection?)_context.Database.GetDbConnection();
            command.CommandText = "SELECT * FROM Users WHERE Email = @Email";
            command.Parameters.AddWithValue("@Email", user.Email);

            _context.Database.OpenConnection();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var dbUser = new User
                    {
                        Id = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        PasswordHash = reader.GetString(2),
                        Firstname = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Lastname = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Verified = reader.IsDBNull(5) ? null : reader.GetBoolean(5),
                        RoleId = reader.GetInt32(6),
                        LocationId = reader.IsDBNull(7) ? null : reader.GetInt32(7)
                    };

                    dbUser.Role = _context.Roles.First(r => r.Id == dbUser.RoleId);
                    
                    _context.Database.CloseConnection();
                    return dbUser;
                }
            }
        }

        _context.Database.CloseConnection();
        return null;
    }
}