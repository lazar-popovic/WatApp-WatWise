using API.BL.Interfaces;
using API.Common;
using API.DAL.Interfaces;
using API.Models;
using API.Models.Entity;

namespace API.BL.Implementations;

public class DeviceDataBL : IDeviceDataBL
{
    private readonly IDeviceDataDAL _deviceDataDal;

    public DeviceDataBL(IDeviceDataDAL deviceDataDal)
    {
        _deviceDataDal = deviceDataDal;
    }
    
    public async Task<Response<object>> GetDeviceDataForToday(int deviceId)
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetDeviceDataForToday(deviceId);

        return response;
    }

    public async Task<Response<object>> GetAllDevicesDataWhereShareWithDsoIsAllowedForToday()
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetAllDevicesDataWhereShareWithDsoIsAllowedForToday();

        return response;
    }

    public async Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForNextNDays(int numberOfDays)
    {
        throw new NotImplementedException();
    }

    /*
    public async Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForTomorrowPrediction()
    {
        var response = new Response();
        response.Success = true;
        response.Data = await _deviceDataDal.GetAllDevicesDataWhereShareWithDsoIsAllowedForTomorrowPrediction();

        return response;
    }


    public async Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForNext3DaysPrediction()
    {
        var response = new Response();
        response.Success = true;
        response.Data = await _deviceDataDal.GetAllDevicesDataWhereShareWithDsoIsAllowedForNext3DaysPrediction();

        return response;
    }

    public async Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForNext7DaysPrediction()
    {
        var response = new Response();
        response.Success = true;
        response.Data = await _deviceDataDal.GetAllDevicesDataWhereShareWithDsoIsAllowedForNext7DaysPrediction();

        return response;
    }
    */
    public async Task<Response<object>> GetDeviceDataForMonth(int deviceId)
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetDeviceDataForMonth(deviceId);

        return response;
    }

    public async Task<Response<object>> GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth()
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth();

        return response;
    }

    public async Task<Response<object>> GetDeviceDataForYear(int deviceId)
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetDeviceDataForYear(deviceId);

        return response;
    }

    public async Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForYear()
    {
        var response = new Response();

        response.Success = true;
        response.Data = await _deviceDataDal.GetAllDevicesDataWhereShareWithDsoIsAllowedForYear();

        return response;
    }

    public async Task<Response<object>> GetDayTotalProductionConsumptionByUserId( int day, int month, int year, int userId)
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetDayTotalProductionConsumptionByUserId( day, month, year, userId);

        return response;
    }

    public async Task<Response> GetTotalConsumptionForPrevious7HoursAndTotalProductionForNext7HoursForAllUsers()
    {
        var response = new Response();

        response.Success = true;
        response.Data = await _deviceDataDal.GetTotalConsumptionForPrevious7HoursAndTotalProductionForNext7HoursForAllUsers();

        return response;
    }

    
}