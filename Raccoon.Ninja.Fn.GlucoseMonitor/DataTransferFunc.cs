using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Raccoon.Ninja.NightScout.Core.Entities;
using Raccoon.Ninja.NightScout.Core.Exceptions;
using Raccoon.Ninja.NightScout.Core.ExtensionMethods;
using Raccoon.Ninja.Fn.GlucoseMonitor.ExtensionMethods;
using Raccoon.Ninja.Fn.GlucoseMonitor.Mongo;

namespace Raccoon.Ninja.Fn.GlucoseMonitor;

public class DataTransferFunc(ILogger<DataTransferFunc> logger)
{
    private readonly ILogger _logger = logger;

    [Function("DataTransferFunc")]
    [CosmosDBOutput(
        "%CosmosDatabaseName%",
        "%CosmosContainerName%",
        Connection = "CosmosConnectionString",
        CreateIfNotExists = false)]
    public IEnumerable<GlucoseReading> Run(
        [TimerTrigger("0 */5 * * * *", RunOnStartup = true)]
        TimerInfo timer,
        [CosmosDBInput(
            "%CosmosDatabaseName%",
            "%CosmosContainerName%",
            Connection = "CosmosConnectionString",
            SqlQuery = "SELECT TOP 1 * FROM c ORDER BY c.readAt DESC"
        )]
        IEnumerable<GlucoseReading> previousReadings)
    {
        try
        {
            var previousReading = previousReadings.FirstOrDefault();

            var targetTimestamp = previousReading?.ReadTimestampUtc ?? 0;

            var collection = GetMongoCollection(_logger);

            var documents = collection.GetDocumentsSince(targetTimestamp);

            if (!documents.HasElements())
            {
                var targetTimestampUtc = DateTimeOffset.FromUnixTimeMilliseconds(targetTimestamp)
                    .UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
                _logger.LogInformation("No new readings since {TargetTimestampUtc}.", targetTimestampUtc);

                return new List<GlucoseReading>();
            }

            var glucoseReadings = documents.ToGlucoseReadings(previousReading);

            _logger.LogInformation("Transferred {Count} readings to Cosmos DB.", documents.Count);

            return glucoseReadings;
        }
        catch (Exception e)
        {
            const string errorMessage = "Failed to transfer data from MongoDb to CosmosDb";
            _logger.LogError(e, errorMessage);

            throw new NightScoutException(errorMessage, e);
        }
    }

    /// <summary>
    ///     Initializes the MongoDb collection.
    ///     This is the source of data for Nightscout.
    /// </summary>
    /// <param name="log">Log instance created by Azure.</param>
    /// <returns>Collection Instance</returns>
    private static IMongoCollection<NightScoutMongoDocument> GetMongoCollection(ILogger log)
    {
        var connectionString = Environment.GetEnvironmentVariable("MongoDbConnectionString");
        var databaseName = Environment.GetEnvironmentVariable("MongoDbDatabaseName");
        var collectionName = Environment.GetEnvironmentVariable("MongoDbCollectionName");
        try
        {
            return new MongoCollectionBuilder()
                .AddConnectionString(connectionString)
                .AddDatabaseName(databaseName)
                .AddCollectionName(collectionName)
                .Build<NightScoutMongoDocument>();
        }
        catch (Exception e)
        {
            log.LogError(e,
                "Error while getting MongoDb collection. " +
                "Connection string size: {ConnectionStringSize} " +
                "| Database: {DatabaseName} " +
                "| Collection: {CollectionName}",
                string.IsNullOrWhiteSpace(connectionString) ? "null" : connectionString.Length,
                databaseName,
                collectionName);
            throw;
        }
    }
}
