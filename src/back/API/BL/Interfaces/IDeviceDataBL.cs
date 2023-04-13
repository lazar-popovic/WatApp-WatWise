using API.Common;
using API.Models;

namespace API.BL.Interfaces;

public interface IDeviceDataBL
{
    Task<Response<object>> GetDeviceDataForToday(int day, int month, int year, int deviceId);
    Task<Response> GetDeviceDataForCategoryAndProsumerIdForToday(int day, int month, int year, int category, int userId);
    Task<Response<object>> GetAllDevicesDataWhereShareWithDsoIsAllowedForToday(int day, int month, int year);
    Task<Response<object>> GetDeviceDataForMonth(int month, int year, int deviceId);
    Task<Response<object>> GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth(int month, int year);
    Task<Response> GetDeviceDataForCategoryAndProsumerIdForMonth(int month, int year, int category, int userId);
    Task<Response<object>> GetDeviceDataForYear( int year, int deviceId);
    Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForYear(int year);
    Task<Response> GetDeviceDataForCategoryAndProsumerIdForYear(int year, int category, int userId);
    Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForNextNDays(int numberOfDays);
    Task<Response> GetDeviceDataForNextNDays(int id, int numberOfDays);
    Task<Response> GetProsumerDevicesDataForNextNDaysForCategory(int numberOfDays, int category, int userId);
    Task<Response> GetTotalConsumptionForPrevious7HoursAndTotalProductionForNext7HoursForAllUsers();
    Task<Response<object>> GetDayTotalProductionConsumptionByUserId( int day, int month, int year, int userId);
  
}