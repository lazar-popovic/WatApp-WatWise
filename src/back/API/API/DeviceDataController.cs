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
    [Route("get-device-data-for-date")]
    public async Task<IActionResult> GetDeviceDataForToday(int day, int month, int year, int deviceId)
    {
        return Ok( await _deviceDataBl.GetDeviceDataForToday(day, month, year, deviceId));
    }

    [HttpGet]
    [Route("get-allowed-share-devices-data-for-date")]
    public async Task<IActionResult> GetAllDevicesDataWhereShareWithDsoIsAllowedForToday(int day, int month, int year)
    {
        return Ok(await _deviceDataBl.GetAllDevicesDataWhereShareWithDsoIsAllowedForToday(day, month, year));
    }

    [HttpGet]
    [Route("get-devices-data-prediction-allowed-share-for-nextNdays")]
    public async Task<IActionResult> GetAllDevicesDataWhereShareWithDsoIsAllowedForNextNDays(int numberOfDays)
    {
        return Ok(await _deviceDataBl.GetAllDevicesDataWhereShareWithDsoIsAllowedForNextNDays(numberOfDays));
    }

    [HttpGet]
    [Route("get-device-data-for-next-n-days")]
    public async Task<IActionResult> GetDeviceDataForNextNDays(int id, int numberOfDays)
    {
        return Ok(await _deviceDataBl.GetDeviceDataForNextNDays(id, numberOfDays));
    }

    [HttpGet]
    [Route("get-device-data-history-by-category-andProsumers-id-for-month")]
    public async Task<IActionResult> GetDeviceDataForCategoryAndProsumerIdForMonth(int month,int year, int category, int userId)
    {
        return Ok(await _deviceDataBl.GetDeviceDataForCategoryAndProsumerIdForMonth(month, year, category, userId));
    }

    [HttpGet]
    [Route("get-device-data-for-month")]
    public async Task<IActionResult> GetDeviceDataForMonth(int month, int year, int deviceId)
    {
        return Ok( await _deviceDataBl.GetDeviceDataForMonth( month, year, deviceId));
    }

    [HttpGet]
    [Route("get-allowed-share-devices-data-for-month")]
    public async Task<IActionResult> GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth(int month, int year)
    {
        return Ok(await _deviceDataBl.GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth(month, year));
    }

    [HttpGet]
    [Route("get-device-data-for-year")]
    public async Task<IActionResult> GetDeviceDataForYear( int year, int deviceId)
    {
        return Ok( await _deviceDataBl.GetDeviceDataForYear( year, deviceId));
    }

    [HttpGet]
    [Route("get-allowed-share-devices-data-for-year")]
    public async Task<IActionResult> GetAllDevicesDataWhereShareWithDsoIsAllowedForYear( int year)
    {
        return Ok(await _deviceDataBl.GetAllDevicesDataWhereShareWithDsoIsAllowedForYear( year));
    }

    [HttpGet]
    [Route("get-day-total-for-user")]
    public async Task<IActionResult> GetDayTotalProductionConsumptionByUserId( int day, int month, int year, int userId)
    {
        return Ok( await _deviceDataBl.GetDayTotalProductionConsumptionByUserId( day, month, year,userId));
    }

    [HttpGet]
    [Route("get-energy-usage-total-7-hours-for-all-devices")]
    public async Task<IActionResult> GetTotalConsumptionForPrevious7HoursAndTotalProductionForNext7HoursForAllUsers()
    {
        return Ok( await _deviceDataBl.GetTotalConsumptionForPrevious7HoursAndTotalProductionForNext7HoursForAllUsers());
    }
}