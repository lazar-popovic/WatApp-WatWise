using API.Common;
using API.Models;

namespace API.BL.Interfaces;

public interface IDeviceDataBL
{
    Task<Response<object>> GetDeviceDataForToday(int day, int month, int year, int deviceId);
    Task<Response<object>> GetAllDevicesDataWhereShareWithDsoIsAllowedForToday(int day, int month, int year);
    Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForTomorrowPrediction();
    Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForNext3DaysPrediction();
    Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForNext7DaysPrediction();
    Task<Response<object>> GetDeviceDataForMonth(int month, int year, int deviceId);
    Task<Response<object>> GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth(int month, int year);
    Task<Response<object>> GetDeviceDataForYear( int year, int deviceId);
    Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForYear(int year);
    Task<Response<object>> GetDeviceDataForToday(int deviceId);
    Task<Response<object>> GetAllDevicesDataWhereShareWithDsoIsAllowedForToday();
    Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForNextNDays(int numberOfDays);
    Task<Response> GetSpecificDeviceDataWhereShareWithDsoIsAllowedForNextNDays(int id, int numberOfDays);
    Task<Response<object>> GetDeviceDataForMonth(int deviceId);
    Task<Response<object>> GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth();
    Task<Response<object>> GetDeviceDataForYear(int deviceId);
    Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForYear();
    Task<Response> GetTotalConsumptionForPrevious7HoursAndTotalProductionForNext7HoursForAllUsers();
    Task<Response<object>> GetDayTotalProductionConsumptionByUserId( int day, int month, int year, int userId);
  
}