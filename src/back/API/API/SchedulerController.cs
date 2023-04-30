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
        return Ok(await _deviceScheduler.ScheduleJob(request));
    }
}