using API.Models;

namespace API.BL.Interfaces;

public interface IDeviceDataBL
{
    Task<Response<object>> GetDeviceDataForToday(int deviceId);
    Task<Response<object>> GetDeviceDataForMonth(int deviceId);
    Task<Response<object>> GetDeviceDataForYear(int deviceId);
    Task<Response<object>> GetTodayTotalProductionConsumptionByUserId(int userId);
}