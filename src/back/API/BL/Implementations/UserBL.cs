﻿using API.BL.Interfaces;
using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models;
using API.Models.Entity;

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

            var user =  await _userDal.GetByIdAsync(id);

            if(user == null)
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

        public async Task<Response<List<User>>> GetUsersBasedOnRoleAsync(int id)
        {
            var response = new Response<List<User>>();

            var users = await _userDal.GetUsersBasedOnRoleAsync(id);

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
    }
}