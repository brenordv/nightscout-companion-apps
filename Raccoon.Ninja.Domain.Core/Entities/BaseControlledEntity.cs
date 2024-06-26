using System.Text.Json.Serialization;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Entities;

public record BaseControlledEntity : BaseEntity
{
    [JsonPropertyName("createdAt")]
    public long CreatedAtUtc { get; init; } = DateTime.UtcNow.ToUnixTimestamp();
}