using MongoDB.Driver;

namespace Raccoon.Ninja.NightScout.Fn.GlucoseMonitor.Mongo;

internal static class MongoCollectionExtensions
{
    private static readonly IList<NightScoutMongoDocument> EmptyList = new List<NightScoutMongoDocument>();

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
