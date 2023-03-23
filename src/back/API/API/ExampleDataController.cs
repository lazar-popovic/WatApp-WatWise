using API.Services.DeviceSimulatorService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Hangfire;
using API.Models.Entity;
using Microsoft.IdentityModel.Tokens;

namespace API.API;

[ApiController]
[Route("api/[controller]")]
public class ExampleDataController : ControllerBase
{
    private readonly IDeviceSimulatorService _simulator;
    //private readonly IRecurringJobManager _recurringJobManager;
    public ExampleDataController(IDeviceSimulatorService simulator)
    {
        _simulator = simulator;
    }

    [HttpGet]
    [Route("device-between-dates")]
    public async Task<IActionResult> GetUsageForDeviceBetweenDates(string device, DateTime startingDate, DateTime endingDate)
    {
        //await _simulator.HourlyUpdate();
        return Ok(await _simulator.GetUsageForDeviceBetweenDates(device, startingDate, endingDate));
    }

    [HttpPost("auto-update")]
    [AutomaticRetry(Attempts = 3)]
    public async Task<IActionResult> AutoHourlyUpdate()
    {
        RecurringJob.AddOrUpdate<IDeviceSimulatorService>(x => x.HourlyUpdate(), Cron.Hourly);
        //await _simulator.HourlyUpdate();
        return Ok();
    }
}

