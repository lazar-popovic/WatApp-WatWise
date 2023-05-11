using API.Common;
using API.Models;
using API.Models.Entity;
using API.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Services.DeviceScheduling.Interfaces;

public interface IDeviceScheduler
{
    Task ExecuteJob(int? deviceId, bool? turn);
    Task ScheduleJob(DeviceJobViewModel request);
    Task<Response> GetActiveJobsForDeviceId(int deviceId);
    Task<Response> GetAllJobs(int userId, bool active);
    Task<Response> RemoveReccuringJobForJobId(int jobId);
    Task<Response> RemoveScheduledJobForJobId(int jobId);
    Task<Response> RemoveJobForId(int jobId);
}