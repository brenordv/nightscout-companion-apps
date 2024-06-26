using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;
using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.Domain.Core.Converters;

public static class EntityConverter
{
    public static StatisticDataPoint ToStatisticDataPoint(
        CalculationData from,
        DateOnly referenceDate,
        StatisticDataPoint previousCalculations)
    {
        if (from is null)
            return null;

        if (!from.Status.Success)
            return StatisticDataPoint.FromError(referenceDate, from.Status.FailedAtStep, from.Status.Message);

        var result = new StatisticDataPoint
        {
            ReferenceDate = referenceDate,
            Status = StatisticalDataPointDocStatus.Success,
            DaysSinceLastCalculation = GetDaysSinceLastCalculation(referenceDate, previousCalculations?.ReferenceDate),
            Average = CalculateSimpleFloatResult(from.Average, previousCalculations?.Average?.Value),
            Median = CalculateSimpleFloatResult(from.Median, previousCalculations?.Median?.Value),
            Min = CalculateSimpleFloatResult(from.Min, previousCalculations?.Min?.Value),
            Max = CalculateSimpleFloatResult(from.Max, previousCalculations?.Max?.Value),
            Mage = CalculateMageResult(from.Mage, previousCalculations?.Mage),
            StandardDeviation =
                CalculateSimpleFloatResult(from.StandardDeviation, previousCalculations?.StandardDeviation?.Value),
            CoefficientOfVariation = CalculateSimpleFloatResult(from.CoefficientOfVariation,
                previousCalculations?.CoefficientOfVariation?.Value),
            HbA1C = CalculateHbA1CResult(from.CurrentHbA1C.Value, previousCalculations?.HbA1C?.Value,
                from.CurrentHbA1C.Status),
            TimeInRange = CalculateTimeInRangeResult(from.TimeInRange, previousCalculations?.TimeInRange),
            Percentile = CalculatePercentileResult(from.Percentile, previousCalculations?.Percentile)
        };

        return result;
    }

    private static StatisticSimpleFloatValue CalculateSimpleFloatResult(float currentValue, float? previousValue)
    {
        return new StatisticSimpleFloatValue
        {
            Value = currentValue,
            Delta = currentValue - previousValue
        };
    }

    private static StatisticMageValue CalculateMageResult(
        CalculationDataMage from,
        StatisticMageValue previous)
    {
        return new StatisticMageValue
        {
            Threshold10 = StatisticMageValueResult.FromSimpleFloatValue(
                CalculateSimpleFloatResult(from.Threshold10.Value, previous?.Threshold10?.Value),
                from.Threshold10.ExcursionsDetected),
            Threshold20 = StatisticMageValueResult.FromSimpleFloatValue(
                CalculateSimpleFloatResult(from.Threshold20.Value, previous?.Threshold20?.Value),
                from.Threshold20.ExcursionsDetected),
            Absolute = StatisticMageValueResult.FromSimpleFloatValue(
                CalculateSimpleFloatResult(from.Absolute.Value, previous?.Absolute?.Value),
                from.Absolute.ExcursionsDetected)
        };
    }

    private static StatisticHbA1CValue CalculateHbA1CResult(
        float currentValue,
        float? previousValue,
        HbA1CCalculationStatus currentStatus)
    {
        return new StatisticHbA1CValue
        {
            Value = currentValue,
            Delta = currentValue - previousValue,
            Status = currentStatus
        };
    }

    private static StatisticTimeInRangeValue CalculateTimeInRangeResult(
        CalculationDataTimeInRange from,
        StatisticTimeInRangeValue previous)
    {
        return new StatisticTimeInRangeValue
        {
            Low = CalculateSimpleFloatResult(from.Low, previous?.Low?.Value),
            Normal = CalculateSimpleFloatResult(from.Normal, previous?.Normal?.Value),
            High = CalculateSimpleFloatResult(from.High, previous?.High?.Value),
            VeryHigh = CalculateSimpleFloatResult(from.VeryHigh, previous?.VeryHigh?.Value)
        };
    }

    private static StatisticPercentileValue CalculatePercentileResult(
        CalculationDataPercentile from,
        StatisticPercentileValue previous)
    {
        return new StatisticPercentileValue
        {
            P10 = CalculateSimpleFloatResult(from.P10, previous?.P10?.Value),
            P25 = CalculateSimpleFloatResult(from.P25, previous?.P25?.Value),
            P75 = CalculateSimpleFloatResult(from.P75, previous?.P75?.Value),
            P90 = CalculateSimpleFloatResult(from.P90, previous?.P90?.Value),
            Iqr = CalculateSimpleFloatResult(from.Iqr, previous?.Iqr?.Value)
        };
    }

    private static int GetDaysSinceLastCalculation(DateOnly referenceDate, DateOnly? previousDate)
    {
        return !previousDate.HasValue
            ? 0
            : referenceDate.DayNumber - previousDate.Value.DayNumber;
    }
}