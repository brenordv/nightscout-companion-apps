using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Raccoon.Ninja.Domain.Core.Models;

[ExcludeFromCodeCoverage]
public record DataLatestHbA1CFuncResponseLegacy
{
    [JsonPropertyName("latestSuccessful")]
    public AggregatedStatisticResponse LatestSuccessful { get; init; }

    [JsonPropertyName("latestPartialSuccessful")]
    public AggregatedStatisticResponse LatestPartialSuccessful { get; init; }
}