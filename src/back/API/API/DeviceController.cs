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
        private readonly IDeviceDAL _ideviceDAL;

        public DeviceController(IDeviceDAL deviceDAL)
        {
            _ideviceDAL = deviceDAL;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllDevices()
        {
            var devices = await _ideviceDAL.GetAllDevicesAsync();
            return Ok(devices);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceById(int id)
        {
            var device = await _ideviceDAL.GetDeviceByIdAsync(id);

            if (device == null)
            {
                return NotFound();
            }
            return Ok(device);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddDevice(Device device)
        {
            await _ideviceDAL.AddDeviceAsync(device);
            return CreatedAtAction(nameof(GetDeviceById), new { id = device.Id }, device);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDevice(int id, Device device)
        {
            if (id != device.Id)
            {
                return BadRequest();
            }
            await _ideviceDAL.UpdateDeviceAsync(device);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            await _ideviceDAL.DeleteDeviceAsync(id);
            return NoContent();
        }


    }
}
