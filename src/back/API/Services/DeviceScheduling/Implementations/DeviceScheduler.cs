using API.Common;
using API.Models;
using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.DeviceScheduling.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services.DeviceScheduling.Implementations;

public class DeviceScheduler : IDeviceScheduler
{
    private readonly DataContext _dbContext;

    public DeviceScheduler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteJob(int deviceId, bool turn)
    {
        var device = await _dbContext.Devices.FirstOrDefaultAsync(device => device.Id == deviceId);

        if (device != null)
        {
            device.ActivityStatus = turn;
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            throw new NullReferenceException("Given device does not exist!");
        }
       
    }

    public Task<Response?> ScheduleJob(DeviceJobViewModel request)
    {
        throw new NotImplementedException();
    }
}