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
            .Where(du => du.DeviceId == deviceId && du.Timestamp!.Value.Date == DateTime.Now.Date)
            .Select( du => new { Timestamp = du.Timestamp, Value = du.Value})
            .OrderBy( du => du.Timestamp).AsNoTracking()
            .ToListAsync();
    }

    public async Task<object> GetDeviceDataForMonth(int deviceId)
    {
        var query = from usage in _dataContext.DeviceEnergyUsage
                    where usage.DeviceId == deviceId && usage.Timestamp!.Value.Year == DateTime.Now.Year && usage.Timestamp.Value.Month == DateTime.Now.Month
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
                        Timestamp = usageGroup.Key.Month + "/" + usageGroup.Key.Year,
                        Value = usageGroup.Sum(u => u.Value)
                    };

        return await query.ToListAsync();
    }

    public async Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForToday( int day, int month, int year)
    {
        var now = new DateTime(year, month, day, 0, 0, 0);
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
            where device.DataShare && energyUsage.Timestamp!.Value.Date == now /*&& energyUsage.Timestamp!.Value < DateTime.Now*/ && deviceType.Category == -1
            group energyUsage by new { Hour = energyUsage.Timestamp!.Value.Hour } into g
            select new
            {
                Timestamp = new DateTime(now.Year, now.Month, now.Day, g.Key.Hour, 0, 0),
                Value = g.Sum(eu => eu.Value)
            }
        ).AsNoTracking().ToListAsync();

        var producingEnergyUsageByTimestamp = await (
            from energyUsage in _dataContext.DeviceEnergyUsage
            join device in _dataContext.Devices on energyUsage.DeviceId equals device.Id
            join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
            where device.DataShare && energyUsage.Timestamp!.Value.Date == now.Date /*&& energyUsage.Timestamp!.Value < DateTime.Now*/ && deviceType.Category == 1
            group energyUsage by new { Hour = energyUsage.Timestamp!.Value.Hour } into g
            select new
            {
                Timestamp = new DateTime(year, month, day, g.Key.Hour, 0, 0),
                Value = g.Sum(eu => eu.Value)
            }
        ).AsNoTracking().ToListAsync();

        return new { producingEnergyUsageByTimestamp, consumingEnergyUsageByTimestamp };
    }

    public async Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth(int month, int year)
    {
        var consumingEnergyUsageByTimestamp = await (
             from energyUsage in _dataContext.DeviceEnergyUsage
             join device in _dataContext.Devices on energyUsage.DeviceId equals device.Id
             join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
             where device.DataShare && energyUsage.Timestamp!.Value.Year == year && energyUsage.Timestamp!.Value.Month == month /*&& energyUsage.Timestamp.Value < DateTime.Now */&& deviceType.Category == -1
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
             where device.DataShare && energyUsage.Timestamp!.Value.Year == year && energyUsage.Timestamp!.Value.Month == month /*&& energyUsage.Timestamp.Value < DateTime.Now */&& deviceType.Category == 1
             group energyUsage by energyUsage.Timestamp!.Value.Date into g
             select new
             {
                 Timestamp = g.Key.Date.ToShortDateString(),
                 Value = g.Sum(eu => eu.Value)
             }).AsNoTracking().ToListAsync();

        return new { producingEnergyUsageByTimestamp, consumingEnergyUsageByTimestamp };
    }

    public async Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForYear(int year)
    {
        var consumingEnergyUsageByTimestamp = await (
            from energyUsage in _dataContext.DeviceEnergyUsage
            join device in _dataContext.Devices on energyUsage.DeviceId equals device.Id
            join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
            where device.DataShare && energyUsage.Timestamp!.Value.Year == year && deviceType.Category == -1
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
           where device.DataShare && energyUsage.Timestamp!.Value.Year == year && deviceType.Category == 1 && energyUsage.Timestamp!.Value <= DateTime.Now
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

    public async Task<object> GetTotalConsumptionForPrevious7HoursAndTotalProductionForNext7HoursForAllUsers()
    {
        var currentTimestamp = DateTime.Now;
        var startConsumptionTimestamp = currentTimestamp.AddHours(-7);
        var endProductionTimestamp = currentTimestamp.AddHours(7);

        var consumptionEnergyUsage = await _dataContext.DeviceEnergyUsage
            .Join(_dataContext.Devices, energyUsage => energyUsage.DeviceId, device => device.Id, (energyUsage, device) => new { EnergyUsage = energyUsage, Device = device })
            .Join(_dataContext.DeviceTypes, joined => joined.Device.DeviceTypeId, type => type.Id, (joined, type) => new { EnergyUsage = joined.EnergyUsage, Device = joined.Device, Category = type.Category!.Value })
            .Where(joined => joined.EnergyUsage.Timestamp!.Value >= startConsumptionTimestamp && joined.EnergyUsage.Timestamp!.Value < endProductionTimestamp && joined.Device.DataShare && joined.Category == -1)
            .GroupBy(joined => new { Timestamp = joined.EnergyUsage.Timestamp!.Value.Date, Hour = joined.EnergyUsage.Timestamp!.Value.Hour })
            .Select(grouped => new { Timestamp = new DateTime(grouped.Key.Timestamp.Year, grouped.Key.Timestamp.Month, grouped.Key.Timestamp.Day, grouped.Key.Hour, 0, 0), Value = Math.Abs(grouped.Sum(joined => joined.EnergyUsage.Value!.Value)) })
            .AsNoTracking().ToListAsync();

        var productionEnergyUsage = await _dataContext.DeviceEnergyUsage
            .Join(_dataContext.Devices, energyUsage => energyUsage.DeviceId, device => device.Id, (energyUsage, device) => new { EnergyUsage = energyUsage, Device = device })
            .Join(_dataContext.DeviceTypes, joined => joined.Device.DeviceTypeId, type => type.Id, (joined, type) => new { EnergyUsage = joined.EnergyUsage, Device = joined.Device, Category = type.Category!.Value })
            .Where(joined => joined.EnergyUsage.Timestamp!.Value >= startConsumptionTimestamp && joined.EnergyUsage.Timestamp!.Value <= endProductionTimestamp && joined.Device.DataShare && joined.Category == 1)
            .GroupBy(joined => new { Timestamp = joined.EnergyUsage.Timestamp!.Value.Date, Hour = joined.EnergyUsage.Timestamp!.Value.Hour })
            .Select(grouped => new { Timestamp = new DateTime(grouped.Key.Timestamp.Year, grouped.Key.Timestamp.Month, grouped.Key.Timestamp.Day, grouped.Key.Hour, 0, 0), Value = grouped.Sum(joined => joined.EnergyUsage.Value!.Value) })
            .AsNoTracking().ToListAsync();

        return new { consumptionEnergyUsage, productionEnergyUsage };

    }
    /*
    public Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForTodayPrediction()
    {
        throw new NotImplementedException();
    }
    */
    public async Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForTomorrowPrediction()
    {
        var consumingEnergyUsageByTimestamp = await (
            from deviceEnergyUsage in _dataContext.DeviceEnergyUsage
            join device in _dataContext.Devices on deviceEnergyUsage.DeviceId equals device.Id
            join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
            where device.DataShare
                && deviceType.Category == -1
                && deviceEnergyUsage.Timestamp > DateTime.Now.Date.AddDays(1)
                && deviceEnergyUsage.Timestamp < DateTime.Now.Date.AddDays(2)
            group deviceEnergyUsage by new { deviceEnergyUsage.Timestamp!.Value.Date, deviceEnergyUsage.Timestamp.Value.Hour } into g
            select new
            {
                Timestamp = new DateTime(g.Key.Date.Year, g.Key.Date.Month, g.Key.Date.Day, g.Key.Hour, 0, 0),
                Value = g.Sum(eu => eu.Value)
            }
        ).AsNoTracking().ToListAsync();

        var producingEnergyUsageByTimestamp = await (
            from deviceEnergyUsage in _dataContext.DeviceEnergyUsage
            join device in _dataContext.Devices on deviceEnergyUsage.DeviceId equals device.Id
            join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
            where device.DataShare
                && deviceType.Category == 1
                && deviceEnergyUsage.Timestamp > DateTime.Now.Date.AddDays(1)
                && deviceEnergyUsage.Timestamp < DateTime.Now.Date.AddDays(2)
            group deviceEnergyUsage by new { deviceEnergyUsage.Timestamp!.Value.Date, deviceEnergyUsage.Timestamp.Value.Hour } into g
            select new
            {
                Timestamp = new DateTime(g.Key.Date.Year, g.Key.Date.Month, g.Key.Date.Day, g.Key.Hour, 0, 0),
                Value = g.Sum(eu => eu.Value)
            }
        ).AsNoTracking().ToListAsync();

        return new { producingEnergyUsageByTimestamp, consumingEnergyUsageByTimestamp };

    }

    public async Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForNext3DaysPrediction()
    {
        var now = DateTime.Now;
        var startDateTime = new DateTime(now.Year, now.Month, now.Day + 1, 0, 0, 0); // start at midnight tomorrow
        var endDateTime = new DateTime(now.Year, now.Month, now.Day + 4, 0, 0, 0); // end at midnight three days from now

        var consumingEnergyUsageByTimestamp = await(
            from deviceEnergyUsage in _dataContext.DeviceEnergyUsage
            join device in _dataContext.Devices on deviceEnergyUsage.DeviceId equals device.Id
            join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
            where device.DataShare
                && deviceType.Category == -1
                && deviceEnergyUsage.Timestamp >= startDateTime
                && deviceEnergyUsage.Timestamp < endDateTime
            group deviceEnergyUsage by new { deviceEnergyUsage.Timestamp!.Value.Date, deviceEnergyUsage.Timestamp.Value.Hour } into g
            select new
            {
                Timestamp = new DateTime(g.Key.Date.Year, g.Key.Date.Month, g.Key.Date.Day, g.Key.Hour, 0, 0),
                Value = g.Sum(eu => eu.Value)
            }
        ).AsNoTracking().ToListAsync();

        var producingEnergyUsageByTimestamp = await(
            from deviceEnergyUsage in _dataContext.DeviceEnergyUsage
            join device in _dataContext.Devices on deviceEnergyUsage.DeviceId equals device.Id
            join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
            where device.DataShare
                && deviceType.Category == 1
                && deviceEnergyUsage.Timestamp >= startDateTime
                && deviceEnergyUsage.Timestamp < endDateTime
            group deviceEnergyUsage by new { deviceEnergyUsage.Timestamp!.Value.Date, deviceEnergyUsage.Timestamp.Value.Hour } into g
            select new
            {
                Timestamp = new DateTime(g.Key.Date.Year, g.Key.Date.Month, g.Key.Date.Day, g.Key.Hour, 0, 0),
                Value = g.Sum(eu => eu.Value)
            }
        ).AsNoTracking().ToListAsync();

        return new { producingEnergyUsageByTimestamp, consumingEnergyUsageByTimestamp };

    }

    public async Task<object> GetAllDevicesDataWhereShareWithDsoIsAllowedForNext7DaysPrediction()
    {
        var now = DateTime.Now;
        var startDateTime = now.Date.AddDays(1); // start at midnight tomorrow
        var endDateTime = now.Date.AddDays(8); // end at midnight seven days from now

        var consumingEnergyUsageByTimestamp = await (
            from deviceEnergyUsage in _dataContext.DeviceEnergyUsage
            join device in _dataContext.Devices on deviceEnergyUsage.DeviceId equals device.Id
            join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
            where device.DataShare
                && deviceType.Category == -1
                && deviceEnergyUsage.Timestamp >= startDateTime
                && deviceEnergyUsage.Timestamp < endDateTime                
            group deviceEnergyUsage by deviceEnergyUsage.Timestamp!.Value.Date into g
            select new
            {
                Timestamp = g.Key.Date.ToShortDateString(),
                Value = g.Sum(eu => eu.Value)
            }
        ).AsNoTracking().ToListAsync();

        var producingEnergyUsageByTimestamp = await (
            from deviceEnergyUsage in _dataContext.DeviceEnergyUsage
            join device in _dataContext.Devices on deviceEnergyUsage.DeviceId equals device.Id
            join deviceType in _dataContext.DeviceTypes on device.DeviceTypeId equals deviceType.Id
            where device.DataShare
                && deviceType.Category == 1
                && deviceEnergyUsage.Timestamp >= startDateTime
                && deviceEnergyUsage.Timestamp < endDateTime
            group deviceEnergyUsage by deviceEnergyUsage.Timestamp!.Value.Date into g
            select new
            {
                //Timestamp = g.Key,
                Timestamp = g.Key.Date.ToShortDateString(),
                Value = g.Sum(eu => eu.Value)
            }
        ).AsNoTracking().ToListAsync();

        return new { producingEnergyUsageByTimestamp, consumingEnergyUsageByTimestamp };

    }
}