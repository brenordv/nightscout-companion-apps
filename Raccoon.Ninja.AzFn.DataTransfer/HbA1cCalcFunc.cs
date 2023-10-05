using System;
using System.Collections.Generic;
using System.Linq;
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
            SqlQuery = "SELECT TOP 1 * FROM c WHERE c.docType = 1 ORDER BY c.createdAt DESC"
        )] IEnumerable<HbA1cCalculation> previousCalculations,
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
            var previousCalculation = previousCalculations.FirstOrDefault();
            
            var hbA1c = readings.CalculateHbA1c(referenceDate);

            if (previousCalculation is not null)
            {
                hbA1c = hbA1c with {Delta = hbA1c.Value - previousCalculation.Value};
            }
            
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