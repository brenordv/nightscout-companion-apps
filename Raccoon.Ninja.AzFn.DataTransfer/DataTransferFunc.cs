using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Raccoon.Ninja.AzFn.DataTransfer.ExtensionMethods;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;
using Raccoon.Ninja.Extensions.MongoDb.Builders;
using Raccoon.Ninja.Extensions.MongoDb.ExtensionMethods;
using Raccoon.Ninja.Extensions.MongoDb.Models;

namespace Raccoon.Ninja.AzFn.DataTransfer;

public static class DataTransferFunc
{
    [FunctionName("DataTransferFunc")]
    public static async Task RunAsync(
        [TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo timer,
        [CosmosDB(databaseName: "%CosmosDatabaseName%", containerName: "%CosmosContainerName%",
            Connection = "CosmosConnectionString",
            CreateIfNotExists = false,
            SqlQuery = "SELECT TOP 1 * FROM c ORDER BY c.readAt DESC"
        )] IEnumerable<GlucoseReading> previousReadings,
        [CosmosDB(databaseName: "%CosmosDatabaseName%", containerName: "%CosmosContainerName%",
            Connection = "CosmosConnectionString",
            CreateIfNotExists = false,
            PartitionKey = "/id"
        )]IAsyncCollector<GlucoseReading> readingsOut,
        ILogger log)
    {
        log.LogInformation("Nightscout Data Transfer Function started");
        try
        {
            var collection = GetMongoCollection(log);

            var targetTimestamp = previousReadings.GetLatestTimestamp();

            var documents = collection.GetDocumentsSince(targetTimestamp);

            if (!documents.HasElements())
            {
                log.LogWarning("No documents to transfer");
                return;
            }
            
            var documentsAdded = await readingsOut.AddRangeAsync(documents, log);

            log.LogInformation("Added {Count} documents to CosmosDb", documentsAdded);
        }
        catch (Exception e)
        {
            log.LogError(e, "Failed to transfer data from MongoDb to CosmosDb");
            throw;
        }
        finally
        {
            log.LogTrace("Nightscout Data Transfer Function finished!");
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