using API.DAL.Interfaces;
using API.Models.Dto;
using API.Models.Entity;

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
            bool emailExists =  _dataContext.Users.Any(u => u.Email == email);

            if(!emailExists) 
            {
                return true;
            }
            else
                return false;
        }

        public User LoginUser(UserLoginDto user)
        {
            User userFromBase = _dataContext.Users.FirstOrDefault(u => u.Email == user.Email);
            /*
            if (userFromBase != null)
            {
            }*/

            if (BCrypt.Net.BCrypt.Verify(user.Password, userFromBase.PasswordHash))
            {
                return userFromBase;
            }

            return null;
            
        }
    }
}
