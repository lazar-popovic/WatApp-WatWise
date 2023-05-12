using API.Models.Entity;
using API.Models;
using API.Models.ViewModels;
using API.Common;

namespace API.BL.Interfaces
{
    public interface IDeviceBL
    {
        Task<Response<List<Device>>> GetDevice();
        Task<Response<String>> AddDevice(Device device);
        Task<Response<Device>> GetByIdAsync(int id);
        Task<Response<String>> UpdateDevice(int id, DeviceNameDsoControlAndDataShareUpdateViewModel request);
        Task<Response<String>> DeleteDevice(int id);
        Task<Response<object>> AddDeviceViewModel(DeviceViewModel devicee);
        Task<Response<List<DeviceType>>> GetDeviceTypesByCategory(int id);
        Response<object> GetDevicesByUserId(int userId);
        Task<Response> TurnDevicesOnOff(DeviceControlViewModel request);
        Task<Response<RegisterResponseViewModel>> TurnDevicesOnOffById(DeviceControlViewModel request, int deviceId);
        Task<Response<RegisterResponseViewModel>> ShareDeviceDataWithDSOById(DeviceControlViewModel request, int deviceId);
        Task<Response<RegisterResponseViewModel>> ShareDeviceDataWithDSO(DeviceControlViewModel request);
        Task<Response<object>> Top3DevicesByUserId(int userId);
        Task<Response> GetDeviceSubtypesByType(int deviceTypeId);
        Task<Response> GetDevicesIdAndNameByUserId(int userId);
        Task<Response> EnableDsoControlFeature(DsoControlViewModel request, int deviceId);
    }
}
