using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;

namespace Raccoon.Ninja.AzFn.DataTransfer;

public class HbA1CCalcFunc
{
    private readonly ILogger _logger;

    public HbA1CCalcFunc(ILogger<HbA1CCalcFunc> logger)
    {
        _logger = logger;
    }

    [Function("HbA1cCalcFunc")]
    [CosmosDBOutput(
        "%CosmosDatabaseName%",
        "%CosmosAggregateContainerName%", 
        Connection = "CosmosConnectionString",
        CreateIfNotExists = false)]
    public string Run(
        [TimerTrigger("0 0 0 * * *"
            #if DEBUG
            , RunOnStartup = true
            #endif
            )] TimerInfo myTimer, 
        [CosmosDBInput(
            databaseName: "%CosmosDatabaseName%", 
            containerName: "%CosmosContainerName%",
            Connection = "CosmosConnectionString",
            SqlQuery = "SELECT TOP 33120 * FROM c ORDER BY c.readAt DESC"
        )] IEnumerable<GlucoseReading> readings, 
        [CosmosDBInput(
            databaseName: "%CosmosDatabaseName%", 
            containerName: "%CosmosAggregateContainerName%",
            Connection = "CosmosConnectionString",
            SqlQuery = "SELECT TOP 1 * FROM c WHERE c.docType = 1 ORDER BY c.createdAt DESC"
        )] IEnumerable<HbA1CCalculation> previousCalculations)
    {
        var referenceDate = DateOnly.FromDateTime(DateTime.UtcNow);

        _logger.LogTrace("Starting HbA1c calculation for {ReferenceDate}", referenceDate);

        try
        {
            var previousCalculation = previousCalculations.FirstOrDefault();

            var hbA1C = readings.CalculateHbA1C(referenceDate);

            if (previousCalculation is not null)
                hbA1C = hbA1C with { Delta = hbA1C.Value - previousCalculation.Value };

            var result = JsonConvert.SerializeObject(hbA1C);

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to calculate HbA1c for {ReferenceDate}", referenceDate);

            throw;
        }
        finally
        {
            _logger.LogTrace("HbA1c calculation for {ReferenceDate} finished!", referenceDate);
        }
    }
}