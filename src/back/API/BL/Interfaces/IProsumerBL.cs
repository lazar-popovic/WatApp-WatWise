using API.Models;
using API.Models.Dot;

namespace API.BL.Interfaces;

public interface IProsumerBL
{
    Response<object> RegisterProsumer(UserRegisterDot user);
}