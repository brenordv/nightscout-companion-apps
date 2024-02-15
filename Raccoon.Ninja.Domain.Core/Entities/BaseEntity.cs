﻿using System.Text.Json.Serialization;

namespace Raccoon.Ninja.Domain.Core.Entities;

public record BaseEntity
{
    [JsonPropertyName("id")] 
    public string Id { get; init; } = Guid.NewGuid().ToString();
    
    [JsonPropertyName("value")]
    public float Value { get; init; }
    
    /// <summary>
    /// Difference between the current and the previous value, if available.
    /// </summary>
    [JsonPropertyName("delta")]
    public float? Delta { get; init; }
};