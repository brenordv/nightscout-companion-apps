using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.AzFn.ScheduledTasks;

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
    public HbA1CCalculation Run(
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

            var sortedGlucoseValues = readings.ToSortedValueArray();

            var chain = BaseCalculatorHandler.BuildChain();

            var calculatedData = chain.Handle(new CalculationData
            {
                GlucoseValues = sortedGlucoseValues,
                PreviousHbA1C = previousCalculation
            });
            
            // TODO: Convert the calculated data into the appropriate CosmosDb documents. 
            return null;
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