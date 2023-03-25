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
            AddressNumber = model.Number
        };
        _context.Locations.Add( location);
        _context.SaveChanges();

        return location.Id;
    }

    public async Task<List<Location>> GetAllLocationsAsync()
    {
        return await _context.Locations.ToListAsync();
    }
}