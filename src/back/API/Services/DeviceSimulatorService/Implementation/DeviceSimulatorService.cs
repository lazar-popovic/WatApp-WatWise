using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.DeviceSimulatorService.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Services.DeviceSimulatorService.Implementation;

public class DeviceSimulatorService : IDeviceSimulatorService
{
    private readonly MongoClient _client;
    private readonly DataContext _context;
    private readonly IMongoDatabase _devices;

    public DeviceSimulatorService( DataContext context)
    {
        _context = context;
        _client = new MongoClient("mongodb://localhost:27017");
        _devices = _client.GetDatabase("devices");
    }
    
    public async Task<List<ElectricalUsageViewModel>> GetUsageForDeviceBetweenDates(string device, DateTime startingDate, DateTime endingDate)
    {
        var collection = _devices.GetCollection<BsonDocument>(device);
        var filterBuilder = Builders<BsonDocument>.Filter;
        var query = filterBuilder.And(
            filterBuilder.Gte("timestamp", startingDate),
            filterBuilder.Lt("timestamp", endingDate)
        );
        var consumptionData = await collection.Find(query).ToListAsync();
        var usageList = new List<ElectricalUsageViewModel>();
        foreach (BsonDocument doc in consumptionData)
        {
            DateTime timestamp = doc["timestamp"].ToUniversalTime();
            double value = doc["value"].ToDouble();
            var usage = new ElectricalUsageViewModel { Timestamp = timestamp, Value = value };
            usageList.Add(usage);
        }
        return usageList;
    }

    public async Task HourlyUpdate()
    {
        var timestamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

        var types = await _context.Devices.Select(d => d.Type).Distinct().ToListAsync();

        foreach (var type in types)
        {
            await UpdateForType(type, timestamp);
        }
        await _context.SaveChangesAsync();
    }

    public async Task UpdateForType(string type, DateTime timestamp)
    {
        var collection = _devices.GetCollection<BsonDocument>(type);
        var filter = Builders<BsonDocument>.Filter.Eq("timestamp", timestamp);
        var result = await collection.Find(filter).FirstOrDefaultAsync();
        var dvcs = await _context.Devices.Where(d => d.Type == type).Select(d => new { d.Id, d.ActivityStatus}).ToListAsync();
        Console.WriteLine(result);
        var value = result["value"].ToDouble();
        foreach (var dvc in dvcs)
        {
            var insertValue = 0.0;
            if (dvc.ActivityStatus == true)
            {
                insertValue = value;
            }
            await _context.DeviceEnergyUsage.AddAsync(new DeviceEnergyUsage { DeviceId = dvc.Id, Value = insertValue, Timestamp = timestamp });
        }
    }
}