using API.Common;
using API.Models;
using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.DeviceScheduling.Interfaces;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace API.Services.DeviceScheduling.Implementations;

public class DeviceScheduler : IDeviceScheduler
{
    private readonly DataContext _dbContext;

    public DeviceScheduler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteJob(int? deviceId, bool? turn)
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

    public async Task<Response?> ScheduleJob(DeviceJobViewModel request)
    {
        var deviceJob = new DeviceJob
        {
            DeviceId = request.DeviceId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Turn = request.Turn,
            Repeat = request.Repeat
        };

        // Schedule the first job
        var firstJobId = BackgroundJob.Schedule(() => ExecuteJob(deviceJob.DeviceId, deviceJob.Turn), deviceJob.StartDate);

        // Schedule the second job
        var secondJobId =
            BackgroundJob.Schedule(() => ExecuteJob(deviceJob.DeviceId, !deviceJob.Turn), deviceJob.EndDate);

        // Set up job recurrence if needed
        if (deviceJob.Repeat)
        {
            RecurringJob.AddOrUpdate(firstJobId, () => ExecuteJob(deviceJob.DeviceId, deviceJob.Turn),
                deviceJob.RepeatInterval);
            RecurringJob.AddOrUpdate(secondJobId, () => ExecuteJob(deviceJob.DeviceId, !deviceJob.Turn),
                deviceJob.RepeatInterval);
        }

        // Save the job IDs in the database
        deviceJob.StartJobId = firstJobId;
        deviceJob.EndJobId = secondJobId;
        await _dbContext.DeviceJobs.AddAsync(deviceJob);
        await _dbContext.SaveChangesAsync();
    }
}