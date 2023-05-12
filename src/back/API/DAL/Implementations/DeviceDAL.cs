using API.Common;
using API.DAL.Interfaces;
using API.Models;
using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.DeviceSimulatorService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
                PurchaseDate = d.PurchaseDate,
                ActivityStatus = d.ActivityStatus,
                DeviceType = d.DeviceType,
                DeviceSubtype = d.DeviceSubtype,
                Capacity = d.Capacity,
                UserId = d.UserId
            }).AsNoTracking().FirstOrDefaultAsync();
        }
        
        public async Task<Device?> GetWholeDeviceByIdAsync(int id)
        {
            return await _dbContext.Devices.FirstOrDefaultAsync(d => d.Id == id);
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
            
            // Remove all device jobs associated with the device
            var jobsToDelete = _dbContext.DeviceJobs.Where(x => x.DeviceId == dev.Id);
            _dbContext.DeviceJobs.RemoveRange(jobsToDelete);
            
            _dbContext.Devices.Remove(dev);

            await _dbContext.SaveChangesAsync();
        }
        public async Task<int> AddDeviceViewModel(DeviceViewModel devicee)
        {
            var device = new Device
            {
                UserId = devicee.UserId,
                ActivityStatus = true,
                PurchaseDate = DateTime.Now,
                DeviceTypeId = devicee.DeviceTypeId,
                DeviceSubtypeId = devicee.DeviceSubtypeId,
                Name = devicee.Name,
                DataShare = true,
                Capacity = devicee.Category == 0 ? devicee.Capacity : null

            };
            await _dbContext.Devices.AddAsync(device);
            await _dbContext.SaveChangesAsync();

            await _deviceSimulatorService.FillDataSinceJanuary1st(((int)devicee.DeviceTypeId!), device.Id);

            return device.Id;
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
                         join usage in _dbContext.DeviceEnergyUsage.Where(u => u.Timestamp == timestamp).DefaultIfEmpty()
                             on device.Id equals usage.DeviceId into usageGroup
                         where device.UserId == userId
                         group new
                         {
                             device.Id,
                             device.Name,
                             device.DeviceType,
                             device.DeviceTypeId,
                             device.ActivityStatus,
                             Value = usageGroup.FirstOrDefault()!.Value,
                             DataShare = device.DataShare
                         } by deviceType.Category into grouped
                         select new
                         {
                             Category = grouped.Key,
                             Devices = grouped.OrderByDescending(d => d.ActivityStatus)
                                             .ThenByDescending(d => d.Value)
                                             .ToList()
                         };

            return result;
        }
        
        public async Task<Response> TurnDevicesOff(int userId)
        {
            var response = new Response();
            
            await using (_dbContext)
            {
                var devices = await _dbContext.Devices.Where(dev => dev.UserId == userId).ToListAsync();

                if (devices.IsNullOrEmpty())
                {
                    response.Errors.Add("This user has no devices!");
                    response.Success = false;

                    return response;
                }
                
                foreach(var dev in devices)
                    await TurnDeviceOffById(dev.Id);

                await _dbContext.SaveChangesAsync();

                response.Data = "Devices turned off successfully";
                response.Success = true;

                return response;
            }
        }

        public async Task<Response> TurnDevicesOn(int userId)
        {
            var response = new Response();
            
            await using (_dbContext)
            {
                var devices = await _dbContext.Devices.Where(dev => dev.UserId == userId).ToListAsync();

                if (devices.IsNullOrEmpty())
                {
                    response.Errors.Add("This user has no devices!");
                    response.Success = false;

                    return response;
                }
                
                foreach(var dev in devices)
                    await TurnDeviceOnById(dev.Id);

                await _dbContext.SaveChangesAsync();

                response.Data = "Devices turned off successfully";
                response.Success = true;

                return response;
            }
        }

        public async Task<Response> TurnDataSharingOff(int userId)
        {
            var response = new Response();
            
            await using (_dbContext)
            {
                var devices = await _dbContext.Devices.Where(dev => dev.UserId == userId).ToListAsync();

                if (devices.IsNullOrEmpty())
                {
                    response.Errors.Add("This user has no devices!");
                    response.Success = false;

                    return response;
                }
                
                devices.ForEach(d => d.DataShare = false);

                await _dbContext.SaveChangesAsync();

                response.Data = "Devices Data sharing feature turned off successfully";
                response.Success = true;

                return response;
            }
        }

        public async Task<Response> TurnDataSharingOn(int userId)
        {
            var response = new Response();
            
            await using (_dbContext)
            {
                var devices = await _dbContext.Devices.Where(dev => dev.UserId == userId).ToListAsync();

                if (devices.IsNullOrEmpty())
                {
                    response.Errors.Add("This user has no devices!");
                    response.Success = false;

                    return response;
                }
                
                devices.ForEach(d => d.DataShare = true);

                await _dbContext.SaveChangesAsync();

                response.Data = "Devices Data sharing feature turned on successfully";
                response.Success = true;

                return response;
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

            var rand = new Random();
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            var usage = await _dbContext.DeviceEnergyUsage.Where(du => du.DeviceId == device.Id && du.Timestamp == now).FirstOrDefaultAsync();
            var deviceType = await _dbContext.DeviceTypes.Where(dt => dt.Id == device.DeviceTypeId).FirstOrDefaultAsync();
            if (usage != null)
            {
                Console.WriteLine(usage.Timestamp);
                if (deviceType?.Id == 3 && device?.ActivityStatus == true)
                {
                    usage.Value = Math.Round((double)(usage?.PredictedValue * (1 + rand.NextDouble() * 0.4 - 0.2))!, 3);
                }
                else if (deviceType?.Id == 3 && device?.ActivityStatus == false)
                {
                    usage.Value = 0;
                }
                else if (deviceType?.Id == 11)
                {
                    usage.Value = Math.Min(Math.Max(Math.Round((double)(usage?.PredictedValue * (1 + rand.NextDouble() * 0.4 - 0.2))!, 3), 0), 1);
                }
                else if (device?.ActivityStatus == true)
                {
                    usage.Value = Math.Round((double)(deviceType?.WattageInkW * (1 + rand.NextDouble() * 0.4 - 0.2))!, 3);
                }
                else
                {
                    usage.Value = 0;
                }
            }

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

            var rand = new Random();
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            var usage = await _dbContext.DeviceEnergyUsage.Where(du => du.DeviceId == device.Id && du.Timestamp == now).FirstOrDefaultAsync();
            var deviceType = await _dbContext.DeviceTypes.Where(dt => dt.Id == device.DeviceTypeId).FirstOrDefaultAsync();
            if (usage != null)
            {
                Console.WriteLine(usage.Timestamp);
                if (deviceType?.Id == 3 && device?.ActivityStatus == true)
                {
                    usage.Value = Math.Round((double)(usage?.PredictedValue * (1 + rand.NextDouble() * 0.4 - 0.2))!, 3);
                }
                else if (deviceType?.Id == 3 && device?.ActivityStatus == false)
                {
                    usage.Value = 0;
                }
                else if (deviceType?.Id == 11)
                {
                    usage.Value = Math.Min(Math.Max(Math.Round((double)(usage?.PredictedValue * (1 + rand.NextDouble() * 0.4 - 0.2))!, 3), 0), 1);
                }
                else if (device?.ActivityStatus == true)
                {
                    usage.Value = Math.Round((double)(deviceType?.WattageInkW * (1 + rand.NextDouble() * 0.4 - 0.2))!, 3);
                }
                else
                {
                    usage.Value = 0;
                }
            }

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

        public async Task<List<DeviceSubtype>> GetDeviceSubtypesByType(int deviceTypeId)
        {
            return await _dbContext.DeviceSubtypes.Where(dt => dt.DeviceTypeId == deviceTypeId)
                                               .AsNoTracking().ToListAsync();
        }

        public async Task<object> GetDevicesIdAndNameByUserId(int userId)
        {
            return await _dbContext.Devices.Where(d => d.UserId == userId)
                .Where( device => device.DeviceType!.Category != 0)
                .Select(d => new
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task TurnDsoControl(bool state, Device? dev)
        {
            //var device = await _dbContext.Devices.Where(d => d.Id == dev!.Id).FirstOrDefaultAsync();
            dev!.DsoControl = state;
            await _dbContext.SaveChangesAsync();
        }
    }
}
