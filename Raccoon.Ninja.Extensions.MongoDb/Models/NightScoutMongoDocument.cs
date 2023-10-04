using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.Extensions.MongoDb.Models;

public record NightScoutMongoDocument
{
    [BsonElement("_id")] public ObjectId Id { get; init; }
    
    [BsonElement("sgv")] public int Value { get; init; }
    
    [BsonElement("date")] public long ReadingTimestamp { get; init; }
    
    [BsonElement("dateString")] public string ReadingTimestampAsString { get; init; }
    
    [BsonElement("trend")] public Trend Trend { get; init; }
    
    [BsonElement("direction")] public string Direction { get; init; }
    
    [BsonElement("device")] public string Device { get; init; }
    
    [BsonElement("type")] public string Type { get; init; }
    
    [BsonElement("utcOffset")] public int UtcOffset { get; init; }
    
    [BsonElement("sysTime")] public string SystemTime { get; init; }
}