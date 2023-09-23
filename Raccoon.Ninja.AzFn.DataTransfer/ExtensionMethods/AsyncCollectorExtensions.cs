using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Extensions.MongoDb.Models;

namespace Raccoon.Ninja.AzFn.DataTransfer.ExtensionMethods;

public static class AsyncCollectorExtensions
{
    public static async Task<int> AddRangeAsync(this IAsyncCollector<GlucoseReading> collector,
        IList<NightScoutMongoDocument> toAdd, ILogger log)
    {
        var count = 0;
        GlucoseReading current = null;
        var currentId = ObjectId.Empty;
        long previousTimestamp = -1;

        try
        {
            foreach (var item in toAdd)
            {
                currentId = item.Id;

                //Implicitly converting EntryDocument to GlucoseReading
                current = item;

                await collector.AddAsync(current);
                count++;
                previousTimestamp = current.ReadTimestampUtc;
            }
        }
        catch (CosmosException e)
        {
            log.LogError(e, "Failed to add item number {Count} of {Total}. " +
                            "Next execution will continue from '{LastOkTimestamp}' timestamp. " +
                            "Problematic source item id: {SourceId}. Converted reading: {Item}",
                count, toAdd.Count, previousTimestamp, currentId, current);
            throw;
        }
        catch (Exception e)
        {
            log.LogError(e, "Unexpected error while trying to add item number {Count} of {Total}. " +
                            "Next execution will continue from '{LastOkTimestamp}' timestamp. " +
                            "Problematic source item id: {SourceId}. Converted reading: {Item}",
                count, toAdd.Count, previousTimestamp, currentId, current);
            throw;
        }


        return count;
    }
}