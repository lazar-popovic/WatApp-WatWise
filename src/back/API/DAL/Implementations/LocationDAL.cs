
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
        var now = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

        var locationDTOs = await _context.Locations
            .Join(_context.Users, l => l.Id, u => u.LocationId, (l, u) => new { Location = l, User = u })
            .Join(_context.Devices, lu => lu.User.Id, d => d.UserId, (lu, d) => new { lu.Location, Device = d })
            .Where(ld => ld.Device.ActivityStatus == true && ld.Device.DataShare == true && ld.Device.DeviceEnergyUsages.Any(usage => usage.Timestamp == now))
            .Select(ld => new {
                ld.Location.Id,
                ld.Location.Address,
                ld.Location.AddressNumber,
                ld.Location.City,
                ld.Location.Latitude,
                ld.Location.Longitude,
                ld.Location.Neighborhood,
                PowerUsage = ld.Device.DeviceType.Category * ld.Device.DeviceEnergyUsages.Where(usage => usage.Timestamp == now).Sum(usage => usage.Value)
            })
            .GroupBy(l => l.Id)
            .Select(g => new LocationWithPowerUsageDTO {
                Id = g.Key,
                Address = g.First().Address,
                AddressNumber = g.First().AddressNumber,
                City = g.First().City,
                Latitude = g.First().Latitude,
                Longitude = g.First().Longitude,
                Neighborhood = g.First().Neighborhood,
                TotalPowerUsage = g.Sum(x => x.PowerUsage)
            })
            .AsNoTracking().ToListAsync();

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