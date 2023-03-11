using API.BL.Interfaces;
using API.DAL.Interfaces;
using API.Models;
using API.Models.Dot;

namespace API.BL.Implementations;

public class ProsumerBL : IProsumerBL
{
    private readonly IProsumerDAL _prosumerDal;

    public ProsumerBL( IProsumerDAL prosumerDal)
    {
        _prosumerDal = prosumerDal;
    }

    public Response<object> RegisterProsumer(UserRegisterDot user)
    {
        var response = new Response<object>();

        user.Email = user.Email.Trim();
        user.Firstname = user.Firstname.Trim();
        user.Lastname = user.Lastname.Trim();

        if (user.Firstname == "")
        {
            response.Errors.Add("First name is required");
        }

        if (user.Lastname == "")
        {
            response.Errors.Add("Last name is required");
        }

        if (user.Email == "")
        {
            response.Errors.Add("Email is required");
        }
        else if (_prosumerDal.EmailExists( user.Email))
        {
            response.Errors.Add("User with this email already exists");
        }


        response.Success = !response.Errors.Any();

        if (!response.Success)
        {
            return response;
        }
        
        _prosumerDal.RegisterUser( user);
        response.Data = "Registration successful";

        return response;
    }
}