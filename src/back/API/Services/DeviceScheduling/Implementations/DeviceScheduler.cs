﻿using API.Common;
using API.Models;
using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.DeviceScheduling.Interfaces;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Services.DeviceScheduling.Implementations;

public class DeviceScheduler : IDeviceScheduler
{
    private readonly DataContext _dbContext;
    private static int firstJobReccuring = 2000;
    private static int secondJobReccuring = 2001;
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

    public async Task<Response> GetActiveJobsForDeviceId(int deviceId)
    {
        var response = new Response();

        response.Success = true;
        response.Data = await _dbContext.DeviceJobs.Where( dj => dj.DeviceId == deviceId && ((dj.Repeat == true) || (dj.Repeat == false && dj.EndDate > DateTime.Now)) && dj.Canceled == false)
                                                   .Select( dj => new
                                                   {
                                                       Id = dj.Id,
                                                       StartDate = dj.StartDate,
                                                       EndDate = dj.EndDate,
                                                       Turn = dj.Turn,
                                                       Repeat = dj.Repeat
                                                   })
                                                   .AsNoTracking()
                                                   .ToListAsync();

        return response;
    }

    public async Task<Response> GetAllJobs(int userId, bool active)
    {
        var response = new Response();

        if (active)
        {
            var activeJobs = await _dbContext.DeviceJobs
                .Where(jobs => (jobs.Repeat || (jobs.EndDate > DateTime.Now && !jobs.Repeat)) && jobs.Canceled == false)
                .Join(_dbContext.Devices,
                      jobs => jobs.DeviceId,
                      devices => devices.Id,
                      (jobs, devices) => new { Job = jobs, Device = devices })
                .Where(j => j.Device.UserId == userId)
                .Select(j => new
                {
                    Id = j.Job.Id,
                    StartDate = j.Job.StartDate,
                    EndDate = j.Job.EndDate,
                    DeviceName = j.Job.Device!.Name,
                    DeviceId = j.Job.DeviceId,
                    Repeat = j.Job.Repeat,
                    Turn = j.Job.Turn,
                    Canceled = j.Job.Canceled
                })
                .OrderBy(j => j.Canceled) // sort by Canceled (false first, then true)
                .ThenByDescending(j => j.Repeat) // sort by Repeat (true first, then false)
                .ThenBy(j => j.StartDate) // sort by StartDate
                .AsNoTracking()
                .ToListAsync();

            if (activeJobs.IsNullOrEmpty())
            {
                response.Errors.Add("There are no active device jobs currently scheduled!");
                response.Success = false;

                return response;
            }
            
            response.Data = activeJobs;
            response.Success = true;

            return response;
        }

        var canceledJobs = await _dbContext.DeviceJobs
            .Where(jobs => jobs.Canceled == true)
            .Join(_dbContext.Devices,
                  jobs => jobs.DeviceId,
                  devices => devices.Id,
                  (jobs, devices) => new { Job = jobs, Device = devices })
            .Where(j => j.Device.UserId == userId)
            .Select(j => new
            {
                Id = j.Job.Id,
                StartDate = j.Job.StartDate,
                EndDate = j.Job.EndDate,
                DeviceName = j.Job.Device!.Name,
                DeviceId = j.Job.DeviceId,
                Repeat = j.Job.Repeat,
                Turn = j.Job.Turn,
                Canceled = j.Job.Canceled
            })
            .OrderBy(j => j.Canceled) // sort by Canceled (false first, then true)
            .ThenByDescending(j => j.Repeat) // sort by Repeat (true first, then false)
            .ThenBy(j => j.StartDate) // sort by StartDate
            .AsNoTracking()
            .ToListAsync();


        if (canceledJobs.IsNullOrEmpty())
        {
            response.Errors.Add("There are no canceled device jobs!");
            response.Success = false;

            return response;
        }

        response.Data = canceledJobs;
        response.Success = true;

        return response;

    }

    public async Task<Response> RemoveReccuringJobForJobId(int jobId)
    {
        var response = new Response();

        var job = await _dbContext.DeviceJobs.FirstOrDefaultAsync(job => job.Id == jobId && job.Repeat == true);

        if (job == null)
        {
            response.Errors.Add("Job for given id does not exist!");
            response.Success = false;

            return response;
        }
        
        RecurringJob.RemoveIfExists(job.StartJobId.ToString());
        RecurringJob.RemoveIfExists(job.EndJobId.ToString());

        job.Canceled = true; //set cancelled as true in order to flag it so we can use it as history for dashboard

        await _dbContext.SaveChangesAsync();
        
        response.Data = "Job removed successfully!";
        response.Success = true;

        return response;
    }

    public async Task<Response> RemoveScheduledJobForJobId(int jobId)
    {
        var response = new Response();

        var scheduledJob = await _dbContext.DeviceJobs.FirstOrDefaultAsync(job => job.Id == jobId && job.Repeat == false);
        
        if (scheduledJob == null)
        {
            response.Errors.Add("Job for given id does not exist!");
            response.Success = false;

            return response;
        }

        var deletedStartJob = BackgroundJob.Delete(scheduledJob.StartJobId.ToString());
        var deletedEndJob = BackgroundJob.Delete(scheduledJob.EndJobId.ToString());
        scheduledJob.Canceled = true;
        await _dbContext.SaveChangesAsync();
        
        if (deletedStartJob && deletedEndJob)
        {
            response.Data = "You have successfully canceled your scheduled job!";
            response.Success = true;

            return response;
        }
        
        if(deletedStartJob == false)
            response.Errors.Add("Error while deleting job for start date");
        if(deletedEndJob == false)
            response.Errors.Add("Error while deleting job for end date");

        response.Success = response.Errors.Count == 0;

        return response;
    }

    public async Task<Response> RemoveJobForId(int jobId)
    {
        var response = new Response();
        var job = await _dbContext.DeviceJobs.FirstOrDefaultAsync(job => job.Id == jobId && job.Canceled == false);

        if (job == null)
        {
            response.Errors.Add("Job for given id does not exist!");
            response.Success = false;

            return response;
        }

        if (job.Repeat)
        {
            RecurringJob.RemoveIfExists(job.StartJobId.ToString());
            RecurringJob.RemoveIfExists(job.EndJobId.ToString());

            job.Canceled = true; //set cancelled as true in order to flag it so we can use it as history for dashboard

            await _dbContext.SaveChangesAsync();

            response.Data = "Job removed successfully!";
            response.Success = true;
        }
        else
        {
            var deletedStartJob = BackgroundJob.Delete(job.StartJobId.ToString());
            var deletedEndJob = BackgroundJob.Delete(job.EndJobId.ToString());
            job.Canceled = true;
            await _dbContext.SaveChangesAsync();

            if (deletedStartJob && deletedEndJob)
            {
                response.Data = "You have successfully canceled your scheduled job!";
                response.Success = true;

                return response;
            }

            if (deletedStartJob == false)
                response.Errors.Add("Error while deleting job for start date");
            if (deletedEndJob == false)
                response.Errors.Add("Error while deleting job for end date");

            response.Success = response.Errors.Count == 0;
        }

        return response;
    }
}