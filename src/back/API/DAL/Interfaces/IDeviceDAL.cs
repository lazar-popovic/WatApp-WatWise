using API.Models.Entity;

namespace API.DAL.Interfaces
{
    public interface IDeviceDAL
    {
        Task<List<Device>> GetAllDevicesAsync();
        Task<Device> GetDeviceByIdAsync(int id);
        Task AddDeviceAsync(Device device);
        Task UpdateDeviceAsync(Device device);
        Task DeleteDeviceAsync(int id);




    }
}
