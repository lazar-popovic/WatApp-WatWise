using API.Common;
using API.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Services.DeviceScheduling.Interfaces;

public interface IDeviceScheduler
{
    Task ExecuteJob(int? deviceId, bool? turn);
    Task ScheduleJob(DeviceJobViewModel request);
    Task<Response> GetActiveJobForDeviceId(int deviceId);
    Task<Response> RemoveReccuringJobForJobId(int jobId);
}