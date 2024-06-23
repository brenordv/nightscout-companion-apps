using Newtonsoft.Json;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticSimpleFloatValue
{
    [JsonProperty("value")]
    public float Value { get; init; }

    [JsonProperty("delta", NullValueHandling = NullValueHandling.Ignore)]
    public float? Delta { get; init; }
}