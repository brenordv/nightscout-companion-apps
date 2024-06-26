using System.Text.Json.Serialization;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticPercentileValue
{
    [JsonPropertyName("p10")]
    public StatisticSimpleFloatValue P10 { get; init; }

    [JsonPropertyName("p25")]
    public StatisticSimpleFloatValue P25 { get; init; }

    [JsonPropertyName("p50")]
    public StatisticSimpleFloatValue P75 { get; init; }

    [JsonPropertyName("p75")]
    public StatisticSimpleFloatValue P90 { get; init; }

    [JsonPropertyName("iqr")]
    public StatisticSimpleFloatValue Iqr { get; init; }
}