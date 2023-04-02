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
    private readonly DataContext _dbContext;

    public ExampleDataController(IDeviceSimulatorService simulator, DataContext dbContext)
    {
        _simulator = simulator;
        _dbContext = dbContext;
    }

    [HttpPost("auto-update")]
    [AutomaticRetry(Attempts = 3)]
    public async Task<IActionResult> AutoHourlyUpdate()
    {
        await _simulator.HourlyUpdate();
        return Ok();
    }
        
    [HttpPost("fill-january-first")]
    [AutomaticRetry(Attempts = 3)]
    public async Task<IActionResult> FillDataSinceJanuary1st( int type, int deviceId)
    {
        await _simulator.FillDataSinceJanuary1st( type, deviceId);
        return Ok();
    }
    
    [HttpPost("example")]
    public async Task<IActionResult> Example( int userId)
    {
        var currentDate = DateTime.UtcNow.Date;
        
        var startTimestamp = new DateTimeOffset(currentDate, TimeSpan.Zero);
        
        var endTimestamp = startTimestamp.AddDays(1);
        
        var producingEnergyUsageByTimestamp =  _dbContext.DeviceEnergyUsage
            .Join(_dbContext.Devices, energyUsage => energyUsage.DeviceId, device => device.Id, (energyUsage, device) => new { EnergyUsage = energyUsage, Device = device })
            .Join(_dbContext.DeviceTypes, joined => joined.Device.DeviceTypeId, type => type.Id, (joined, type) => new { EnergyUsage = joined.EnergyUsage, Device = joined.Device, Category = type.Category!.Value })
            .Where(joined => joined.EnergyUsage.Timestamp!.Value.Date == DateTime.Now.Date && joined.Device.UserId == userId && joined.Category == 1)
            .GroupBy(joined => new { Timestamp = joined.EnergyUsage.Timestamp!.Value})
            .Select(grouped => new { Timestamp = grouped.Key.Timestamp, TotalEnergyUsage = grouped.Sum(joined => joined.EnergyUsage.Value) })
            .ToList();

        var consumingEnergyUsageByTimestamp = _dbContext.DeviceEnergyUsage
            .Join(_dbContext.Devices, energyUsage => energyUsage.DeviceId, device => device.Id, (energyUsage, device) => new { EnergyUsage = energyUsage, Device = device })
            .Join(_dbContext.DeviceTypes, joined => joined.Device.DeviceTypeId, type => type.Id, (joined, type) => new { EnergyUsage = joined.EnergyUsage, Device = joined.Device, Category = type.Category.Value })
            .Where(joined => joined.EnergyUsage.Timestamp.Value.Date == DateTime.Now.Date && joined.Device.UserId == userId && joined.Category == -1)
            .GroupBy(joined => new { Timestamp = joined.EnergyUsage.Timestamp.Value})
            .Select(grouped => new { Timestamp = grouped.Key.Timestamp, TotalEnergyUsage = Math.Abs(grouped.Sum(joined => joined.EnergyUsage.Value.Value)) })
            .ToList();

        var response = new { producingEnergyUsageByTimestamp, consumingEnergyUsageByTimestamp};
        return Ok( response);
    }
}

