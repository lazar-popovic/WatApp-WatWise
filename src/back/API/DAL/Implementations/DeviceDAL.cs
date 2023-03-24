using API.DAL.Interfaces;
using API.Models.Entity;
using API.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API.DAL.Implementations
{
    public class DeviceDAL:IDeviceDAL
    {
        private readonly DataContext _dbContext;

        public DeviceDAL(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Device>> GetAllDevicesAsync()
        {
            return await _dbContext.Devices.ToListAsync();
        }

        public async Task<Device> GetDeviceByIdAsync(int id)
        {
            return await _dbContext.Devices.FindAsync(id);
        }

        public async Task AddDeviceAsync(Device device)
        {
            await _dbContext.Devices.AddAsync(device);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDeviceAsync(Device device)
        {
            _dbContext.Entry(device).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDeviceAsync(int id)
        {
            Device device = await GetDeviceByIdAsync(id);
            _dbContext.Devices.Remove(device);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddDeviceViewModel(DeviceViewModel devicee)
        {

            var device = new Device
            {
                UserId = devicee.UserId,
                ActivityStatus = false,
                PurchaseDate = DateTime.Now,
                Type = devicee.Type,
                Category = devicee.Category,
                Name = devicee.Name
            };
            await _dbContext.Devices.AddAsync(device);
            await _dbContext.SaveChangesAsync();
        }
    }
}
