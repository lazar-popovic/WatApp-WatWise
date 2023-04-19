using API.BL.Interfaces;
using API.Common.API_Keys;
using API.DAL.Interfaces;
using API.Services.JWTCreation.Interfaces;
using API.Services.WeatherForecast.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services.WeatherForecast.Implementations;

public class WeatherForecastService : IWeatherForecastService
{
    private const string WeatherBaseUrl = "https://api.openweathermap.org/data/2.5/weather?";
    private const string ForecastBaseUrl = "https://api.openweathermap.org/data/2.5/forecast?";
    
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly IHttpContextAccessor _context;
    
    private readonly IJWTCreator _jwtCreator;
    private readonly IUserDAL _userDal;
    
    private const string Units = "metric";
    private double? _lat;
    private double? _lon;

    public WeatherForecastService(IJWTCreator jwtCreator, IUserDAL userDal,IHttpContextAccessor context)
    {
        _context = context;
        _jwtCreator = jwtCreator;
        _userDal = userDal;
        GetUserId(_context);
    }

    private async Task GetUserId(IHttpContextAccessor context)
    {
        int userId = 0;
        string authorizationHeader = context.HttpContext.Request.Headers["Authorization"];

        var jwt = authorizationHeader.Substring("bearer ".Length).Trim();
        try
        {
            userId = _jwtCreator.GetUserIdFromToken(jwt);
        }
        catch(SecurityTokenException se)
        {
            Console.WriteLine("Invalid token!" + se.InnerException?.Message);
        }
        catch(Exception e)
        {
            Console.WriteLine("Invalid token!" + e.Message);;
        }
        
        if(userId == -1)
            Console.WriteLine("Something went wrong with getting user id from token!");
        else
        {
            var user = await _userDal.GetByIdAsync(userId);
            
            if(user == null)
                Console.WriteLine("User with provided id doesent exist in database");
            else
            {
                _lon = user.Location!.Longitude;
                _lat = user.Location!.Latitude;
            }
        }
    }
    
    public async Task<string?> GetCurrentWeatherAsync()
    {
        string responseContent = null;
        try
        {
            var response = await _httpClient.GetAsync($"{WeatherBaseUrl}lat={_lat}&lon={_lon}&appid={WeatherForecastApiKey.Key}&units={Units}");
            response.EnsureSuccessStatusCode();

            responseContent = await response.Content.ReadAsStringAsync();

            /*
            //if (!string.IsNullOrEmpty(responseContent))
                //_weatherData = JsonConvert.DeserializeObject<Weather>(responseContent);
            else
                throw new Exception("Content response is null");
                */
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
        //return _weatherData ?? null;
        return responseContent;
    }

    public async Task<string?> Get5DayWeatherForecastAsync()
    {
        string responseContent = null;
        try
        {
            var response = await _httpClient.GetAsync($"{ForecastBaseUrl}lat={_lat}&lon={_lon}&appid={WeatherForecastApiKey.Key}&units={Units}");
            response.EnsureSuccessStatusCode();

            responseContent = await response.Content.ReadAsStringAsync();
            /*
            if (!string.IsNullOrEmpty(responseContent))
                _forecastData = JsonConvert.DeserializeObject<Forecast>(responseContent);
            else
                throw new Exception("Content from response is NULL!");
                */
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
        //return _forecastData ?? null;
        return responseContent;
    }
}