using API.BL.Interfaces;
using API.DAL.Interfaces;
using API.Models;

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

    public async Task<Response<object>> GetDeviceDataForMonth(int deviceId)
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetDeviceDataForMonth(deviceId);

        return response;
    }

    public async Task<Response<object>> GetDeviceDataForYear(int deviceId)
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetDeviceDataForYear(deviceId);

        return response;
    }

    public async Task<Response<object>> GetDayTotalProductionConsumptionByUserId(int day, int month, int year, int userId)
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetDayTotalProductionConsumptionByUserId(day,month,year,userId);

        return response;
    }
}