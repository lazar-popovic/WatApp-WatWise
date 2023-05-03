
using API.DAL.Interfaces;
using API.Models.DTOs;
using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.Geocoding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

    public async Task<List<LocationWithPowerUsageDTO>> GetAllLocationsAsync()
    {
        
        var locations = await _context.Locations.Include(l => l.Users)
                                                .ThenInclude(u => u.Devices)
                                                .ThenInclude(d => d.DeviceEnergyUsages)
                                            .Include(l => l.Users)
                                                .ThenInclude(u => u.Devices)
                                                .ThenInclude(d => d.DeviceType)
                                                .Where(l => l.Users.Any(u => u.Verified == true))
                                            .ToListAsync(); 


        var locationDTOs = new List<LocationWithPowerUsageDTO>();
        var now = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
        foreach (var location in locations)
        {
            double? totalPowerUsage = 0.0;
            if (location.Users != null && location.Users.Any())
            {
                foreach (var user in location.Users)
                {
                    if (user.Devices != null && user.Devices.Any())
                    {
                        foreach (var device in user.Devices)
                        {
                            if (device.ActivityStatus == true && device.DataShare == true)
                            {
                                var energyUsage = await _context.DeviceEnergyUsage
                                    .Where(d => d.DeviceId == device.Id && d.Timestamp == now)
                                    .SumAsync(d => d.Value);
                                totalPowerUsage += device.DeviceType.Category * energyUsage;
                            }
                        }
                    }
                }
            }

            var locationDTO = new LocationWithPowerUsageDTO()
            {
                Id = location.Id,
                Address = location.Address,
                AddressNumber = location.AddressNumber,
                City = location.City,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Neighborhood = location.Neighborhood,
                TotalPowerUsage = totalPowerUsage
            };
            locationDTOs.Add(locationDTO);
        }

        return locationDTOs;
    }
    
    public async Task<List<string?>?> GetAllLocationsCity()
    {
        var cities = await _context.Locations
             .Select(l => l.City)
             .Distinct()
             .ToListAsync();
        
        return cities;

    }
    public async Task<List<String>> GetAllNeighborhood(string city)
    {
        var neighborhoods = await _context.Locations
           .Where(l => l.City.ToLower() == city.ToLower())
           .Select(l => l.Neighborhood)
           .Distinct()
           .ToListAsync();
        return neighborhoods;
    }
    public async Task<List<Location>> GetAllLocationWithNeighborhood(string neighborhood)
    {
        var locations = await _context.Locations
            .Where(l => l.Neighborhood.ToLower() == neighborhood.ToLower())
            .ToListAsync();

        return locations;
    }
}