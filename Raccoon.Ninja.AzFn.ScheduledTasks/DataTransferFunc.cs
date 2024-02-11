using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using Raccoon.Ninja.AzFn.ScheduledTasks.ExtensionMethods;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;
using Raccoon.Ninja.Extensions.MongoDb.Builders;
using Raccoon.Ninja.Extensions.MongoDb.ExtensionMethods;
using Raccoon.Ninja.Extensions.MongoDb.Models;

namespace Raccoon.Ninja.AzFn.ScheduledTasks;

public class DataTransferFunc
{
    private readonly ILogger _logger;

    public DataTransferFunc(ILogger<DataTransferFunc> logger)
    {
        _logger = logger;
    }

    [Function("DataTransferFunc")]
    [CosmosDBOutput(
        "%CosmosDatabaseName%",
        "%CosmosContainerName%", 
        Connection = "CosmosConnectionString",
        CreateIfNotExists = false)]
    public string Run(
        [TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo timer, 
        [CosmosDBInput(
            databaseName: "%CosmosDatabaseName%", 
            containerName: "%CosmosContainerName%",
            Connection = "CosmosConnectionString",
            SqlQuery = "SELECT TOP 1 * FROM c ORDER BY c.readAt DESC"
        )] IEnumerable<GlucoseReading> previousReadings)
    {
        _logger.LogInformation("Nightscout Data Transfer Function started");

        try
        {
            var previousReading = previousReadings.FirstOrDefault();

            var targetTimestamp = previousReading?.ReadTimestampUtc ?? 0;

            var collection = GetMongoCollection(_logger);

            var documents = collection.GetDocumentsSince(targetTimestamp);

            if (!documents.HasElements())
            {
                _logger.LogWarning("No documents to transfer");

                return null;
            }

            var glucoseReadings = documents.ToGlucoseReadings(previousReading);

            _logger.LogInformation("Converted {Count} documents to CosmosDb", documents.Count);

            var result = JsonConvert.SerializeObject(glucoseReadings);
            
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to transfer data from MongoDb to CosmosDb");

            throw;
        }
        finally
        {
            _logger.LogTrace("Nightscout Data Transfer Function finished!");
        }
    }

    /// <summary>
    /// Initializes the MongoDb collection.
    /// This is the source of data for Nightscout.
    /// </summary>
    /// <param name="log">Log instance created by Azure.</param>
    /// <returns>Collection Instance</returns>
    private static IMongoCollection<NightScoutMongoDocument> GetMongoCollection(ILogger log)
    {
        log.LogTrace("Getting MongoDb collection...");
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
        finally
        {
            log.LogTrace("Getting MongoDb collection... Done!");
        }
    }
}