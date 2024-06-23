using System.Text.Json.Serialization;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticMageValueResult : StatisticSimpleFloatValue
{
    [JsonPropertyName("excursionsDetected")]
    public bool ExcursionsDetected { get; init; }

    public static StatisticMageValueResult FromSimpleFloatValue(
        StatisticSimpleFloatValue value,
        bool excursionsDetected)
    {
        return new StatisticMageValueResult
        {
            Value = value.Value,
            Delta = value.Delta,
            ExcursionsDetected = excursionsDetected
        };
    }
}