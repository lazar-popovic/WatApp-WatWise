using API.BL.Interfaces;
using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.API
{
    [Route("api/device")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceBL _deviceBL;

        public DeviceController(IDeviceBL deviceBL)
        {
            _deviceBL = deviceBL;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDevices()
        {
            return Ok(await _deviceBL.GetDevice());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceById(int id)
        {
            return Ok(await _deviceBL.GetByIdAsync(id));
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddDevice(Device device)
        {
            return Ok(await _deviceBL.AddDevice(device));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDevice(int id, Device device)
        {
            return Ok(await _deviceBL.UpdateDevice(id, device));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            return Ok(await _deviceBL.DeleteDevice(id));
        }


    }
}
