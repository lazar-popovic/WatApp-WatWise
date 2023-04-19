namespace API.Services.WeatherForecast.Models;

public class Forecast
{
    public DateTime Date { get; set; }
    public string? Day { get; set; }
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public double? Temperature { get; set; }
    public double? MinTemperature { get; set; }
    public double? MaxTemperature { get; set; }
    public double? Humidity { get; set; }
    public double? WindSpeed { get; set; }
}