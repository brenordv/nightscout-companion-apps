using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;
using Raccoon.Ninja.Domain.Core.Converters;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;
using Raccoon.Ninja.Domain.Core.Exceptions;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.AzFn.ScheduledTasks;

public class StatisticsCalculationFunc
{
    private readonly ILogger _logger;

    public StatisticsCalculationFunc(ILogger<StatisticsCalculationFunc> logger)
    {
        _logger = logger;
    }

    [Function("StatisticsCalculationFunc")]
    [CosmosDBOutput(
        "%CosmosDatabaseName%",
        "%CosmosAggregateContainerName%",
        Connection = "CosmosConnectionString",
        CreateIfNotExists = false)]
    public StatisticDataPoint Run(
        [TimerTrigger("0 0 0 * * *"
#if DEBUG
            , RunOnStartup = true
#endif
        )]
        TimerInfo myTimer,
        [CosmosDBInput(
            "%CosmosDatabaseName%",
            "%CosmosContainerName%",
            Connection = "CosmosConnectionString",
            SqlQuery = "SELECT TOP 33120 * FROM c ORDER BY c.readAt DESC"
        )]
        IEnumerable<GlucoseReading> readings,
        [CosmosDBInput(
            "%CosmosDatabaseName%",
            "%CosmosAggregateContainerName%",
            Connection = "CosmosConnectionString",
            SqlQuery = "SELECT TOP 1 * FROM c WHERE c.docType = 1 and c.status = 1 ORDER BY c.createdAt DESC"
        )]
        IEnumerable<StatisticDataPoint> prevStatisticalDataPointFetch
    )
    {
        var referenceDate = DateOnly.FromDateTime(DateTime.UtcNow);

        _logger.LogTrace("Starting statistics calculation for {ReferenceDate}", referenceDate);

        try
        {
            var previousCalculations = prevStatisticalDataPointFetch.FirstOrDefault();

            var sortedGlucoseValues = readings.ToSortedValueArray();

            var chain = BaseCalculatorHandler.BuildChain();

            var calculatedData = chain.Handle(new CalculationData
            {
                GlucoseValues = sortedGlucoseValues
            });

            var result = EntityConverter.ToStatisticDataPoint(calculatedData, referenceDate, previousCalculations);

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to calculate statistic data for {ReferenceDate}", referenceDate);

            throw new NightScoutException($"Failed to calculate statistic data for {referenceDate}", e);
        }
        finally
        {
            _logger.LogTrace("Statistic data calculation for {ReferenceDate} finished!", referenceDate);
        }
    }
}