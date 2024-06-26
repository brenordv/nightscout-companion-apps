using System.Text.Json.Serialization;

namespace Raccoon.Ninja.Domain.Core.Entities;

public record BaseEntity
{
    [JsonPropertyName("id")] 
    public string Id { get; init; } = Guid.NewGuid().ToString();
}
