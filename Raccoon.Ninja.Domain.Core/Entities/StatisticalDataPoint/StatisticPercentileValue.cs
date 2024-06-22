using Newtonsoft.Json;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticPercentileValue
{
    [JsonProperty("p10")]
    public StatisticSimpleFloatValue P10 { get; init; }

    [JsonProperty("p25")]
    public StatisticSimpleFloatValue P25 { get; init; }

    [JsonProperty("p50")]
    public StatisticSimpleFloatValue P75 { get; init; }

    [JsonProperty("p75")]
    public StatisticSimpleFloatValue P90 { get; init; }

    [JsonProperty("iqr")]
    public StatisticSimpleFloatValue Iqr { get; init; }
}