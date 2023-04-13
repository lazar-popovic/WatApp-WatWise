using System.Net;
using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.Geocoding.Interfaces;
using Newtonsoft.Json.Linq;

namespace API.Services.Geocoding.Implementations;

public class GeocodingService : IGeocodingService
{
    public static string Url = "https://dev.virtualearth.net/REST/v1/Locations";


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