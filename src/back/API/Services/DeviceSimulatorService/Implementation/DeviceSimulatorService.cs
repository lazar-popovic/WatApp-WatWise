﻿using API.Models.Entity;
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

        var devices = await _context.Devices
            .GroupBy(d => d.Type)
            .Select(g => new { Type = g.Key, Devices = g.Select( d => new { d.Id, d.ActivityStatus }).ToList() })
            .ToListAsync();

        var deviceEnergyUsageList = new List<DeviceEnergyUsage>();

        foreach (var deviceType in devices)
        {
            var collection = _devices.GetCollection<BsonDocument>(deviceType.Type);
            var filter = Builders<BsonDocument>.Filter.Eq("timestamp", timestamp);
            var result = await collection.Find(filter).FirstOrDefaultAsync();

            if (result != null)
            {
                var value = result["value"].ToDouble();

                foreach (var device in deviceType.Devices)
                {
                    var insertValue = device.ActivityStatus == true ? value : 0.0;
                    deviceEnergyUsageList.Add(new DeviceEnergyUsage { DeviceId = device.Id, Value = insertValue, Timestamp = timestamp });
                }
            }
        }

        await _context.DeviceEnergyUsage.AddRangeAsync(deviceEnergyUsageList);
        await _context.SaveChangesAsync();
    }
}