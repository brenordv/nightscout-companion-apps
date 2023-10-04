using Newtonsoft.Json;

namespace Raccoon.Ninja.Domain.Core.Entities;

public record BaseEntity
{
    [JsonProperty("id")] 
    public string Id { get; init; } = Guid.NewGuid().ToString();
    
    [JsonProperty("value")]
    public float Value { get; init; }
};