using API.BL.Interfaces;
using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models.Entity;
using API.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        
        [HttpPost("insert-device"), Authorize(Roles = "User")]
        public async Task<IActionResult> AddDeviceViewModel(DeviceViewModel device)
        {
            return Ok(await _deviceBL.AddDeviceViewModel(device));
        }

        [HttpGet]
        [Route("get-types-by-category")]
        public async Task<IActionResult> GetDeviceTypesByCategory(int category)
        {
            return Ok(await _deviceBL.GetDeviceTypesByCategory( category));
        }
    }
}
