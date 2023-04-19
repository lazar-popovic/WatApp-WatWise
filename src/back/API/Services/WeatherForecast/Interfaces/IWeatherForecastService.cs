namespace API.Services.WeatherForecast.Interfaces;

public interface IWeatherForecastService
{
    Task<string?> GetCurrentWeatherAsync();
    Task<string?> Get5DayWeatherForecastAsync();
}