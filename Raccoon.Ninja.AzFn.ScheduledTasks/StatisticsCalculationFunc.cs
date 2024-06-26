using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.AzFn.ScheduledTasks.ExtensionMethods;
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
    public StatisticDataPointDocument Run(
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
        IEnumerable<GlucoseReading> readingsFetcher,
        [CosmosDBInput(
            "%CosmosDatabaseName%",
            "%CosmosAggregateContainerName%",
            Connection = "CosmosConnectionString",
            SqlQuery = "SELECT TOP 1 * FROM c WHERE c.docType = 1 and c.full.status = 1 ORDER BY c.createdAt DESC"
        )]
        IEnumerable<StatisticDataPointDocument> prevStatisticalDataPointFetch
    )
    {
        var referenceDate = DateOnly.FromDateTime(DateTime.UtcNow);

        _logger.LogTrace("Starting statistics calculation for {ReferenceDate}", referenceDate);

        try
        {
            var previousCalculations = prevStatisticalDataPointFetch.FirstOrDefault();

            var chain = BaseCalculatorHandler.BuildChain();

            var readings = readingsFetcher.ToList();

            var fullResult = CalculateStatistics(readings, chain, referenceDate, previousCalculations?.Full);
            var last30DaysResult = CalculateStatistics(readings.GetLastDays(3), chain, referenceDate,
                previousCalculations?.Last30Days);
            var last15DaysResult = CalculateStatistics(readings.GetLastDays(2), chain, referenceDate,
                previousCalculations?.Last15Days);
            var last7DaysResult = CalculateStatistics(readings.GetLastDays(1), chain, referenceDate,
                previousCalculations?.Last7Days);

            return new StatisticDataPointDocument
            {
                ReferenceDate = referenceDate,
                Full = fullResult,
                Last30Days = last30DaysResult,
                Last15Days = last15DaysResult,
                Last7Days = last7DaysResult
            };
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

    private StatisticDataPoint CalculateStatistics(
        IEnumerable<GlucoseReading> readings,
        BaseCalculatorHandler chain,
        DateOnly referenceDate,
        StatisticDataPoint previousCalculations)
    {
        var sortedGlucoseValues = readings.ToSortedValueArray();
        var calculatedData = chain.Handle(new CalculationData
        {
            GlucoseValues = sortedGlucoseValues
        });

        var result = EntityConverter.ToStatisticDataPoint(calculatedData, referenceDate, previousCalculations);
        return result;
    }
}