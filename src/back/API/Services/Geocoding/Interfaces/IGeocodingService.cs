namespace API.Services.Geocoding.Interfaces;

public interface IGeocodingService
{
    LongLat Geocode(string address);
}