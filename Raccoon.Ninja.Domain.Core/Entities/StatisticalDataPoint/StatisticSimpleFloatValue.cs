using System.Text.Json.Serialization;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticSimpleFloatValue
{
    [JsonPropertyName("value")]
    public float Value { get; init; }

    [JsonPropertyName("delta")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? Delta { get; init; }
}