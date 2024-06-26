using System.Text.Json.Serialization;

namespace Raccoon.Ninja.Domain.Core.Models;

public record GetDataRequest
{
    [JsonPropertyName("key")]
    public string Key { get; set; }
}