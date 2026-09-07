using System.Text.Json.Serialization;
using Raccoon.Ninja.NightScout.Core.ExtensionMethods;

namespace Raccoon.Ninja.NightScout.Core.Entities;

public record BaseControlledEntity : BaseEntity
{
    [JsonPropertyName("createdAt")]
    public long CreatedAtUtc { get; init; } = DateTime.UtcNow.ToUnixTimestamp();
}