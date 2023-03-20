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
        public async Task<IActionResult> GetAllEmployees()
        {
            return Ok(await _userBL.GetUsersBasedOnRoleAsync((int)RoleEnum.Role.Employee));
        }

        //Role = 3 => Prosumers
        [HttpGet("prosumers")]
        public async  Task<IActionResult> GetAllProsumers()
        {
            return Ok(await _userBL.GetUsersBasedOnRoleAsync((int)RoleEnum.Role.User));
        }



    }
}
