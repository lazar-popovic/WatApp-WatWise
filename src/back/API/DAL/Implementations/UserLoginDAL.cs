using API.DAL.Interfaces;
using API.Models.Dto;
using API.Models.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace API.DAL.Implementations
{
    public class UserLoginDAL : BaseDAL, ILoginDAL
    {
        private readonly DataContext _dataContext;

        public UserLoginDAL(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public bool LoginEmailDoesentExists(string email)
        {
            bool emailExists = _dataContext.Users.Any(u => u.Email == email);

            if(!emailExists) 
            {
                return true;
            }
            else
                return false;
        }

        public User LoginUser(UserLoginDto user)
        {
            /*var userFromBase = _dataContext.Users.First(u => u.Email == user.Email);
            
            if (BCrypt.Net.BCrypt.Verify(user.Password, userFromBase.PasswordHash))
            {
                return userFromBase;
            }

            return null;*/
            using (var command = new SqliteCommand())
            {
                command.Connection = (SqliteConnection?)_dataContext.Database.GetDbConnection();
                command.CommandText = "SELECT * FROM Users WHERE Email = @Email";
                command.Parameters.AddWithValue("@Email", user.Email);

                _dataContext.Database.OpenConnection();

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
                            Authenticated = reader.IsDBNull(5) ? null : reader.GetBoolean(5),
                            Verified = reader.IsDBNull(6) ? null : reader.GetBoolean(6),
                            RoleId = reader.GetInt32(7),
                            LocationId = reader.IsDBNull(8) ? null : reader.GetInt32(8)
                        };

                        return dbUser;
                    }
                }
            }

            _dataContext.Database.CloseConnection();
            return null;

        }
    }
}
