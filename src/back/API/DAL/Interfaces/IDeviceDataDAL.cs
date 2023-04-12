namespace API.DAL.Interfaces;

public interface IDeviceDataDAL
{
    Task<object> GetDeviceDataForToday(int day, int month, int year, int deviceId);
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForToday( int day, int month, int year);
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForTomorrowPrediction();
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForNext3DaysPrediction();
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForNext7DaysPrediction();
    Task<object> GetDeviceDataForMonth(int month, int year, int deviceId);
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth(int month, int year);
    Task<object> GetDeviceDataForCategoryAndProsumerIdForMonth(int month, int year, int category, int userId);
    Task<object> GetDeviceDataForYear( int year, int deviceId);
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForYear(int year);
    Task<object> GetDeviceDataForCategoryAndProsumerIdForYear(int year, int category, int userId);
    Task<object> GetDeviceDataForTomorrowPrediction(int id);
    Task<object> GetDeviceDataForNext3DaysPrediction(int id);
    Task<object> GetDeviceDataForNext7DaysPrediction(int id);
    Task<object> GetDayTotalProductionConsumptionByUserId( int day, int month, int year, int userId);
    Task<object> GetTotalConsumptionForPrevious7HoursAndTotalProductionForNext7HoursForAllUsers();
}