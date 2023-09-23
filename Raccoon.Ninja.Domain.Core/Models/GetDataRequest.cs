using Newtonsoft.Json;

namespace Raccoon.Ninja.Domain.Core.Models;

public record GetDataRequest
{
    [JsonProperty("key")] public string Key { get; set; }
}