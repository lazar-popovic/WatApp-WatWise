using System.ComponentModel.DataAnnotations;
using API.BL.Interfaces;
using API.Common;
using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models.Entity;
using API.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.API
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL _userBL;

        public UserController(IUserBL userBL)
        {
            _userBL = userBL;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok(await _userBL.GetByIdAsync(id));
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userBL.GetUsers());
         
        }

        //Role = 2 => Employees
        [HttpGet("employees")]
        public async Task<IActionResult> GetAllEmployees(int pageSize, int pageNumber)
        {
            return Ok(await _userBL.GetUsersBasedOnRoleAsync((int)RoleEnum.Role.Employee, pageSize, pageNumber));
        }

        //Role = 3 => Prosumers
        [HttpGet("prosumers")]
        public async  Task<IActionResult> GetAllProsumers(int pageSize, int pageNumber)
        {
            return Ok(await _userBL.GetUsersBasedOnRoleAsync((int)RoleEnum.Role.User, pageSize, pageNumber));
        }

        [HttpGet("users-with-locationId")]
        public async Task<IActionResult> GetAllUsersWithLocationId(int id)
        {
            return Ok(await _userBL.GetUsersWithLocationId(id));
        }

        [HttpGet("employees-number")]
        public async Task<IActionResult> GetNumberEmployees()
        {
            return Ok(await _userBL.getNumberOfUsers((int)RoleEnum.Role.Employee));
        }

        [HttpGet("prosumers-number")]
        public async Task<IActionResult> GetNumberProsumers()
        {
            return Ok(await _userBL.getNumberOfUsers((int)RoleEnum.Role.User));
        }

        [HttpGet("get-prosumers-filtered")]
        public async Task<IActionResult> GetAllProsumersFiltered( string? fullName, string? streetAddress, int pageSize, int pageNumber, string? sortOrder)
        {
            return Ok(await _userBL.FindUsers((int)RoleEnum.Role.User, fullName, streetAddress, pageSize, pageNumber, sortOrder));
        }

        [HttpGet("get-employees-filtered")]
        public async Task<IActionResult> GetAllEmployeesFiltered( string? fullName, int pageSize, int pageNumber, string? sortOrder)
        {
            return Ok(await _userBL.FindUsers((int)RoleEnum.Role.Employee, fullName, null, pageSize, pageNumber, sortOrder));
        }

        [HttpPatch("update-user-password")]
        public async Task<IActionResult> UpdateUserPassword(UpdateUserPasswordViewModel request, int id)
        {
            return Ok(await _userBL.UpdateUserPassword(request, id));
        }

        [HttpPatch("update-user-name-email")]
        public async Task<IActionResult> UpdateUserNameAndEmail(UpdateUserNameAndEmailViewModel request, int id)
        {
            return Ok(await _userBL.UpdateUserNameAndEmail(request, id));
        }
        
        [HttpPost("update-user-image")]
        public async Task<IActionResult> SaveImageForUser(int id, [FromBody] byte[] imagePicture)
        {
            return Ok(await _userBL.SaveImageForUser(id, imagePicture));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProsumer(int id)
        {
            return Ok(await _userBL.DeleteProsumer(id));
        }
    }
}
