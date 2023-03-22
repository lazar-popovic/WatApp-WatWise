using API.Model;
using API.Services.DevicesDataGenerator.Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Services.DevicesDataGenerator.Implementation;

public class DevicesDataGenerator : IDevicesDataGenerator
{
    private MongoClient _mongoClient;

    public DevicesDataGenerator()
    {
        _mongoClient = new MongoClient("mongodb://localhost:27017");
    }

    public async Task<List<BsonDocument>> GetUsageBetweenDates(string deviceType, DateTime startingDate, DateTime endingDate)
    {
        var database = _mongoClient.GetDatabase("devices");
        var collection = database.GetCollection<BsonDocument>("fridge");
        
        var filter = Builders<BsonDocument>.Filter.Eq("datetime", new BsonDateTime(new DateTime(2023, 3, 17)));
        var result = await collection.Find(filter).ToListAsync();

        return result;
    }
}
