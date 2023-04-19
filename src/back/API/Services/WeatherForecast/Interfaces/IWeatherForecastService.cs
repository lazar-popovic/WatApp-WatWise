using API.Services.WeatherForecast.Models;

namespace API.Services.WeatherForecast.Interfaces;

public interface IWeatherForecastService
{
    Task<Weather?> GetCurrentWeatherAsync();
    Task<Forecast?> Get5DayWeatherForecastAsync();
}