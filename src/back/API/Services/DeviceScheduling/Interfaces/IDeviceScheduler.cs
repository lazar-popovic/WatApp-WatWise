﻿using API.Common;
using API.Models;
using API.Models.Entity;
using API.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Services.DeviceScheduling.Interfaces;

public interface IDeviceScheduler
{
    Task ExecuteJob(int? deviceId, bool? turn);
    Task ScheduleJob(DeviceJobViewModel request);
    Task<Response> GetActiveJobForDeviceId(int deviceId);
    Task<Response<List<DeviceJob>>> GetAllReccuringJobs(bool active);
    Task<Response> RemoveReccuringJobForJobId(int jobId);
    Task<Response> RemoveScheduledJobForJobId(int jobId);
}