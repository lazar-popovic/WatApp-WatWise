namespace API.DAL.Interfaces;

public interface IDeviceDataDAL
{
    Task<object> GetDeviceDataForToday(int deviceId);
    Task<object> GetDeviceDataForMonth(int deviceId);
    Task<object> GetDeviceDataForYear(int deviceId);
    Task<object> GetTodayTotalProductionConsumptionByUserId(int userId);
}