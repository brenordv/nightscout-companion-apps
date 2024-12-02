using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Raccoon.Ninja.Domain.Core.Models;

[ExcludeFromCodeCoverage]
public record DataLatestStatisticalAggregationFuncResponse
{
    [JsonPropertyName("latest")]
    public LatestStatisticsResponse Latest { get; init; }

    [JsonPropertyName("latestPartial")]
    public LatestStatisticsResponse LatestPartial { get; init; }
}