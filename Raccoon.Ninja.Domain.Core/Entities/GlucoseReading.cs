using System.Text.Json.Serialization;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Entities;

public record GlucoseReading: AggregationDataPoint
{
    [JsonPropertyName("trend")]
    public Trend Trend { get; init; }
    
    [JsonPropertyName("readAt")]
    public long ReadTimestampUtc { get; init; }

    [JsonIgnore] public DateTime ReadTimestampUtcAsDateTime => ReadTimestampUtc.ToUtcDateTime();
}