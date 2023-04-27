using API.Models.ViewModels;

namespace API.Services.Geocoding.Interfaces;

public interface IGeocodingService
{
    LongLat Geocode( LocationViewModel location);
    Task<string> GeoCodeYandex(LocationViewModel request);
    Task<object> Autocomplete(string? StreetAddress);
}