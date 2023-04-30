﻿
using API.DAL.Interfaces;
using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.Geocoding;
using Microsoft.EntityFrameworkCore;

namespace API.DAL.Implementations;

public class LocationDAL : ILocationDAL
{
    private readonly DataContext _context;

    public LocationDAL(DataContext context)
    {
        _context = context;
    }
    
    public int GetLocationByLatLongAsync(LongLat request)
    {
        return _context.Locations.Where(l => l.Longitude == request.Longitude && l.Latitude == request.Latitude)
            .Select(l => l.Id).FirstOrDefault();
    }

    public int InsertLocation(LocationViewModel model, LongLat cords)
    {
        var location = new Location
        {
            Latitude = cords.Latitude,
            Longitude = cords.Longitude,
            Address = model.Address,
            City = model.City,
            Neighborhood = model.Neighborhood,
            AddressNumber = model.Number
        };
        _context.Locations.Add( location);
        _context.SaveChanges();

        return location.Id;
    }

    public async Task<List<Location>> GetAllLocationsAsync()
    {
        return await _context.Locations.Where( l => l.Users.Count( u => u.Verified == true) > 0).ToListAsync();
    }
    public async Task<List<String>> GetAllLocationsCity()
    {
        var cities = _context.Locations
             .Select(l => l.City)
             .Distinct()
             .ToList();
        return cities;
    }
    public async Task<List<String>> GetAllNeighborhood(string city)
    {
        var neighborhoods = _context.Locations
           .Where(l => l.City.Equals(city, StringComparison.OrdinalIgnoreCase))
           .Select(l => l.Neighborhood)
           .Distinct()
           .ToList();
        return neighborhoods;
    }
    public async Task<List<Location>> GetAllLocationWithNeighborhood(string neighborhood)
    {
        var locations = _context.Locations
            .Where(l => l.Neighborhood.Equals(neighborhood, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return locations;
    }
}