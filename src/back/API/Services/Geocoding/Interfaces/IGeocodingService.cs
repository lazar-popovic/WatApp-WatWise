using API.Models.ViewModels;

namespace API.Services.Geocoding.Interfaces;

public interface IGeocodingService
{
    LongLat Geocode( LocationViewModel location);
}