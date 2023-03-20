using API.Models.Dto;

namespace API.Obsolete;

public interface IBaseDAL
{
    void Insert();
    void Update();
    void Delete();
    void Get();

    //bool LoginEmailDoesentExists(string email);
    //void LoginUser(UserLoginDto user);
}