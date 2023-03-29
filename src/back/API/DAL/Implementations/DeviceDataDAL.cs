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
            .Where(du => du.DeviceId == deviceId && du.Timestamp.Value.Date == DateTime.Now.Date && du.Timestamp.Value < DateTime.Now)
            .Select( du => new { Timestamp = du.Timestamp, Value = du.Value})
            .OrderBy( du => du.Timestamp)
            .ToListAsync();
    }

    public async Task<object> GetDeviceDataForMonth(int deviceId)
    {
        var query = from usage in _dataContext.DeviceEnergyUsage
            where usage.DeviceId == deviceId && usage.Timestamp.Value.Year == DateTime.Now.Year && usage.Timestamp.Value.Month == DateTime.Now.Month && usage.Timestamp.Value < DateTime.Now
            group usage by usage.Timestamp.Value.Date into usageGroup
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
            where usage.DeviceId == deviceId && usage.Timestamp.Value.Year == DateTime.Now.Year && usage.Timestamp.Value < DateTime.Now
            group usage by new { usage.Timestamp.Value.Year, usage.Timestamp.Value.Month } into usageGroup
            select new
            {
                Timestamp = usageGroup.Key.Month +"/"+usageGroup.Key.Year,
                Value = usageGroup.Sum(u => u.Value)
            };

        return await query.ToListAsync();
    }
}