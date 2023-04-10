using API.Common;
using API.Models;

namespace API.BL.Interfaces;

public interface IDeviceDataBL
{
    Task<Response<object>> GetDeviceDataForToday(int deviceId);
    Task<Response<object>> GetAllDevicesDataWhereShareWithDsoIsAllowedForToday(int day, int month, int year);
    Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForTomorrowPrediction();
    Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForNext3DaysPrediction();
    Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForNext7DaysPrediction();
    Task<Response<object>> GetDeviceDataForMonth(int deviceId);
    Task<Response<object>> GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth(int month, int year);
    Task<Response<object>> GetDeviceDataForYear(int deviceId);
    Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForYear(int year);
    Task<Response> GetTotalConsumptionForPrevious7HoursAndTotalProductionForNext7HoursForAllUsers();
    Task<Response<object>> GetDayTotalProductionConsumptionByUserId( int day, int month, int year, int userId);
  
}