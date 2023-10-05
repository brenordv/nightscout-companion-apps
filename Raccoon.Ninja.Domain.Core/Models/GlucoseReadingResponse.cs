using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Core.Converters;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Models;

public record GlucoseReadingResponse
{
    [JsonProperty("id")] public string Id { get; init; }
    
    [JsonProperty("trend")]
    public Trend Trend { get; init; }
    
    [JsonProperty("value")]
    public float Value { get; init; }
    
    [JsonProperty("delta", NullValueHandling = NullValueHandling.Ignore)]
    public float? Delta { get; init; }
    
    [JsonProperty("readAt")]
    public long ReadTimestampUtc { get; init; }

    [JsonProperty("readAtDateTimeUtc")] public DateTime ReadTimestampUtcAsDateTime => ReadTimestampUtc.ToUtcDateTime();

    [JsonProperty("trendLabel")] public string TrendLabel => Converter.ToTrendString(Trend);
    
    public static implicit operator GlucoseReadingResponse(GlucoseReading reading) => new()
    {
        Id = reading.Id,
        Trend = reading.Trend,
        Value = reading.Value,
        ReadTimestampUtc = reading.ReadTimestampUtc,
        Delta = reading.Delta
    };
}