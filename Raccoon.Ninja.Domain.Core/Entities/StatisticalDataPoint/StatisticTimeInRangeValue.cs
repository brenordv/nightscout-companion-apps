using System.Text.Json.Serialization;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticTimeInRangeValue
{
    [JsonPropertyName("low")]
    public StatisticSimpleFloatValue Low { get; init; }

    [JsonPropertyName("normal")]
    public StatisticSimpleFloatValue Normal { get; init; }

    [JsonPropertyName("high")]
    public StatisticSimpleFloatValue High { get; init; }

    [JsonPropertyName("veryHigh")]
    public StatisticSimpleFloatValue VeryHigh { get; init; }
}