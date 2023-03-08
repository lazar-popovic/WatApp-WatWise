using System.Net;
using Newtonsoft.Json.Linq;

namespace API.Services.Geocoding;

public class GeocodingService
{
    public static string Url = "https://dev.virtualearth.net/REST/v1/Locations";
    
    public LongLat Geocode(string address)
    {
        LongLat result = new LongLat();
        string url = $"{Url}?q={address}&key={ApiKey.Key}";

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseText = reader.ReadToEnd();
                        JObject responseJson = JObject.Parse(responseText);

                        JToken resourceSet = responseJson["resourceSets"][0];
                        JToken resource = resourceSet["resources"][0];
                        JToken point = resource["point"];
                        double latitude = (double)point["coordinates"][0];
                        double longitude = (double)point["coordinates"][1];

                        result.Latitude = latitude;
                        result.Longitude = longitude;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle errors here
        }

        return result;
    }
}