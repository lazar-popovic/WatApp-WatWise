namespace API.DAL.Interfaces;

public interface IDeviceDataDAL
{
    Task<object> GetDeviceDataForToday(int deviceId);
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForToday();
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForTomorrowPrediction();
    Task<object> GetDeviceDataByIdWhereShareWithDsoIsAllowedForTomorrowPrediction(int id);
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForNext3DaysPrediction();
    Task<object> GetDeviceDataByIdWhereShareWithDsoIsAllowedForNext3DaysPrediction(int id);
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForNext7DaysPrediction();
    Task<object> GetDeviceDataByIdWhereShareWithDsoIsAllowedForNext7DaysPrediction(int id);
    Task<object> GetDeviceDataForMonth(int deviceId);
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth();
    Task<object> GetDeviceDataForYear(int deviceId);
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForYear();
    Task<object> GetDayTotalProductionConsumptionByUserId( int day, int month, int year, int userId);
    Task<object> GetTotalConsumptionForPrevious7HoursAndTotalProductionForNext7HoursForAllUsers();
}