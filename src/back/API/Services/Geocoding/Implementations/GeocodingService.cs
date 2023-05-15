using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using API.Common.API_Keys;
using API.Models.ViewModels;
using API.Services.Geocoding.Interfaces;
using Newtonsoft.Json.Linq;

namespace API.Services.Geocoding.Implementations;

public class GeocodingService : IGeocodingService
{
    private static string Url = "https://dev.virtualearth.net/REST/v1/Locations";
    private string BaseYandexUrl = "https://geocode-maps.yandex.ru/1.x/?";
    private readonly HttpClient _httpClient;

    public GeocodingService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<object> Autocomplete(string? query)
    {
        var requestUrl = $"https://nominatim.openstreetmap.org/search?q={query}&format=jsonv2&addressdetails=1&limit=5";
        Console.WriteLine(requestUrl);
        var request = (HttpWebRequest)WebRequest.Create(requestUrl);
        request.Method = "GET";
        request.UserAgent = "My Application"; // Replace with your application's user agent

        try
        {
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var reader = new StreamReader(responseStream))
            {
                var content = reader.ReadToEnd();
                var datas = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(content);

                var result = new List<object>();
                foreach (var data in datas!)
                {
                    var address = data["address"];
                    var addressDetails = data["display_name"];
                    result.Add( new { addressDetails, address });
                }

                return result;
            }
        }
        catch (WebException ex)
        {
            throw new Exception($"Failed to get coordinates for '{query}': {ex.Message}");
        }
    }

    public LongLat Geocode( LocationViewModel location)
    {
        var address = $"{location.Address} {location.Number}, {location.City}, Serbia";
        var result = new LongLat();
        var url = $"{Url}?q={address}&key={ApiKeys.BingKey}";
        try
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            using var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using var reader = new StreamReader(response.GetResponseStream());
                var responseText = reader.ReadToEnd();
                var responseJson = JObject.Parse(responseText);

                var resourceSet = responseJson!["resourceSets"]![0];
                var resource = resourceSet!["resources"]![0]!;
                var point = resource["point"];
                var latitude = (double)point!["coordinates"]![0]!;
                var longitude = (double)point!["coordinates"]![1]!;

                result.Latitude = latitude;
                result.Longitude = longitude;
            }
        }
        catch
        {
            // Handle errors here
        }

        return result;
    }

    public async Task<string> GeoCodeYandex(LocationViewModel request)
    {
        string? responseContent = null;
        LongLat longlat = new LongLat();
        try
        {
            var response =
                await _httpClient.GetAsync(
                    $"{BaseYandexUrl}apikey={ApiKeys.YandexKey}&geocode={request.Address}+{request.Number}, +{request.City},+Serbia&lang=en_US&format=json");
            response.EnsureSuccessStatusCode();

            responseContent = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException requestException)
        {
            Console.WriteLine("HttpRequest failed and didn't return 200!" + requestException.Message + requestException.StatusCode);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
            throw;
        }

        //longlat.Longitude = responseContent.;
        return responseContent;
    }
}