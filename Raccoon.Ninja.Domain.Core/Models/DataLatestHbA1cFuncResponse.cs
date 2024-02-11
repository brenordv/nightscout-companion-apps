using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Raccoon.Ninja.Domain.Core.Models;

[ExcludeFromCodeCoverage]
public record DataLatestHbA1CFuncResponse
{
    [JsonPropertyName("latestSuccessful")]
    public HbA1CCalculationResponse LatestSuccessful { get; init; }
    
    [JsonPropertyName("latestPartialSuccessful")]
    public HbA1CCalculationResponse LatestPartialSuccessful { get; init; }
};