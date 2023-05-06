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
    private static int firstJobReccuring = 1;
    private static int secondJobReccuring = 2;
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

    public async Task ScheduleJob(DeviceJobViewModel request)
    {
        string? firstJobId = null;
        string? secondJobId = null;

        var deviceJob = new DeviceJob
        {
            DeviceId = request.DeviceId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Turn = request.Turn,
            Repeat = request.Repeat,
            Canceled = false
        };
        
        
        // Set up job recurrence if needed
        if (!deviceJob.Repeat)
        {
             firstJobId = BackgroundJob.Schedule(() => ExecuteJob(deviceJob.DeviceId, deviceJob.Turn), request.StartDate.AddHours(-2));

            // Schedule the second job
             secondJobId =
                BackgroundJob.Schedule(() => ExecuteJob(deviceJob.DeviceId, !deviceJob.Turn), request.EndDate.AddHours(-2));
             
             deviceJob.StartJobId = int.Parse(firstJobId);
             deviceJob.EndJobId = int.Parse(secondJobId);
             await _dbContext.DeviceJobs.AddAsync(deviceJob);
             await _dbContext.SaveChangesAsync();
        }
        else
        {
            var startDateTime = request.StartDate.AddDays(1);
            var endDateTime = request.EndDate.AddDays(1);

            RecurringJob.AddOrUpdate(firstJobReccuring.ToString(),() => ExecuteJob(deviceJob.DeviceId, deviceJob.Turn),
                CronMaker.ToCron(startDateTime.AddHours(-2)));
            RecurringJob.AddOrUpdate(secondJobReccuring.ToString(),() => ExecuteJob(deviceJob.DeviceId, !deviceJob.Turn),
                CronMaker.ToCron(endDateTime.AddHours(-2)));

            deviceJob.StartJobId = firstJobReccuring;
            deviceJob.EndJobId = secondJobReccuring;
            await _dbContext.DeviceJobs.AddAsync(deviceJob);
            await _dbContext.SaveChangesAsync();

            firstJobReccuring += 2;
            secondJobReccuring += 2;
        }
    }

    public async Task<Response> GetActiveJobForDeviceId(int deviceId)
    {
        var response = new Response();

        response.Success = true;
        response.Data = await _dbContext.DeviceJobs.Where( dj => dj.DeviceId == deviceId && ((dj.Repeat == true) || (dj.Repeat == false && dj.EndDate < DateTime.Now)))
                                                   .Select( dj => new
                                                   {
                                                       Id = dj.Id,
                                                       StartDate = dj.StartDate,
                                                       EndDate = dj.EndDate,
                                                       Turn = dj.Turn,
                                                       Repeat = dj.Repeat
                                                   })
                                                   .AsNoTracking()
                                                   .FirstOrDefaultAsync();

        return response;
    }

    public async Task<Response> RemoveReccuringJobForJobId(int jobId)
    {
        var response = new Response();
        
        RecurringJob.RemoveIfExists(jobId.ToString());
        
        var canceledReccuringJob = await _dbContext.DeviceJobs.Where(job => job.Id == jobId).FirstOrDefaultAsync();
        if (canceledReccuringJob == null)
        {
            response.Errors.Add("Job for given id does not exist!");
            response.Success = false;

            return response;
        }
        canceledReccuringJob.Canceled = true;
        await _dbContext.SaveChangesAsync();
        
        response.Data = "Job removed successfully!";
        response.Success = true;

        return response;
    }
}