using Newtonsoft.Json;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticMageValue
{
    [JsonProperty("threshold10")]
    public StatisticMageValueResult Threshold10 { get; init; }

    [JsonProperty("threshold20")]
    public StatisticMageValueResult Threshold20 { get; init; }

    [JsonProperty("absolute")]
    public StatisticMageValueResult Absolute { get; init; }
}