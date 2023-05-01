﻿using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.Geocoding;

namespace API.DAL.Interfaces;

public interface ILocationDAL
{
    int GetLocationByLatLongAsync( LongLat request);
    int InsertLocation(LocationViewModel model, LongLat cords);
    Task<List<Location>> GetAllLocationsAsync();
    Task<List<string?>?> GetAllLocationsCity();
    Task<List<String>> GetAllNeighborhood(string city);
    Task<List<Location>> GetAllLocationWithNeighborhood(string neighborhood);
}