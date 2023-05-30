using API.Models.DTOs;
using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.Geocoding;

namespace API.DAL.Interfaces;

public interface ILocationDAL
{
    int GetLocationByLatLongAsync( LongLat request);
    int InsertLocation(LocationViewModel model, LongLat cords);
    Task<List<LocationWithPowerUsageDTO>> GetAllLocationsAsync();
    Task<List<string?>?> GetAllLocationsCity();
    Task<List<String>> GetAllNeighborhood(string city);
    Task<List<LocationWithPowerUsageDTO>> GetAllLocationWithNeighborhood(string city, string neighborhood, int category);
    Task<List<NeighborhoodPowerUsageDTO>> Top5NeighborhoodsForCityPowerUsageAndPredictedPowerUsage(string city, int category);
}