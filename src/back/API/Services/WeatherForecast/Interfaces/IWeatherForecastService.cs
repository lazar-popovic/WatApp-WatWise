using API.Services.WeatherForecast.ViewModels;

namespace API.Services.WeatherForecast.Interfaces;

public interface IWeatherForecastService
{
    Weather GetCurrentWeather();
}