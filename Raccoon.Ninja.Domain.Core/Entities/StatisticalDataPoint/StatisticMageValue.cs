using System.Text.Json.Serialization;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticMageValue
{
    [JsonPropertyName("threshold10")]
    public StatisticMageValueResult Threshold10 { get; init; }

    [JsonPropertyName("threshold20")]
    public StatisticMageValueResult Threshold20 { get; init; }

    [JsonPropertyName("absolute")]
    public StatisticMageValueResult Absolute { get; init; }
}