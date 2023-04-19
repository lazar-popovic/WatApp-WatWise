using API.Common.API_Keys;
using API.Services.WeatherForecast.Interfaces;
using API.Services.WeatherForecast.Models;
using Newtonsoft.Json;

namespace API.Services.WeatherForecast.Implementations;

public class WeatherForecastService : IWeatherForecastService
{
    private const string WeatherBaseUrl = "https://api.openweathermap.org/data/2.5/weather?";
    private const string ForecastBaseUrl = "https://api.openweathermap.org/data/2.5/forecast?";
    private readonly HttpClient _httpClient;
    private const string Units = "metric";
    private double _lat;
    private double _lon;
    private Weather? _weatherData;
    private Forecast? _forecastData;
    

    public WeatherForecastService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Weather?> GetCurrentWeatherAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{WeatherBaseUrl}lat={_lat}&lon={_lon}&appid={WeatherForecastApiKey.Key}&units={Units}");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(responseContent))
                _weatherData = JsonConvert.DeserializeObject<Weather>(responseContent);
            else
                throw new Exception("Content response is null");
        }
        catch (HttpRequestException httpRequestException)
        {
            Console.WriteLine(httpRequestException.Message);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return _weatherData ?? null;
    }

    public async Task<Forecast?> Get5DayWeatherForecastAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ForecastBaseUrl}lat={_lat}&lon={_lon}&appid={WeatherForecastApiKey.Key}&units={Units}");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(responseContent))
                _forecastData = JsonConvert.DeserializeObject<Forecast>(responseContent);
            else
                throw new Exception("Content from response is NULL!");
        }
        catch (HttpRequestException httpRequestException)
        {
            Console.WriteLine(httpRequestException.Message);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return _forecastData ?? null;
    }
}