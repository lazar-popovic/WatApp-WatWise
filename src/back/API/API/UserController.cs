using API.BL.Interfaces;
using API.Common;
using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models.Entity;
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




    }
}
