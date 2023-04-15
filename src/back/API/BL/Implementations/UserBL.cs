using API.BL.Interfaces;
using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models;
using API.Models.Entity;
using API.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace API.BL.Implementations
{
    public class UserBL : IUserBL
    {
        private readonly IUserDAL _userDal;

        public UserBL(IUserDAL userDal)
        {
            _userDal = userDal;
        }

        public async Task<Response<User>> GetByIdAsync(int id)
        {
            var response = new Response<User>();

            var user = await _userDal.GetByIdAsync(id);

            if (user == null)
            {
                response.Errors.Add("User doesen't exist!");
                response.Success = false;

                return response;
            }

            response.Data = user!;
            response.Success = response.Errors.Count() == 0;

            return response;
        }

        public async Task<Response<List<User>>> GetUsers()
        {
            var response = new Response<List<User>>();

            var users = await _userDal.GetUsers();

            if (users == null)
            {
                response.Errors.Add("Error with displaying users from base!");
                response.Success = false;

                return response;
            }

            response.Data = users!;
            response.Success = response.Errors.Count() == 0;

            return response;
        }

        public async Task<Response<List<User>>> GetUsersBasedOnRoleAsync(int id, int pageSize, int pageNumber)
        {
            var response = new Response<List<User>>();

            var users = await _userDal.GetUsersBasedOnRoleAsync(id, pageSize, pageNumber);

            if (users == null)
            {
                response.Errors.Add("Error with displaying users from base!");
                response.Success = false;

                return response;
            }

            response.Data = users!;
            response.Success = response.Errors.Count() == 0;

            return response;
        }
        public async Task<Response<List<User>>> GetUsersWithLocationId(int id)
        {
            var response = new Response<List<User>>();

            var users = await _userDal.GetUsersWithLocationId(id);

            if (users == null)
            {
                response.Errors.Add("Error with displaying users from base!");
                response.Success = false;

                return response;
            }

            response.Data = users!;
            response.Success = response.Errors.Count() == 0;

            return response;
        }
        public async Task<Response<int>> getNumberOfUsers(int id)
        {
            var response = new Response<int>();
            int number = await _userDal.getNumberOfProsumersOrEmployees(id);
            if (number == 0)
            {
                response.Errors.Add("There are no loaded users");
                response.Success = false;

                return response;
            }

            response.Data = number!;
            response.Success = response.Errors.Count() == 0;

            return response;

        }
        public async Task<Response<List<User>>> FindUsers(int id, string search, string mail, int pageSize, int pageNum, string order)
        {
            var response = new Response<List<User>>();

            var users = await _userDal.FindUser(id, search, mail, pageSize, pageNum, order);

            if (users == null)
            {
                response.Errors.Add("Error with displaying users from base!");
                response.Success = false;

                return response;
            }

            response.Data = users!;
            response.Success = response.Errors.Count() == 0;

            return response;
        }

        public async Task<Response<string>> UpdateUserPassword(UpdateUserPasswordViewModel request, int id)
        {
            var response = new Response<string>();

            if (string.IsNullOrEmpty(request.OldPassword))
            {
                response.Errors.Add("Old password field cannot be empty!");
            }

            if (string.IsNullOrEmpty(request.NewPassword))
            {
                response.Errors.Add("New password field cannot be empty!");
            }

            if (string.IsNullOrEmpty(request.ConfirmedPassword))
            {
                response.Errors.Add("Confirmed password field cannot be empty!");
            }

            if (request.NewPassword != request.ConfirmedPassword)
            {
                response.Errors.Add("Passwords must match!");
            }

            response.Success = response.Errors.Count() == 0;
            if (!response.Success)
                return response;

            var user = await _userDal.GetByIdWithPasswordAsync(id);

            if (user == null)
            {
                response.Errors.Add("User with this id doesen't exist!");

                response.Success = false;
                return response;
            }

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
            {
                response.Errors.Add("Your old password is incorrect!");

                response.Success = false;
                return response;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _userDal.UpdateUser(user);

            response.Data = "Password has been updated succesfully";

            response.Success = response.Errors.Count == 0;

            return response;
        }

        public async Task<Response<string>> UpdateUserNameAndEmail(UpdateUserNameAndEmailViewModel request, int id)
        {
            var response = new Response<string>();

            var user = await _userDal.GetByIdWithPasswordAsync(id);

            if (user == null)
            {
                response.Errors.Add("User with this email doesent exist!");
                response.Success = false;

                return response;
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                var user2 = await _userDal.GetByEmailAsync(request.Email);

                if (user2 != null)
                {
                    response.Errors.Add("Cannot update your email with email of existing user!");
                    response.Success = false;

                    return response;
                }
                else
                    user.Email = request.Email;
            }

            if (!string.IsNullOrEmpty(request.FirstName))
            {
                user.Firstname = request.FirstName;
            }

            if (!string.IsNullOrEmpty(request.LastName))
            {
                user.Lastname = request.LastName;
            }

            _userDal.UpdateUser(user);

            response.Data = "User info has been updated successfully!";
            response.Success = response.Errors.Count == 0;

            return response;
        }

        public async Task<Response<User>> SaveImageForUser(int id, [FromBody] byte[] profilePicture)
        
        {
            var response = new Response<User>();

            var user = await _userDal.SaveProfilePictureAsync(id, profilePicture);

            if (user == null)
            {
                response.Errors.Add("User doesen't exist!");
                response.Success = false;

                return response;
            }

            response.Data = user!;
            response.Success = response.Errors.Count() == 0;

            return response;
        }

    }
}
