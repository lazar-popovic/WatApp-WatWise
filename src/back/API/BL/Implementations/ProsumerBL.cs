using API.BL.Interfaces;
using API.DAL.Interfaces;
using API.Models;
using API.Models.Dot;
using API.Models.Dto;
using API.Models.Entity;
using API.Services.E_mail.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace API.BL.Implementations;

public class ProsumerBL : IProsumerBL
{
    private readonly IProsumerDAL _prosumerDal;
    private readonly IMailService _mailService;

    public ProsumerBL( IProsumerDAL prosumerDal, IMailService mailService)
    {
        _prosumerDal = prosumerDal;
        _mailService = mailService;
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
        
        User newUser = _prosumerDal.RegisterUser( user);
        newUser.Role = new Role { Id = 3, RoleName = "User" };
        _mailService.sendTokenProsumer( newUser);
        
        response.Data = "Registration successful";

        return response;
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
        if (_prosumerDal.LoginEmailDoesentExists(user.Email))
        {
            response.Errors.Add("User with this email doesent exists");
        }

        response.Success = !response.Errors.Any();

        if (!response.Success)
            return response;

        var verifiedUser = _prosumerDal.LoginUser(user);

        if ((bool)!verifiedUser.Verified)
        {
            response.Errors.Add("Your account is not verified");
            response.Success = !response.Errors.Any();
            return response;
        }

        if (verifiedUser.RoleId != 3)
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

    public Response<object> CheckEmailForForgottenPassword(ForgottenPasswordRequest request)
    {
        var response = new Response<Object>();

        request.Email = request.Email.Trim();

        if (request.Email == "")
        {
            response.Errors.Add("Email is required");
        }
        if (_prosumerDal.LoginEmailDoesentExists(request.Email))
        {
            response.Errors.Add("User with this email doesent exists");
        }

        var user = _prosumerDal.UserForGivenEmail(request.Email);

        response.Success = !response.Errors.Any();

        if (!response.Success)
            return response;

        if ((bool)!user.Verified)
        {
            response.Errors.Add("Your account is not verified");
            response.Success = !response.Errors.Any();
            return response;
        }

        if (user.RoleId != 3)
        {
            response.Errors.Add("You are not authorized");
            response.Success = !response.Errors.Any();
            return response;
        }

        response.Data = user;
        response.Success = true;

        return response;
    }

    public ResetPasswordToken GenerateNewResetPasswordToken(int userID)
    {
        var token = new ResetPasswordToken
        {
            Token = Guid.NewGuid().ToString(),
            ExpiryTime = DateTime.UtcNow.AddMinutes(15),
            UserId = userID
        };

        _prosumerDal.AddResetToken(token);

        return token;
    }

    public ResetPasswordToken GetResetTokenEntity(string t)
    {
        return _prosumerDal.GetResetTokenEntityFromBase(t);
    }

    public User FindUserByIdFromTokenEntity(int id)
    {
        return _prosumerDal.FindUserByIdFromTokenEntityFromBase(id);
    }

    public void SetNewPasswordAfterResetting(User user, string password)
    {
        user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        _prosumerDal.UpdateUserAfterPasswordReset(user);
    }

    public void RemovePasswordResetToken(ResetPasswordToken resetToken)
    {
        _prosumerDal.RemovePasswordResetTokenFromBase(resetToken);
    }
}
