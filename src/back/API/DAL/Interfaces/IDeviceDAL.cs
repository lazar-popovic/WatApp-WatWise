using API.BL.Implementations;
using API.Common;
using API.Models;
using API.Models.Entity;
using API.Models.ViewModels;

namespace API.DAL.Interfaces
{
    public interface IDeviceDAL
    {
        Task<List<Device>> GetAllDevicesAsync();
        Task<Device?> GetDeviceByIdAsync(int id);
        Task<Device?> GetWholeDeviceByIdAsync(int id);
        Task AddDeviceAsync(Device device);
        Task UpdateDeviceAsync(Device device);
        Task DeleteDeviceAsync(Device device);
        Task<int> AddDeviceViewModel(DeviceViewModel device);
        Task<List<DeviceType>> GetDeviceTypesByCategory(int id);
        object GetDevicesByUserId( int userId);
        Task<Response> TurnDevicesOff(int userid);
        Task<Response> TurnDeviceOffById(int deviceId);
        Task<Response> TurnDeviceOnById(int deviceId);
        Task<Response<RegisterResponseViewModel>> ShareDataOffById(int deviceId);
        Task<Response<RegisterResponseViewModel>> ShareDataOnById(int deviceId);
        Task<Response> TurnDevicesOn(int userId);
        Task<Response> TurnDataSharingOff(int userId);
        Task<Response> TurnDataSharingOn(int userId);
        Task<object> Top3DevicesByUserId( int userId);
        Task<List<DeviceSubtype>> GetDeviceSubtypesByType( int deviceTypeId);
        Task<object> GetDevicesIdAndNameByUserId(int userId);
        Task TurnDsoControl(bool state, Device? dev);
        Task<Response> DsoControlForUserDevices(int userId, bool state);
    }
}
