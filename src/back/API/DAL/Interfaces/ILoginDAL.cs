using API.Models.Dto;
using API.Models.Entity;

namespace API.DAL.Interfaces
{
    public interface ILoginDAL : IBaseDAL
    {
        bool LoginEmailDoesentExists(string email);
        User LoginUser(UserLoginDto user);
        

    }
}
