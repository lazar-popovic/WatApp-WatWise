using API.Services.DeviceSimulatorService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.API;

[ApiController]
[Route("api/[controller]")]
public class ExampleDataController : ControllerBase
{
    private readonly IDeviceSimulatorService _simulator;

    public ExampleDataController(IDeviceSimulatorService simulator)
    {
        _simulator = simulator;
    }

    [HttpGet]
    [Route("device-between-dates")]
    public async Task<IActionResult> GetUsageForDeviceBetweenDates(string device, DateTime startingDate, DateTime endingDate)
    {
        await _simulator.HourlyUpdate();
        return Ok(await _simulator.GetUsageForDeviceBetweenDates(device, startingDate, endingDate));
    }
}