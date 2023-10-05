using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Extensions.MongoDb.ExtensionMethods;
using Raccoon.Ninja.Extensions.MongoDb.Models;

namespace Raccoon.Ninja.AzFn.DataTransfer.ExtensionMethods;

public static class AsyncCollectorExtensions
{
    public static async Task<int> AddRangeAsync(this IAsyncCollector<GlucoseReading> collector,
        IList<NightScoutMongoDocument> toAdd, ILogger log, GlucoseReading previousReading)
    {
        var documentsProcessed = 0;
        GlucoseReading current = null;
        var previous = previousReading ?? new GlucoseReading();
        var currentMongoId = ObjectId.Empty;

        try
        {
            foreach (var mongoDbDoc in toAdd)
            {
                currentMongoId = mongoDbDoc.Id;
                current = mongoDbDoc.ToGlucoseReading(previous.Value);
                await collector.AddAsync(current);
                previous = current;
                documentsProcessed++;
            }
        }
        catch (CosmosException e)
        {
            log.LogError(e, "Failed to add item number {Count} of {Total}. " +
                            "Next execution will continue from '{LastOkTimestamp}' timestamp. " +
                            "Problematic source item id: {SourceId}. Converted reading: {Item}",
                documentsProcessed, toAdd.Count, previous.ReadTimestampUtc, currentMongoId, current);
            throw;
        }
        catch (Exception e)
        {
            log.LogError(e, "Unexpected error while trying to add item number {Count} of {Total}. " +
                            "Next execution will continue from '{LastOkTimestamp}' timestamp. " +
                            "Problematic source item id: {SourceId}. Converted reading: {Item}",
                documentsProcessed, toAdd.Count, previous.ReadTimestampUtc, currentMongoId, current);
            throw;
        }
        return documentsProcessed;
    }
}