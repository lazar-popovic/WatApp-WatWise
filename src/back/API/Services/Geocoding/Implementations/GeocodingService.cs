using System.Net;
using System.Text.Json;
using API.Models.ViewModels;
using API.Services.Geocoding.Interfaces;
using Newtonsoft.Json.Linq;

namespace API.Services.Geocoding.Implementations;

public class GeocodingService : IGeocodingService
{
    public static string Url = "https://dev.virtualearth.net/REST/v1/Locations";

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
                foreach (var data in datas)
                {
                    var address = (Dictionary<string, object>)data["address"];

                    result.Add(address);
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
        var address = $"{location.Address} {location.Number}, {location.City}";
        var result = new LongLat();
        var url = $"{Url}?q={address}&key={ApiKey.Key}";
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
}