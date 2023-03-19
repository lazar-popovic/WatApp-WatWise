using API.Models.Dto;
using API.Services.DeviceSimulatorService.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Services.DeviceSimulatorService.Implementation;

public class DeviceSimulatorService : IDeviceSimulatorService
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _devices;

    public DeviceSimulatorService()
    {
        _client = new MongoClient("mongodb://localhost:27017");
        _devices = _client.GetDatabase("devices");
    }
    
    public async Task<List<ElectricalUsage>> GetUsageForDeviceBetweenDates(string device, DateTime startingDate, DateTime endingDate)
    {
        var collection = _devices.GetCollection<BsonDocument>(device);
        var filterBuilder = Builders<BsonDocument>.Filter;
        var query = filterBuilder.And(
            filterBuilder.Gte("timestamp", startingDate),
            filterBuilder.Lt("timestamp", endingDate)
        );
        var consumptionData = collection.Find(query).ToList();
        var usageList = new List<ElectricalUsage>();
        foreach (BsonDocument doc in consumptionData)
        {
            DateTime timestamp = doc["timestamp"].ToUniversalTime();
            double value = doc["value"].ToDouble();
            var usage = new ElectricalUsage { Timestamp = timestamp, Value = value };
            usageList.Add(usage);
        }
        return usageList;
    }
}