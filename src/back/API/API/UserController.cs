using API.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.API
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserDAL _iuserDAL;

        public UserController(IUserDAL userDAL)
        {
            _iuserDAL = userDAL;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _iuserDAL.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var user = await _iuserDAL.GetUsers();
            return Ok(user);
        }
        //Role = 2 => Employees
        [HttpGet("employees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var user = await _iuserDAL.GetByIdAsync(2);
            return Ok(user);
        }
        //Role = 3 => Prosumers
        [HttpGet("prosumers")]
        public async Task<IActionResult> GetAllProsumers()
        {
            var prosumer = await _iuserDAL.GetByIdAsync(3);
            return Ok(prosumer);
        }



    }
}
