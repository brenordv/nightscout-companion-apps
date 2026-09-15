using MongoDB.Driver;
using Raccoon.Ninja.NightScout.Fn.GlucoseMonitor.Mongo;

namespace Raccoon.Ninja.NightScout.Fn.GlucoseMonitor.Tests.Mongo;

public class MongoCollectionExtensionsTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(long.MinValue)]
    public void GetDocumentsSince_NegativeTimestamp_ReturnsEmptyListWithoutQuerying(long timestamp)
    {
        // A negative timestamp short-circuits before the collection is touched,
        // so a null collection proves the guard runs first.
        IMongoCollection<NightScoutMongoDocument> collection = null;

        // Act
        var result = collection.GetDocumentsSince(timestamp);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
