using API.Models.ViewModels;
using API.Services.DeviceScheduling.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.API;

[ApiController]
[Route("api/scheduler")]
public class SchedulerController : ControllerBase
{
    private readonly IDeviceScheduler _deviceScheduler;

    public SchedulerController(IDeviceScheduler deviceScheduler)
    {
        _deviceScheduler = deviceScheduler;
    }

    [HttpPost("device-job")]
    public async Task<IActionResult> ScheduleDeviceActivity(DeviceJobViewModel request)
    {
        await _deviceScheduler.ScheduleJob(request);
        return Ok();
    }

    [HttpGet("get-active-job-for-device-id")]
    public async Task<IActionResult> GetActiveJobForDeviceId( int deviceId)
    {
        return Ok( await _deviceScheduler.GetActiveJobsForDeviceId( deviceId));
    }

    [HttpPost("cancel-reccuring-job")]
    public async Task<IActionResult> CancelReccuringJob(int jobId)
    {
        return Ok(await _deviceScheduler.RemoveReccuringJobForJobId(jobId));
    }

    [HttpPost("get-all-reccuring-jobs")]
    public async Task<IActionResult> GetAllReccuringJobs(bool active)
    {
        return Ok(await _deviceScheduler.GetAllReccuringJobs(active));
    }

    [HttpPost("cancel-scheduled-job")]
    public async Task<IActionResult> CancelScheduledJob(int jobId)
    {
        return Ok(await _deviceScheduler.RemoveScheduledJobForJobId(jobId));
    }
}