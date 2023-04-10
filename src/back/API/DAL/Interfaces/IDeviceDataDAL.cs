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
    Task<object> GetDeviceDataForYear( int year, int deviceId);
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForYear(int year);
    Task<object> GetDayTotalProductionConsumptionByUserId( int day, int month, int year, int userId);
    Task<object> GetTotalConsumptionForPrevious7HoursAndTotalProductionForNext7HoursForAllUsers();
}