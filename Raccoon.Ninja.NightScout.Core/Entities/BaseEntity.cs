using System.Text.Json.Serialization;

namespace Raccoon.Ninja.NightScout.Core.Entities;

public record BaseEntity
{
    [JsonPropertyName("id")] 
    public string Id { get; init; } = Guid.NewGuid().ToString();
}
