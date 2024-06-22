using Newtonsoft.Json;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticTimeInRangeValue
{
    [JsonProperty("low")]
    public StatisticSimpleFloatValue Low { get; init; }

    [JsonProperty("normal")]
    public StatisticSimpleFloatValue Normal { get; init; }

    [JsonProperty("high")]
    public StatisticSimpleFloatValue High { get; init; }

    [JsonProperty("veryHigh")]
    public StatisticSimpleFloatValue VeryHigh { get; init; }
}