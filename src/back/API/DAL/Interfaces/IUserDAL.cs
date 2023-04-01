﻿using API.Models.Entity;

namespace API.DAL.Interfaces
{
    public interface IUserDAL
    {
        Task<User?> GetByIdAsync(int id);
        Task<List<User>?> GetUsersBasedOnRoleAsync(int id, int pageSize, int pageNumber);
        Task<List<User>?> GetUsers();
        Task<List<User>?> GetUsersWithLocationId(int id);
        Task<int> getNumberOfProsumersOrEmployees(int id);
        Task<List<User>?> FindUser(int id, string search, string mail, int pageSize, int pageNum, string order);
        }
}
