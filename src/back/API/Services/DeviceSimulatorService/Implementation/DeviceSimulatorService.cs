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
    private readonly IMongoCollection<BsonDocument> _collection;

    public DeviceSimulatorService(DataContext context)
    {
        _context = context;
        _client = new MongoClient("mongodb://localhost:10087");
        _devices = _client.GetDatabase("database");
        _collection = _devices.GetCollection<BsonDocument>("devices");
    }

    public async Task HourlyUpdate()
    {
        var pom = DateTime.Now.AddDays(7);
        var timestamp = new DateTime(pom.Year, pom.Month, pom.Day, pom.Hour, 0, 0, 0);
        
        var devices = await _context.Devices
            .GroupBy(d => d.DeviceTypeId)
            .Select(g => new { DeviceTypeId = g.Key, Devices = g.Select(d => new { d.Id, d.ActivityStatus }).ToList() })
            .AsNoTracking()
            .ToListAsync();

        var deviceEnergyUsageList = new List<DeviceEnergyUsage>();

        foreach (var deviceType in devices)
        {
            var rand = new Random();
            var filter = Builders<BsonDocument>.Filter.Eq("type", deviceType.DeviceTypeId);
            var result = await _collection.Find(filter).FirstOrDefaultAsync();

            if (result != null)
            {
                var usageList = result["usage"].AsBsonArray;
                var usageData =
                    usageList.FirstOrDefault(u => u["timestamp"].ToUniversalTime() == timestamp.ToUniversalTime());
                var value = usageData?["value"].ToDouble();

                foreach (var device in deviceType.Devices)
                {
                    deviceEnergyUsageList.Add(new DeviceEnergyUsage
                        { DeviceId = device.Id,
                          Value = Math.Round(value!.Value * (1 + rand.NextDouble() * 0.4 - 0.2), 3),
                          PredictedValue = Math.Round((value!.Value+0.01) * (1 + rand.NextDouble() * 0.6 - 0.3), 3), 
                          Timestamp = timestamp });
                }
            }
        }

        await _context.DeviceEnergyUsage.AddRangeAsync(deviceEnergyUsageList);
        await _context.SaveChangesAsync();
        await SetCurrentDataTo0IfOff();
    }

    public async Task FillDataSinceJanuary1st(int type, int deviceId)
    {
        var rand = new Random();
        
        var startingDate = new DateTime(2023, 1, 1, 0, 0, 0, 0);
        var pom = DateTime.Now.AddDays(7);
        var endingDate = new DateTime(pom.Year, pom.Month, pom.Day, pom.Hour, 0, 0, 0);

        var filter = Builders<BsonDocument>.Filter.Eq("type", type);
        var result = await _collection.Find(filter).FirstOrDefaultAsync();
        var deviceEnergyUsageList = new List<DeviceEnergyUsage>();
        
        var usageList = result["usage"].AsBsonArray;
        var usageDatas =
            usageList.Where(u => u["timestamp"].ToUniversalTime() >= startingDate && u["timestamp"].ToUniversalTime() <= endingDate).ToList();

        foreach (var usageData in usageDatas)
        {
            deviceEnergyUsageList.Add( new DeviceEnergyUsage{ DeviceId = deviceId, Value = Math.Round(usageData["value"].ToDouble() * (1 + rand.NextDouble() * 0.4 - 0.2), 3), Timestamp = usageData["timestamp"].ToUniversalTime(), PredictedValue = usageData["value"].ToDouble()});
        }
        
        await _context.DeviceEnergyUsage.AddRangeAsync(deviceEnergyUsageList);
        await _context.SaveChangesAsync();
    }

    public async Task SetCurrentDataTo0IfOff()
    {
        var now = DateTime.Now;
        var hourStart = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, 0);
        var rand = new Random();

        var usages = await _context.DeviceEnergyUsage
            .Where(u => u.Timestamp == hourStart)
            .ToListAsync();

        foreach (var usage in usages)
        {
            var device = await _context.Devices
                .Where(d => d.Id == usage.DeviceId)
                .Select(d => new { d.ActivityStatus, d.DeviceType })
                .AsNoTracking()
                .FirstOrDefaultAsync();


            if (device?.DeviceType?.Id == 3 && device?.ActivityStatus == true)
            {
                usage.Value = Math.Round((double)(usage?.PredictedValue * (1 + rand.NextDouble() * 0.4 - 0.2))!, 3);
            }
            else if (device?.DeviceType?.Id == 3 && device?.ActivityStatus == false)
            {
                usage.Value = 0;
            }
            else if (device?.DeviceType?.Id == 11)
            {
                usage.Value = Math.Min(Math.Max(Math.Round((double)(usage?.PredictedValue * (1 + rand.NextDouble() * 0.4 - 0.2))!, 3), 0), 1);
            }
            else if (device?.ActivityStatus == true)
            {
                usage.Value = Math.Round((double)(device?.DeviceType?.WattageInkW * (1 + rand.NextDouble() * 0.4 - 0.2))!, 3);
            } 
            else
            {
                usage.Value = 0;
            }
        }

        await _context.SaveChangesAsync();
    }
}