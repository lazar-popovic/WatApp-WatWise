using API.Services.WeatherForecast.Interfaces;
using API.Services.WeatherForecast.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.API;

[Route("api/weather")]
[ApiController,Authorize(Roles = "User,Employee")]

public class WeatherController : ControllerBase
{
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherController(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
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