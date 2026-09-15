using System.Text.Json.Serialization;
using Raccoon.Ninja.NightScout.Core.ExtensionMethods;
using Raccoon.Ninja.NightScout.Core.Enums;

namespace Raccoon.Ninja.NightScout.Core.Entities;

public record GlucoseReading : BaseValueEntity
{
    [JsonPropertyName("trend")] public Trend Trend { get; init; }

    [JsonPropertyName("readAt")] public long ReadTimestampUtc { get; init; }

    [JsonIgnore] public DateTime ReadTimestampUtcAsDateTime => ReadTimestampUtc.ToUtcDateTime();
}