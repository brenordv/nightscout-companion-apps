using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

namespace Raccoon.Ninja.Domain.Core.Models;

[ExcludeFromCodeCoverage]
public record AggregatedStatisticResponse
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("referenceDate")]
    public DateOnly ReferenceDate { get; init; }

    [JsonPropertyName("daysSinceLastCalculation")]
    public int DaysSinceLastCalculation { get; init; }

    [JsonPropertyName("createdAt")]
    public long CreatedAt { get; init; }

    [JsonPropertyName("average")]
    public StatisticSimpleFloatValue Average { get; init; }

    [JsonPropertyName("median")]
    public StatisticSimpleFloatValue Median { get; init; }

    [JsonPropertyName("min")]
    public StatisticSimpleFloatValue Min { get; init; }

    [JsonPropertyName("max")]
    public StatisticSimpleFloatValue Max { get; init; }

    [JsonPropertyName("mage")]
    public StatisticMageValue Mage { get; init; }

    [JsonPropertyName("standardDeviation")]
    public StatisticSimpleFloatValue StandardDeviation { get; init; }

    [JsonPropertyName("coefficientOfVariation")]
    public StatisticSimpleFloatValue CoefficientOfVariation { get; init; }

    [JsonPropertyName("hbA1c")]
    public StatisticHbA1CValue HbA1C { get; init; }

    [JsonPropertyName("timeInRange")]
    public StatisticTimeInRangeValue TimeInRange { get; init; }

    [JsonPropertyName("percentile")]
    public StatisticPercentileValue Percentile { get; init; }

    public static implicit operator AggregatedStatisticResponse(StatisticDataPoint doc)
    {
        if (doc is null)
            return null;

        return new AggregatedStatisticResponse
        {
            Id = doc.Id,
            ReferenceDate = doc.ReferenceDate,
            DaysSinceLastCalculation = doc.DaysSinceLastCalculation,
            CreatedAt = doc.CreatedAt,
            Average = doc.Average,
            Median = doc.Median,
            Min = doc.Min,
            Max = doc.Max,
            Mage = doc.Mage,
            StandardDeviation = doc.StandardDeviation,
            CoefficientOfVariation = doc.CoefficientOfVariation,
            HbA1C = doc.HbA1C,
            TimeInRange = doc.TimeInRange,
            Percentile = doc.Percentile
        };
    }
}