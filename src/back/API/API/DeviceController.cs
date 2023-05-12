using API.BL.Interfaces;
using API.Models.Entity;
using API.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateDevice(int id, DeviceNameDsoControlAndDataShareUpdateViewModel request)
        {
            return Ok(await _deviceBL.UpdateDevice(id, request));
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
            return Ok(await _deviceBL.GetDeviceTypesByCategory(category));
        }

        [HttpGet]
        [Route("get-subtypes-by-type")]
        public async Task<IActionResult> GetDeviceSubtypesByType(int deviceTypeId)
        {
            return Ok(await _deviceBL.GetDeviceSubtypesByType( deviceTypeId));
        }

        [HttpGet("get-devices-by-user-id")]
        public IActionResult GetDevicesByUserId(int userId)
        {
            return Ok( _deviceBL.GetDevicesByUserId( userId));
        }
        
        [HttpGet("get-top-3-devices-by-user-id")]
        public async Task<IActionResult> Top3DevicesByUserId(int userId)
        {
            return Ok( await _deviceBL.Top3DevicesByUserId( userId));
        }

        [HttpPatch("device-control")]
        public async Task<IActionResult> TurnDevicesOnOff(DeviceControlViewModel request)
        {
            return Ok(await _deviceBL.TurnDevicesOnOff(request));
        }

        [HttpPatch("device-control-id")]
        public async Task<IActionResult> TurnDevicesOnOffById(DeviceControlViewModel request, int deviceId)
        {
            return Ok(await _deviceBL.TurnDevicesOnOffById(request, deviceId));
        }

        [HttpPatch("share-to-dso")]
        public async Task<IActionResult> ShareDeviceDataWithDSO(DeviceControlViewModel request)
        {
            return Ok(await _deviceBL.ShareDeviceDataWithDSO(request));
        }

        [HttpPatch("enable-dso-control-feature-by-deviceId")]
        public async Task<IActionResult> EnableDsoControlFeature(DsoControlViewModel request, int deviceId)
        {
            return Ok(await _deviceBL.EnableDsoControlFeature(request, deviceId));
        }

        [HttpPatch("enadble-dso-control-feature-for-users-devices")]
        public async Task<IActionResult> EnableDsoControlFeatureForAllDevices(DsoControlViewModel request, int userId)
        {
            return Ok(await _deviceBL.EnableDsoControlFeatureForAllDevices(request, userId));
        }
        
        [HttpPatch("share-to-dso-id")]
        public async Task<IActionResult> ShareDeviceDataWithDSOById(DeviceControlViewModel request, int deviceId)
        {
            return Ok(await _deviceBL.ShareDeviceDataWithDSOById(request, deviceId));
        }

        [HttpGet("get-devices-id-and-name-by-user-id")]
        public async Task<IActionResult> GetDevicesIdAndNameByUserId( int userId)
        {
            return Ok(await _deviceBL.GetDevicesIdAndNameByUserId( userId));
        }
    }
}
