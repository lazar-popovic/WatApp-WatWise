using API.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace API.API;

[ApiController]
[Route("api/scheduler")]
public class SchedulerController : ControllerBase
{
    private readonly IDeviceScheduling _deviceScheduling;

    public SchedulerController(IDeviceScheduling deviceScheduling)
    {
        _deviceScheduling = deviceScheduling;
    }

    [HttpPost("device-job")]
    public async Task<IActionResult> ScheduleDeviceActivity(DeviceJobViewModel request)
    {
        return Ok(await _deviceScheduling.ScheduleDeviceJob(request));
    }
}