
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
        return _context.Locations
            .Where(l => Equals(l.Longitude, request.Longitude) && Equals(l.Latitude, request.Latitude))
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
        _context.Locations.Add(location);
        _context.SaveChanges();

        return location.Id;
    }

    public async Task<List<LocationWithPowerUsageDTO>> GetAllLocationsAsync()
    {
        var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

        var locationDTOs = await _context.Locations
            .GroupJoin(_context.Users, l => l.Id, u => u.LocationId, (l, users) => new { Location = l, Users = users })
            .SelectMany(lu => lu.Users.DefaultIfEmpty(), (lu, user) => new { lu.Location, User = user })
            .GroupJoin(_context.Devices, lu => lu.User.Id, d => d.UserId,
                (lu, devices) => new { lu.Location, lu.User, Devices = devices })
            .SelectMany(ld => ld.Devices.DefaultIfEmpty(),
                (ld, device) => new { ld.Location, ld.User, Device = device })
            .Where(ld => ld.Device == null || (ld.Device.ActivityStatus == true && ld.Device.DataShare == true &&
                                               ld.Device.DeviceEnergyUsages.Any(usage => usage.Timestamp == now)))
            .Select(ld => new
            {
                ld.Location.Id,
                ld.Location.Address,
                ld.Location.AddressNumber,
                ld.Location.City,
                ld.Location.Latitude,
                ld.Location.Longitude,
                ld.Location.Neighborhood,
                PowerUsage = ld.Device == null
                    ? 0.0
                    : ld.Device.DeviceType.Category * ld.Device.DeviceEnergyUsages
                        .Where(usage => usage.Timestamp == now).Sum(usage => usage.Value)
            })
            .GroupBy(l => l.Id)
            .Select(g => new LocationWithPowerUsageDTO
            {
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

    public async Task<List<LocationWithPowerUsageDTO>> GetAllLocationWithNeighborhood(string city, string neighborhood)
    {
        var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

        var locationDTOs = await _context.Locations
            .GroupJoin(_context.Users, l => l.Id, u => u.LocationId, (l, users) => new { Location = l, Users = users })
            .SelectMany(lu => lu.Users.DefaultIfEmpty(), (lu, user) => new { lu.Location, User = user })
            .GroupJoin(_context.Devices, lu => lu.User.Id, d => d.UserId,
                (lu, devices) => new { lu.Location, lu.User, Devices = devices })
            .SelectMany(ld => ld.Devices.DefaultIfEmpty(),
                (ld, device) => new { ld.Location, ld.User, Device = device })
            .Where(ld =>
                (ld.Device == null || (ld.Device.ActivityStatus == true && ld.Device.DataShare == true &&
                                       ld.Device.DeviceEnergyUsages.Any(usage => usage.Timestamp == now))) &&
                ((ld.Location.Neighborhood.ToLower() == neighborhood.ToLower() || neighborhood == "All" ||
                  neighborhood == "all") && ld.Location.City.ToLower() == city.ToLower()))
            .Select(ld => new
            {
                ld.Location.Id,
                ld.Location.Address,
                ld.Location.AddressNumber,
                ld.Location.City,
                ld.Location.Latitude,
                ld.Location.Longitude,
                ld.Location.Neighborhood,
                PowerUsage = ld.Device == null
                    ? 0.0
                    : ld.Device.DeviceType.Category * ld.Device.DeviceEnergyUsages
                        .Where(usage => usage.Timestamp == now).Sum(usage => usage.Value)
            })
            .GroupBy(l => l.Id)
            .Select(g => new LocationWithPowerUsageDTO
            {
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

    public async Task<List<NeighborhoodPowerUsageDTO>> Top5NeighborhoodsForCityPowerUsageAndPredictedPowerUsage(string city, int category)
    {
        var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

        var query = from loc in _context.Locations
                    join usr in _context.Users on loc.Id equals usr.LocationId into userGroup
                    from usr in userGroup.DefaultIfEmpty()
                    join dev in _context.Devices on usr.Id equals dev.UserId into deviceGroup
                    from dev in deviceGroup.DefaultIfEmpty()
                    join devType in _context.DeviceTypes on dev.DeviceTypeId equals devType.Id into deviceTypeGroup
                    from devType in deviceTypeGroup.DefaultIfEmpty()
                    join energy in _context.DeviceEnergyUsage on dev.Id equals energy.DeviceId into energyGroup
                    from energy in energyGroup.DefaultIfEmpty()
                    where loc.City == city &&
                          (devType == null || devType.Category == category) &&
                          (dev == null || dev.DataShare == true) &&
                          (energy == null || energy.Timestamp == now)
                    group new { energy.Value, energy.PredictedValue } by new { loc.Neighborhood } into g
                    select new NeighborhoodPowerUsageDTO
                    {
                        Neighborhood = g.Key.Neighborhood,
                        PowerUsage = g.Sum(e => e.Value),
                        PredictedPowerUsage = g.Sum(e => e.PredictedValue)
                    };

        var neighborhoodDTOs = await query.OrderByDescending(n => n.PowerUsage)
                                          .Take(5)
                                          .AsNoTracking()
                                          .ToListAsync();

        return neighborhoodDTOs;
    }
}