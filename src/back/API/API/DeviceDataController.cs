using API.BL.Interfaces;
using API.Models;
using API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.API;

[ApiController]
[Route("api/device-data")]
public class DeviceDataController : ControllerBase
{
    private readonly IDeviceDataBL _deviceDataBl;

    public DeviceDataController(IDeviceDataBL deviceDataBl)
    {
        _deviceDataBl = deviceDataBl;
    }

    [HttpGet]
    [Route("get-device-data-for-today")]
    public async Task<IActionResult> GetDeviceDataForToday(int deviceId)
    {
        return Ok( await _deviceDataBl.GetDeviceDataForToday( deviceId));
    }

    [HttpGet]
    [Route("get-allowed-share-devices-data-for-today")]
    public async Task<IActionResult> GetAllDevicesDataWhereShareWithDsoIsAllowedForToday()
    {
        return Ok(await _deviceDataBl.GetAllDevicesDataWhereShareWithDsoIsAllowedForToday());
    }
    
    [HttpGet]
    [Route("get-device-data-for-month")]
    public async Task<IActionResult> GetDeviceDataForMonth(int deviceId)
    {
        return Ok( await _deviceDataBl.GetDeviceDataForMonth( deviceId));
    }
        
    [HttpGet]
    [Route("get-device-data-for-year")]
    public async Task<IActionResult> GetDeviceDataForYear(int deviceId)
    {
        return Ok( await _deviceDataBl.GetDeviceDataForYear( deviceId));
    }
    
    [HttpGet]
    [Route("get-day-total-for-user")]
    public async Task<IActionResult> GetDayTotalProductionConsumptionByUserId( int day, int month, int year, int userId)
    {
        return Ok( await _deviceDataBl.GetDayTotalProductionConsumptionByUserId( day, month, year,userId));
    }
}