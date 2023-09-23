using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Entities;

public record GlucoseReading
{
    [JsonProperty("id")] public string Id { get; init; } = Guid.NewGuid().ToString();
    
    [JsonProperty("trend")]
    public Trend Trend { get; init; }
    
    [JsonProperty("value")]
    public float Value { get; init; }
    
    [JsonProperty("readAt")]
    public long ReadTimestampUtc { get; init; }

    [JsonIgnore] public DateTime ReadTimestampUtcAsDateTime => ReadTimestampUtc.ToUtcDateTime();
}