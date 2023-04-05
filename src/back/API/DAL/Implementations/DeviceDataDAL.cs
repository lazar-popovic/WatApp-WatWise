using API.DAL.Interfaces;
using API.Models.Entity;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        /*
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
                .ToListAsync(); */
        /*
        return await _dataContext.DeviceEnergyUsage
                .Join(_dataContext.Devices,
                    energyUsage => energyUsage.DeviceId,
                    device => device.Id,
                    (energyUsage, device) => new { EnergyUsage = energyUsage, Device = device })
                .Where(joined => joined.Device.DataShare && joined.EnergyUsage.Timestamp!.Value.Date == DateTime.Now.Date && joined.EnergyUsage.Timestamp.Value < DateTime.Now)
                .GroupBy(joined => new { Hour = joined.EnergyUsage.Timestamp!.Value.Hour, Id = joined.Device.Id })
                .Select(group => new { Timestamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, group.Key.Hour, 0, 0), Value = group.Sum(joined => joined.EnergyUsage.Value) }).AsNoTracking()
                .ToListAsync();*/

        var consumingEnergyUsageByTimestamp = await (
            from energyUsage in _dataContext.DeviceEnergyUsage
            join device in _dataContext.Devices on energyUsage.DeviceId equals device.Id
            join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
            where device.DataShare && energyUsage.Timestamp!.Value.Date == DateTime.Now.Date && energyUsage.Timestamp!.Value < DateTime.Now && deviceType.Category == -1
            group energyUsage by new { Hour = energyUsage.Timestamp!.Value.Hour } into g
            select new
            {
                Timestamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, g.Key.Hour, 0, 0),
                Value = g.Sum(eu => eu.Value)
            }
        ).AsNoTracking().ToListAsync();

        var producingEnergyUsageByTimestamp = await (
            from energyUsage in _dataContext.DeviceEnergyUsage
            join device in _dataContext.Devices on energyUsage.DeviceId equals device.Id
            join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
            where device.DataShare && energyUsage.Timestamp!.Value.Date == DateTime.Now.Date && energyUsage.Timestamp!.Value < DateTime.Now && deviceType.Category == 1
            group energyUsage by new { Hour = energyUsage.Timestamp!.Value.Hour } into g
            select new
            {
                Timestamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, g.Key.Hour, 0, 0),
                Value = g.Sum(eu => eu.Value)
            }
        ).AsNoTracking().ToListAsync();

        return new { producingEnergyUsageByTimestamp, consumingEnergyUsageByTimestamp };
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

    public async Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth()
    {
        var consumingEnergyUsageByTimestamp = await (
             from energyUsage in _dataContext.DeviceEnergyUsage
             join device in _dataContext.Devices on energyUsage.DeviceId equals device.Id
             join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
             where device.DataShare && energyUsage.Timestamp!.Value.Year == DateTime.Now.Year && energyUsage.Timestamp!.Value.Month == DateTime.Now.Month && energyUsage.Timestamp.Value < DateTime.Now && deviceType.Category == -1
             group energyUsage by energyUsage.Timestamp!.Value.Date into g
             select new
             {
                 Timestamp = g.Key.Date.ToShortDateString(),
                 Value = g.Sum(eu => eu.Value)
             }).AsNoTracking().ToListAsync();

        var producingEnergyUsageByTimestamp = await (
             from energyUsage in _dataContext.DeviceEnergyUsage
             join device in _dataContext.Devices on energyUsage.DeviceId equals device.Id
             join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
             where device.DataShare && energyUsage.Timestamp!.Value.Year == DateTime.Now.Year && energyUsage.Timestamp!.Value.Month == DateTime.Now.Month && energyUsage.Timestamp.Value < DateTime.Now && deviceType.Category == 1
             group energyUsage by energyUsage.Timestamp!.Value.Date into g
             select new
             {
                 Timestamp = g.Key.Date.ToShortDateString(),
                 Value = g.Sum(eu => eu.Value)
             }).AsNoTracking().ToListAsync();

        return new { producingEnergyUsageByTimestamp, consumingEnergyUsageByTimestamp };
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

    public async Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForYear()
    {
        var consumingEnergyUsageByTimestamp = await (
            from energyUsage in _dataContext.DeviceEnergyUsage
            join device in _dataContext.Devices on energyUsage.DeviceId equals device.Id
            join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
            where device.DataShare && energyUsage.Timestamp!.Value.Year == DateTime.Now.Year && deviceType.Category == -1
            group energyUsage by new { energyUsage.Timestamp!.Value.Year, energyUsage.Timestamp.Value.Month } into g
            select new
            {
                Timestamp = g.Key.Month + "/" + g.Key.Year,
                Value = Math.Abs((decimal)g.Sum(eu => eu.Value)!)
            }).AsNoTracking().ToListAsync();

        var producingEnergyUsageByTimestamp = await (
           from energyUsage in _dataContext.DeviceEnergyUsage
           join device in _dataContext.Devices on energyUsage.DeviceId equals device.Id
           join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
           where device.DataShare && energyUsage.Timestamp!.Value.Year == DateTime.Now.Year && deviceType.Category == 1
           group energyUsage by new { energyUsage.Timestamp!.Value.Year, energyUsage.Timestamp.Value.Month } into g
           select new
           {
               Timestamp = g.Key.Month + "/" + g.Key.Year,
               Value =g.Sum(eu => eu.Value)
           }).AsNoTracking().ToListAsync();

        return new { producingEnergyUsageByTimestamp, consumingEnergyUsageByTimestamp };
    }

    public async Task<object> GetDayTotalProductionConsumptionByUserId( int day, int month, int year, int userId)
    {
        var startTimestamp = new DateTime(year, month, day);
        
        Console.WriteLine( startTimestamp);
        
        var producingEnergyUsageByTimestamp = await _dataContext.DeviceEnergyUsage
            .Join(_dataContext.Devices, energyUsage => energyUsage.DeviceId, device => device.Id, (energyUsage, device) => new { EnergyUsage = energyUsage, Device = device })
            .Join(_dataContext.DeviceTypes, joined => joined.Device.DeviceTypeId, type => type.Id, (joined, type) => new { EnergyUsage = joined.EnergyUsage, Device = joined.Device, Category = type.Category!.Value })
            .Where(joined => joined.EnergyUsage.Timestamp!.Value.Date == startTimestamp.Date && joined.Device.UserId == userId && joined.Category == 1)
            .GroupBy(joined => new { Timestamp = joined.EnergyUsage.Timestamp!.Value})
            .Select(grouped => new { Timestamp = grouped.Key.Timestamp, TotalEnergyUsage = grouped.Sum(joined => joined.EnergyUsage.Value) })
            .ToListAsync();

        var consumingEnergyUsageByTimestamp = await _dataContext.DeviceEnergyUsage
            .Join(_dataContext.Devices, energyUsage => energyUsage.DeviceId, device => device.Id, (energyUsage, device) => new { EnergyUsage = energyUsage, Device = device })
            .Join(_dataContext.DeviceTypes, joined => joined.Device.DeviceTypeId, type => type.Id, (joined, type) => new { EnergyUsage = joined.EnergyUsage, Device = joined.Device, Category = type.Category!.Value })
            .Where(joined => joined.EnergyUsage.Timestamp!.Value.Date == startTimestamp.Date && joined.Device.UserId == userId && joined.Category == -1)
            .GroupBy(joined => new { Timestamp = joined.EnergyUsage.Timestamp!.Value})
            .Select(grouped => new { Timestamp = grouped.Key.Timestamp, TotalEnergyUsage = Math.Abs(grouped.Sum(joined => joined.EnergyUsage.Value!.Value)) })
            .ToListAsync();

        return new { producingEnergyUsageByTimestamp, consumingEnergyUsageByTimestamp};
    }
}