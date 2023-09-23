using MongoDB.Bson;
using MongoDB.Driver;
using Raccoon.Ninja.Extensions.MongoDb.Models;

namespace Raccoon.Ninja.Extensions.MongoDb.ExtensionMethods;

public static class MongoCollectionExtensions
{
    private static readonly IList<NightScoutMongoDocument> EmptyList = new List<NightScoutMongoDocument>();

    public static NightScoutMongoDocument GetLatestDocument(this IMongoCollection<NightScoutMongoDocument> collection)
    {
        var filter = Builders<NightScoutMongoDocument>.Sort
            .Descending(doc => doc.ReadingTimestamp);

        var latestDocuments = collection
            .Find(new BsonDocument())
            .Sort(filter)
            .Limit(1)
            .FirstOrDefault();
        return latestDocuments;        
    }
    
    public static IList<NightScoutMongoDocument> GetDocumentsSince(
        this IMongoCollection<NightScoutMongoDocument> collection, long timestamp)
    {
        if (timestamp < 0)
        {
            return EmptyList;
        }

        var filter = Builders<NightScoutMongoDocument>.Filter.Gt(doc => doc.ReadingTimestamp, timestamp);
        var results = collection.Find(filter).ToList();
        return results;
    }
}