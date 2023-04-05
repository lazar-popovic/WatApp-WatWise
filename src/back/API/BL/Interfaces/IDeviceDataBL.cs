using API.Common;
using API.Models;

namespace API.BL.Interfaces;

public interface IDeviceDataBL
{
    Task<Response<object>> GetDeviceDataForToday(int deviceId);
    Task<Response<object>> GetAllDevicesDataWhereShareWithDsoIsAllowedForToday();
    Task<Response<object>> GetDeviceDataForMonth(int deviceId);
    Task<Response<object>> GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth();
    Task<Response<object>> GetDeviceDataForYear(int deviceId);
    Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForYear();
    Task<Response<object>> GetDayTotalProductionConsumptionByUserId( int day, int month, int year, int userId);
  
}