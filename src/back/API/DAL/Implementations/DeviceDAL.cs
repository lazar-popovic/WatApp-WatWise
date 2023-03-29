﻿using API.DAL.Interfaces;
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
                ActivityStatus = d.ActivityStatus,
                DeviceType = d.DeviceType,
                UserId = d.UserId
            }).FirstOrDefaultAsync();
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
            var device = await GetDeviceByIdAsync(id);
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
                DeviceTypeId = devicee.DeviceTypeId,
                Name = devicee.Name,
                DataShare = false
            };
            await _dbContext.Devices.AddAsync(device);
            await _dbContext.SaveChangesAsync();

            await _deviceSimulatorService.FillDataSinceJanuary1st(((int)devicee.DeviceTypeId), device.Id);
        }

        public async Task<List<DeviceType>> GetDeviceTypesByCategory(int id)
        {
            return await _dbContext.DeviceTypes.Where(dt => dt.Category == id)
                                               .Select(dt => new DeviceType { Id = dt.Id, Type = dt.Type })
                                               .ToListAsync();
        }

        public object GetDevicesByUserId(int userId)
        {
            var timestamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            var result = from device in _dbContext.Devices
                join deviceType in _dbContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
                join usage in _dbContext.DeviceEnergyUsage.Where(u => u.Timestamp == timestamp).DefaultIfEmpty() on
                    device.Id equals usage.DeviceId into usageGroup
                where device.UserId == userId
                group new { device.Id, device.Name, device.ActivityStatus, Value = usageGroup.FirstOrDefault().Value } by deviceType.Category into grouped
                select new {
                    Category = grouped.Key,
                    Devices = grouped.ToList()
                };
            return result;
        }
    }
}
