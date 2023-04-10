﻿using API.Models;
using API.Models.Entity;
using API.Models.ViewModels;

namespace API.BL.Interfaces
{
    public interface IUserBL
    {
        Task<Response<User>> GetByIdAsync(int id);
        Task<Response<List<User>>> GetUsersBasedOnRoleAsync(int id, int pageSize, int pageNumber);
        Task<Response<List<User>>> GetUsers();
        Task<Response<List<User>>> GetUsersWithLocationId(int id);
        Task<Response<int>> getNumberOfUsers(int id);
        Task<Response<List<User>>> FindUsers(int id, string search, string mail, int pageSize, int pageNum, string order);
        Task<Response<string>> UpdateUserPassword(UpdateUserPasswordViewModel request, int id);
        Task<Response<string>> UpdateUserNameAndEmail(UpdateUserNameAndEmailViewModel request, int id);
        Task<Response<User>> SaveImageForUser(int id, byte[] profilePicture);
    }
}
