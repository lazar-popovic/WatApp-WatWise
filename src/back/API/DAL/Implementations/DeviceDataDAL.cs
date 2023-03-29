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
            .Where(du => du.DeviceId == deviceId && du.Timestamp.Value.Date == DateTime.Now.Date)
            .Select( du => new { du.Timestamp, du.Value})
            .OrderBy( du => du.Timestamp)
            .ToListAsync();
    }

    public async Task<object> GetDeviceDataForMonth(int deviceId)
    {
        var query = from usage in _dataContext.DeviceEnergyUsage
            where usage.DeviceId == deviceId && usage.Timestamp.Value.Year == DateTime.Now.Year && usage.Timestamp.Value.Month == DateTime.Now.Month
            group usage by usage.Timestamp.Value.Date into usageGroup
            select new
            {
                Date = usageGroup.Key,
                TotalUsage = usageGroup.Sum(u => u.Value)
            };

        return await query.ToListAsync();
    }

    public async Task<object> GetDeviceDataForYear(int deviceId)
    {
        var query = from usage in _dataContext.DeviceEnergyUsage
            where usage.DeviceId == deviceId && usage.Timestamp.Value.Year == DateTime.Now.Year
            group usage by new { usage.Timestamp.Value.Year, usage.Timestamp.Value.Month } into usageGroup
            select new
            {
                Year = usageGroup.Key.Year,
                Month = usageGroup.Key.Month,
                TotalUsage = usageGroup.Sum(u => u.Value)
            };

        return await query.ToListAsync();
    }
}