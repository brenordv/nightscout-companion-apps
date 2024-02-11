using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Raccoon.Ninja.Domain.Core.Converters;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Models;

[ExcludeFromCodeCoverage]
public record GlucoseReadingResponse
{
    [JsonPropertyName("id")] public string Id { get; init; }
    
    [JsonPropertyName("trend")]
    public Trend Trend { get; init; }
    
    [JsonPropertyName("value")]
    public float Value { get; init; }
    
    [JsonPropertyName("delta")]
    public float? Delta { get; init; }
    
    [JsonPropertyName("readAt")]
    public long ReadTimestampUtc { get; init; }

    [JsonPropertyName("readAtDateTimeUtc")] public DateTime ReadTimestampUtcAsDateTime => ReadTimestampUtc.ToUtcDateTime();

    [JsonPropertyName("trendLabel")] public string TrendLabel => Converter.ToTrendString(Trend);
    
    public static implicit operator GlucoseReadingResponse(GlucoseReading reading) => new()
    {
        Id = reading.Id,
        Trend = reading.Trend,
        Value = reading.Value,
        ReadTimestampUtc = reading.ReadTimestampUtc,
        Delta = reading.Delta
    };
}