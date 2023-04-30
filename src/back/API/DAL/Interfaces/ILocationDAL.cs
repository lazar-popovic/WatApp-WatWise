using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.Geocoding;

namespace API.DAL.Interfaces;

public interface ILocationDAL
{
    int GetLocationByLatLongAsync( LongLat request);
    int InsertLocation(LocationViewModel model, LongLat cords);
    Task<List<Location>> GetAllLocationsAsync();
    Task<List<String>> GetAllLocationsCity();
    Task<List<String>> GetAllNeighborhood(string city);
}