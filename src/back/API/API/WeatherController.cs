using System.Security.Claims;
using API.Services.WeatherForecast.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.API;

[Route("api/weather")]
[ApiController,Authorize(Roles = "User,Employee")]

public class WeatherController : ControllerBase
{
    private readonly IWeatherForecastService _weatherForecastService;
    private readonly IHttpContextAccessor _contextAccessor;
    public WeatherController(IWeatherForecastService weatherForecastService, IHttpContextAccessor contextAccessor)
    {
        _weatherForecastService = weatherForecastService;
        _contextAccessor = contextAccessor;
    }
    
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentWeather()
    {
        return Ok(await _weatherForecastService.GetCurrentWeatherAsync());
    }

    [HttpGet("forecast")]
    public async Task<IActionResult> Get5DayForecast()
    {
        return Ok(await _weatherForecastService.Get5DayWeatherForecastAsync());
    }
}