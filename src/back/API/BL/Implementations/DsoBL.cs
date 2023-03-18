using API.BL.Interfaces;
using API.Common;
using API.DAL.Interfaces;
using API.Models;
using API.Models.Dto;
using API.Models.Entity;
using API.Services.E_mail.Interfaces;

namespace API.BL.Implementations;

public class DsoBL : IDsoBL
{
    private readonly IDsoDAL _dsoDal;
    private readonly IMailService _mailService;

    public DsoBL( IDsoDAL dsoDal, IMailService mailService)
    {
        _dsoDal = dsoDal;
        _mailService = mailService;
    }
    public Response<object> CheckForLoginCredentials(UserLoginDto user)
    {
        var response = new Response<object>();

        user.Email = user.Email.Trim();
        user.Password = user.Password.Trim();

        if (user.Email == "")
        {
            response.Errors.Add("Email is required");
        }
        if (_dsoDal.LoginEmailDoesentExists(user.Email))
        {
            response.Errors.Add("User with this email doesent exists");
        }

        response.Success = !response.Errors.Any();

        if (!response.Success)
            return response;

        var verifiedUser = _dsoDal.LoginUser(user);

        if ((bool)!verifiedUser.Verified)
        {
            response.Errors.Add("Your account is not verified");
            response.Success = !response.Errors.Any();
            return response;
        }

        if (verifiedUser.Role.RoleName == RoleEnum.Role.User.ToString())
        {
            response.Errors.Add("You are not authorized");
            response.Success = !response.Errors.Any();
            return response;
        }

        if (!BCrypt.Net.BCrypt.Verify(user.Password, verifiedUser.PasswordHash))
        {
            response.Errors.Add("Sorry, we couldn't verify your password. Please check and try again");
            response.Success = !response.Errors.Any();
            return response;
        }

        response.Data = verifiedUser;
        response.Success = true;
        return response;
    }

    public Response<object> RegisterEmployee(UserRegisterDto user)
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
        else if (_dsoDal.EmailExists( user.Email))
        {
            response.Errors.Add("User with this email already exists");
        }


        response.Success = !response.Errors.Any();

        if (!response.Success)
        {
            return response;
        }
        
        User newUser = _dsoDal.RegisterEmployee( user);
        newUser.Role = new Role { Id = 2, RoleName = "Employee" };
        _mailService.sendTokenEmployee( newUser);

        response.Data = new MessageDot { Message = "Registration successful" };

        return response;
    }
}