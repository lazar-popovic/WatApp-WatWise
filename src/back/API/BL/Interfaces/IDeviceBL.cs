using API.Models.Entity;
using API.Models;
using API.Models.ViewModels;

namespace API.BL.Interfaces
{
    public interface IDeviceBL
    {
        Task<Response<List<Device>>> GetDevice();
        Task<Response<String>> AddDevice(Device device);
        Task<Response<Device>> GetByIdAsync(int id);
        Task<Response<String>> UpdateDevice(int id, Device device);
        Task<Response<String>> DeleteDevice(int id);
        Task<Response<String>> AddDeviceViewModel(DeviceViewModel devicee);
        Task<Response<List<DeviceType>>> GetDeviceTypesByCategory(int id);
        Response<object> GetDevicesByUserId(int userId);
        Task<Response<RegisterResponseViewModel>> TurnDevicesOnOff(DeviceControlViewModel request);
        Task<Response<RegisterResponseViewModel>> TurnDevicesOnOffById(DeviceControlViewModel request, int deviceId);
        Task<Response<RegisterResponseViewModel>> ShareDeviceDataWithDSOById(DeviceControlViewModel request, int deviceId);
        Task<Response<RegisterResponseViewModel>> ShareDeviceDataWithDSO(DeviceControlViewModel request);
        Task<Response<object>> Top3DevicesByUserId(int userId);
    }
}
