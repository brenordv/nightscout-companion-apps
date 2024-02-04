using Newtonsoft.Json;

namespace Raccoon.Ninja.Domain.Core.Models;

public record DataLatestHbA1CFuncResponse
{
    [JsonProperty("latestSuccessful")]
    public HbA1CCalculationResponse LatestSuccessful { get; init; }
    
    [JsonProperty("latestPartialSuccessful")]
    public HbA1CCalculationResponse LatestPartialSuccessful { get; init; }
};