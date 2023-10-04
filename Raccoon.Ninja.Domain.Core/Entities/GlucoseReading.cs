using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Entities;

public record GlucoseReading: BaseEntity
{
    [JsonProperty("trend")]
    public Trend Trend { get; init; }
    
    [JsonProperty("readAt")]
    public long ReadTimestampUtc { get; init; }

    [JsonIgnore] public DateTime ReadTimestampUtcAsDateTime => ReadTimestampUtc.ToUtcDateTime();
}