namespace API.DAL.Interfaces;

public interface IDeviceDataDAL
{
    Task<object> GetDeviceDataForToday(int deviceId);
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForToday();
    Task<object> GetDeviceDataForMonth(int deviceId);
    Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth();
    Task<object> GetDeviceDataForYear(int deviceId);
    Task<object> GetDayTotalProductionConsumptionByUserId( int day, int month, int year, int userId);
}