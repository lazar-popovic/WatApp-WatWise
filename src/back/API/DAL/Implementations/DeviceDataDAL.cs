using API.DAL.Interfaces;
using API.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace API.DAL.Implementations;

public class DeviceDataDAL : IDeviceDataDAL
{
    private readonly DataContext _dataContext;

    public DeviceDataDAL(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<object> GetDeviceDataForToday(int deviceId)
    {
        return await _dataContext.DeviceEnergyUsage
            .Where(du => du.DeviceId == deviceId && du.Timestamp!.Value.Date == DateTime.Now.Date && du.Timestamp.Value < DateTime.Now)
            .Select( du => new { Timestamp = du.Timestamp, Value = du.Value})
            .OrderBy( du => du.Timestamp).AsNoTracking()
            .ToListAsync();
    }

    public async Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForToday()
    {
        return await _dataContext.DeviceEnergyUsage
                .Join(_dataContext.Devices,
                    energyUsage => energyUsage.DeviceId,
                    device => device.Id,
                    (energyUsage, device) => new { EnergyUsage = energyUsage, Device = device })
                .Join(_dataContext.Devices
                    .Join(_dataContext.DeviceTypes,
                        device => device.DeviceTypeId,
                        deviceType => deviceType.Id,
                        (device, deviceType) => new { Device = device, DeviceType = deviceType }),
                    joined => joined.Device.Id,
                    deviceJoin => deviceJoin.Device.Id,
                    (joined, deviceJoin) => new { EnergyUsage = joined.EnergyUsage, Device = deviceJoin.Device, DeviceType = deviceJoin.DeviceType })
                .Where(joined => joined.DeviceType.Category == -1 && joined.Device.DataShare && joined.EnergyUsage.Timestamp!.Value.Date == DateTime.Now.Date && joined.EnergyUsage.Timestamp.Value < DateTime.Now)
                .GroupBy(joined => joined.DeviceType.Type)
                .Select(group => new { Type = group.Key, EnergyUsageSum = group.Sum(joined => joined.EnergyUsage.Value) }).AsNoTracking()
                .ToListAsync();
    }

    public async Task<object> GetDeviceDataForMonth(int deviceId)
    {
        var query = from usage in _dataContext.DeviceEnergyUsage
            where usage.DeviceId == deviceId && usage.Timestamp!.Value.Year == DateTime.Now.Year && usage.Timestamp.Value.Month == DateTime.Now.Month && usage.Timestamp.Value < DateTime.Now
            group usage by usage.Timestamp!.Value.Date into usageGroup
            select new
            {
                Timestamp = usageGroup.Key.Date.ToShortDateString(),
                Value = usageGroup.Sum(u => u.Value)
            };

        return await query.ToListAsync();
    }

    public async Task<object> GetDeviceDataForYear(int deviceId)
    {
        var query = from usage in _dataContext.DeviceEnergyUsage
            where usage.DeviceId == deviceId && usage.Timestamp!.Value.Year == DateTime.Now.Year && usage.Timestamp.Value < DateTime.Now
            group usage by new { usage.Timestamp!.Value.Year, usage.Timestamp.Value.Month } into usageGroup
            select new
            {
                Timestamp = usageGroup.Key.Month +"/"+usageGroup.Key.Year,
                Value = usageGroup.Sum(u => u.Value)
            };

        return await query.ToListAsync();
    }

    public async Task<object> GetDayTotalProductionConsumptionByUserId( int day, int month, int year, int userId)
    {
        var startTimestamp = new DateTime(year, month, day);
        
        Console.WriteLine( startTimestamp);
        
        var producingEnergyUsageByTimestamp = await _dataContext.DeviceEnergyUsage
            .Join(_dataContext.Devices, energyUsage => energyUsage.DeviceId, device => device.Id, (energyUsage, device) => new { EnergyUsage = energyUsage, Device = device })
            .Join(_dataContext.DeviceTypes, joined => joined.Device.DeviceTypeId, type => type.Id, (joined, type) => new { EnergyUsage = joined.EnergyUsage, Device = joined.Device, Category = type.Category.Value })
            .Where(joined => joined.EnergyUsage.Timestamp.Value.Date == startTimestamp.Date && joined.Device.UserId == userId && joined.Category == 1)
            .GroupBy(joined => new { Timestamp = joined.EnergyUsage.Timestamp.Value})
            .Select(grouped => new { Timestamp = grouped.Key.Timestamp, TotalEnergyUsage = grouped.Sum(joined => joined.EnergyUsage.Value) })
            .ToListAsync();

        var consumingEnergyUsageByTimestamp = await _dataContext.DeviceEnergyUsage
            .Join(_dataContext.Devices, energyUsage => energyUsage.DeviceId, device => device.Id, (energyUsage, device) => new { EnergyUsage = energyUsage, Device = device })
            .Join(_dataContext.DeviceTypes, joined => joined.Device.DeviceTypeId, type => type.Id, (joined, type) => new { EnergyUsage = joined.EnergyUsage, Device = joined.Device, Category = type.Category.Value })
            .Where(joined => joined.EnergyUsage.Timestamp.Value.Date == startTimestamp.Date && joined.Device.UserId == userId && joined.Category == -1)
            .GroupBy(joined => new { Timestamp = joined.EnergyUsage.Timestamp.Value})
            .Select(grouped => new { Timestamp = grouped.Key.Timestamp, TotalEnergyUsage = Math.Abs(grouped.Sum(joined => joined.EnergyUsage.Value.Value)) })
            .ToListAsync();

        return new { producingEnergyUsageByTimestamp, consumingEnergyUsageByTimestamp};
    }

}