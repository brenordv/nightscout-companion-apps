using Newtonsoft.Json;

namespace Raccoon.Ninja.Domain.Core.Models;

public record DataLatestHbA1cFuncResponse
{
    [JsonProperty("latestSuccessful")]
    public HbA1cCalculationResponse LatestSuccessful { get; init; }
    
    [JsonProperty("latestPartialSuccessful")]
    public HbA1cCalculationResponse LatestPartialSuccessful { get; init; }
};