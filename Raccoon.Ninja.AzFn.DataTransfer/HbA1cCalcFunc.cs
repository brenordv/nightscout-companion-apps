using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.AzFn.DataTransfer;

public static class HbA1cCalcFunc
{
    [FunctionName("HbA1cCalcFunc")]
    public static async Task RunAsync(
        [TimerTrigger("0 0 0 * * *"
            #if DEBUG
            , RunOnStartup = true
            #endif
            )] TimerInfo myTimer,
        [CosmosDB(databaseName: "%CosmosDatabaseName%", containerName: "%CosmosContainerName%",
            Connection = "CosmosConnectionString",
            CreateIfNotExists = false,
            SqlQuery = "SELECT TOP 33120 * FROM c ORDER BY c.readAt DESC"
        )] IEnumerable<GlucoseReading> readings,
        [CosmosDB(databaseName: "%CosmosDatabaseName%", containerName: "%CosmosAggregateContainerName%",
            Connection = "CosmosConnectionString",
            CreateIfNotExists = false,
            PartitionKey = "/id"
        )]IAsyncCollector<HbA1cCalculation> calculationsOut,
        ILogger log)
    {
        var referenceDate = DateOnly.FromDateTime(DateTime.UtcNow);
        log.LogTrace("Starting HbA1c calculation for {ReferenceDate}", referenceDate);
        try
        {
            var hbA1c = readings.CalculateHbA1c(referenceDate);
            await calculationsOut.AddAsync(hbA1c);
        }
        catch (Exception e)
        {
            log.LogError(e, "Failed to calculate HbA1c for {ReferenceDate}", referenceDate);
        }
        finally
        {
            log.LogTrace("HbA1c calculation for {ReferenceDate} finished!", referenceDate);
        }
    }
}