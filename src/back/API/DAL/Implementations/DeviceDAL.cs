using API.DAL.Interfaces;
using API.Models;
using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.DeviceSimulatorService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.DAL.Implementations
{
    public class DeviceDAL : IDeviceDAL
    {
        private readonly DataContext _dbContext;
        private readonly IDeviceSimulatorService _deviceSimulatorService;

        public DeviceDAL(DataContext dbContext, IDeviceSimulatorService deviceSimulatorService)
        {
            _dbContext = dbContext;
            _deviceSimulatorService = deviceSimulatorService;
        }

        public async Task<List<Device>> GetAllDevicesAsync()
        {
            return await _dbContext.Devices.ToListAsync();
        }

        public async Task<Device?> GetDeviceByIdAsync(int id)
        {
            return await _dbContext.Devices.Where(d=>d.Id == id).Select(d => new Device
            {
                Id = d.Id,
                Name = d.Name,
                DataShare = d.DataShare,
                DeviceTypeId = d.DeviceTypeId,
                PurchaseDate = d.PurchaseDate,
                ActivityStatus = d.ActivityStatus,
                DeviceType = d.DeviceType,
                UserId = d.UserId
            }).AsNoTracking().FirstOrDefaultAsync();
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

        public async Task DeleteDeviceAsync(Device dev)
        {
            var recordsToDelete = _dbContext.DeviceEnergyUsage.Where(x => x.DeviceId == dev.Id);
            _dbContext.DeviceEnergyUsage.RemoveRange(recordsToDelete);

            _dbContext.Devices.Remove(dev!);

            await _dbContext.SaveChangesAsync();
        }
        public async Task AddDeviceViewModel(DeviceViewModel devicee)
        {

            var device = new Device
            {
                UserId = devicee.UserId,
                ActivityStatus = true,
                PurchaseDate = DateTime.Now,
                DeviceTypeId = devicee.DeviceTypeId,
                Name = devicee.Name,
                DataShare = true
            };
            await _dbContext.Devices.AddAsync(device);
            await _dbContext.SaveChangesAsync();

            await _deviceSimulatorService.FillDataSinceJanuary1st(((int)devicee.DeviceTypeId!), device.Id);
        }

        public async Task<List<DeviceType>> GetDeviceTypesByCategory(int id)
        {
            return await _dbContext.DeviceTypes.Where(dt => dt.Category == id)
                                               .Select(dt => new DeviceType { Id = dt.Id, Type = dt.Type })
                                               .AsNoTracking().ToListAsync();
        }

        public object GetDevicesByUserId(int userId)
        {
            var timestamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            var result = from device in _dbContext.Devices
                join deviceType in _dbContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
                join usage in _dbContext.DeviceEnergyUsage.Where(u => u.Timestamp == timestamp).DefaultIfEmpty() on
                    device.Id equals usage.DeviceId into usageGroup
                where device.UserId == userId
                group new { device.Id, device.Name, device.DeviceType, device.DeviceTypeId, device.ActivityStatus, Value = usageGroup.FirstOrDefault()!.Value, DataShare = device.DataShare } by deviceType.Category into grouped
                select new {
                    Category = grouped.Key,
                    Devices = grouped.ToList()
                };
            return result;
        }
        
        public async Task TurnDevicesOff()
        {
            using (_dbContext)
            {
                var devices = await _dbContext.Devices.ToListAsync();

                devices.ForEach(d => d.ActivityStatus = false);

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task TurnDevicesOn()
        {
            using (_dbContext)
            {
                var devices = await _dbContext.Devices.ToListAsync();

                devices.ForEach(d => d.ActivityStatus = true);

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task TurnDataSharingOff()
        {
            using (_dbContext)
            {
                var devices = await _dbContext.Devices.ToListAsync();

                devices.ForEach(d => d.DataShare = false);

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task TurnDataSharingOn()
        {
            using (_dbContext)
            {
                var devices = await _dbContext.Devices.ToListAsync();

                devices.ForEach(d => d.DataShare = true);

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<object> Top3DevicesByUserId(int userId)
        {
            var timestamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            var result = from device in _dbContext.Devices
                join deviceType in _dbContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
                join usage in _dbContext.DeviceEnergyUsage.Where(u => u.Timestamp == timestamp).DefaultIfEmpty()
                    on device.Id equals usage.DeviceId into usageGroup
                where device.UserId == userId && device.ActivityStatus == true
                group new { device.Name, Value = usageGroup.FirstOrDefault()!.Value }
                    by deviceType.Category into grouped
                orderby grouped.Max(g => g.Value) descending
                select new {
                    Category = grouped.Key,
                    Devices = grouped.OrderByDescending(g => g.Value).Take(4).ToList()
                };
            return await result.ToListAsync();
        }

        public async Task<Response<RegisterResponseViewModel>> TurnDeviceOffById(int deviceId)
        {
            var response = new Response<RegisterResponseViewModel>();
            var device = await _dbContext.Devices.Where(d => d.Id == deviceId).FirstOrDefaultAsync();

            if (device == null)
            {
                response.Errors.Add("Device doesen't exist");
                response.Success = false;

                return response;
            }

            device!.ActivityStatus = false;
            await _dbContext.SaveChangesAsync();

            response.Success = response.Errors.Count == 0;
            return response;
        }

        public async Task<Response<RegisterResponseViewModel>> TurnDeviceOnById(int deviceId)
        {
            var response = new Response<RegisterResponseViewModel>();
            var device = await _dbContext.Devices.Where(d => d.Id == deviceId).FirstOrDefaultAsync();

            if (device == null)
            {
                response.Errors.Add("Device doesen't exist");
                response.Success = false;

                return response;
            }

            device!.ActivityStatus = true;
            await _dbContext.SaveChangesAsync();

            response.Success = response.Errors.Count == 0;
            return response;
        }

        public async Task<Response<RegisterResponseViewModel>> ShareDataOffById(int deviceId)
        {
            var response = new Response<RegisterResponseViewModel>();
            var device = await _dbContext.Devices.Where(d => d.Id == deviceId).FirstOrDefaultAsync();

            if (device == null)
            {
                response.Errors.Add("Device doesen't exist");
                response.Success = false;

                return response;
            }

            device!.DataShare = false;
            await _dbContext.SaveChangesAsync();

            response.Success = response.Errors.Count == 0;
            return response;
        }

        public async Task<Response<RegisterResponseViewModel>> ShareDataOnById(int deviceId)
        {
            var response = new Response<RegisterResponseViewModel>();
            var device = await _dbContext.Devices.Where(d => d.Id == deviceId).FirstOrDefaultAsync();

            if (device == null)
            {
                response.Errors.Add("Device doesen't exist");
                response.Success = false;

                return response;
            }

            device!.DataShare = true;
            await _dbContext.SaveChangesAsync();

            response.Success = response.Errors.Count == 0;
            return response;
        }
    }
}
