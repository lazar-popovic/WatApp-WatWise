﻿using API.BL.Interfaces;
using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models;
using API.Models.Entity;
using API.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using API.Common;
using API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Amazon.Runtime.Internal;
using API.Services.Geocoding.Interfaces;
using Cyrillic.Convert;

namespace API.BL.Implementations
{
    public class UserBL : IUserBL
    {
        private readonly IUserDAL _userDal;
        private readonly ILocationDAL _locationDal;
        private readonly IGeocodingService _geocodingService;

        public UserBL(IUserDAL userDal, IGeocodingService geocodingService, ILocationDAL locationDAL)
        {
            _userDal = userDal;
            _geocodingService = geocodingService;
            _locationDal = locationDAL;
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
        public async Task<Response> FindUsers(int id, string search, string mail, int pageSize, int pageNum, string order)
        {
            var response = new Response();

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
                response.Errors.Add("User with this id doesn't exist!");

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
                response.Errors.Add("User with this email doesn't exist!");
                response.Success = false;

                return response;
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                var user2 = await _userDal.GetByEmailAsync(request.Email);

                if (user2 != null && user2.Id != id)
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

        public async Task<Response> SaveImageForUser(int id, [FromBody] byte[] profilePicture)
        
        {
            var response = new Response();

            var user = await _userDal.SaveProfilePictureAsync(id, profilePicture);

            if (user == null)
            {
                response.Errors.Add("User doesn't exist!");
                response.Success = false;

                return response;
            }

            response.Data = new { Message = "Picture successfully changed!" };
            response.Success = response.Errors.Count() == 0;

            return response;
        }

        public async Task<Response<string>> DeleteProsumer(int id)
        {
            Response<string> response = new Response<string>();

            var user = await _userDal.GetByIdAsync(id);

            if (user == null)
            {
                response.Errors.Add("User with this id doesn't exist!");
                response.Success = response.Errors.Count == 0;

                return response;
            }

            /*if (user.RoleId != 3)
            {
                response.Errors.Add("Only prosumer can be deleted!");
                response.Success = response.Errors.Count == 0;

                return response;
            }*/

            await _userDal.DeleteUser(user);

            response.Data = "User has been successfully deleted!";
            response.Success = response.Errors.Count == 0;

            return response;
        }

        public async Task<Response<string>> DeleteProfilePictureAsync(int userId)
        {
            var response = new Response<string>();
            var user = await _userDal.DeleteProfilePictureAsync(userId);

            if (user == null)
            {
                response.Errors.Add("User is null");
                response.Success = false;
            }
           
            response.Data = "Pass";

            response.Success = response.Errors.Count() == 0;

            return response;



        }

        public async Task<Response<List<AllProsumersWithConsumptionProductionDTO>>> ProsumersWithEnergyUsage()
        {
            Response<List<AllProsumersWithConsumptionProductionDTO>> response =
                new Response<List<AllProsumersWithConsumptionProductionDTO>>();
            
            var users = await _userDal.ProsumersWithConsumptionProductionAndNumberOfWorkingDevices();

            if (users.IsNullOrEmpty())
            {
                response.Errors.Add("No prosumers are registered!");
                response.Success = false;

                return response;
            }

            response.Data = users;
            response.Success = response.Errors.Count == 0;

            return response;
        }

        public async Task<Response<List<User>>> GetAllEmployees()
        {
            Response<List<User>> response =
                new Response<List<User>>();
            
            var employees = await _userDal.GetAllEmployees();

            if (employees.IsNullOrEmpty())
            {
                response.Errors.Add("No employees are registered!");
                response.Success = false;

                return response;
            }

            response.Data = employees;
            response.Success = response.Errors.Count == 0;

            return response;
        }

        public async Task<Response> UpdateProsumer(UpdateUserViewModel updateUserViewModel)
        {
            var response = new Response();

            var user = await _userDal.GetByIdWithPasswordAsync(updateUserViewModel.Id);
            
            if (user == null)
            {
                response.Errors.Add("User with this id doesn't exist!");

                response.Success = false;
                return response;
            }

            if (string.IsNullOrEmpty(updateUserViewModel.Email))
            {
                response.Errors.Add("Email can not be empty!");
            }

            if (string.IsNullOrEmpty(updateUserViewModel.Firstname))
            {
                response.Errors.Add("Firstname can not be empty!");
            }

            if (string.IsNullOrEmpty(updateUserViewModel.Lastname))
            {
                response.Errors.Add("Lastname can not be empty!");
            }

            if (string.IsNullOrEmpty(updateUserViewModel.Location.Address))
            {
                response.Errors.Add("Address can not be empty!");
            }

            if (string.IsNullOrEmpty(updateUserViewModel.Location.City))
            {
                response.Errors.Add("City can not be empty!");
            }

            if ( updateUserViewModel.Location.Number == null)
            {
                response.Errors.Add("Number can not be empty!");
            }

            if (!string.IsNullOrEmpty(updateUserViewModel.Email))
            {
                var user2 = await _userDal.GetByEmailAsync(updateUserViewModel.Email);

                if (user2 != null && user2.Id != updateUserViewModel.Id)
                {
                    response.Errors.Add("Can not change email because is alredy used by other user!");
                    response.Success = false;

                    return response;
                }
            }
            
            user.Firstname = updateUserViewModel.Firstname;
            user.Lastname = updateUserViewModel.Lastname;
            user.Email = updateUserViewModel.Email;

            var conversion = new Conversion();
            updateUserViewModel.Location.Address = conversion.SerbianCyrillicToLatin(updateUserViewModel.Location.Address);
            updateUserViewModel.Location.Neighborhood = conversion.SerbianCyrillicToLatin(updateUserViewModel.Location.Neighborhood);
            updateUserViewModel.Location.City = conversion.SerbianCyrillicToLatin(updateUserViewModel.Location.City);
            var cords = _geocodingService.Geocode(updateUserViewModel.Location);
            var locationId = _locationDal.GetLocationByLatLongAsync(cords);
            if (locationId == 0)
            {
                locationId = _locationDal.InsertLocation(updateUserViewModel.Location, cords);
            }

            user.LocationId = locationId;

            _userDal.UpdateUser(user);

            response.Success = response.Errors.Count() == 0;
            response.Data = "Prosumer information successfully changed!";

            return response;
        }
    }
}
