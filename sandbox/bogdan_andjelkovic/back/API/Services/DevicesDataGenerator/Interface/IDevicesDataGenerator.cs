using API.Model;
using MongoDB.Bson;

namespace API.Services.DevicesDataGenerator.Interface;

public interface IDevicesDataGenerator
{
    Task<List<BsonDocument>> GetUsageBetweenDates( string deviceType, DateTime startingDate, DateTime endingDate);
}