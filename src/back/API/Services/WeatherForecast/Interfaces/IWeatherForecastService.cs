using API.Services.WeatherForecast.Models;

namespace API.Services.WeatherForecast.Interfaces;

public interface IWeatherForecastService
{
    Weather GetCurrentWeather();
    Forecast Get5DayWeatherForecast();
}