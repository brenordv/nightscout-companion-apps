using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

namespace Raccoon.Ninja.Domain.Core.Models;

[ExcludeFromCodeCoverage]
public record LatestStatisticsResponse
{
    [JsonPropertyName("referenceDate")]
    public DateOnly ReferenceDate { get; init; }

    [JsonPropertyName("full")]
    public AggregatedStatisticResponse Full { get; init; }

    [JsonPropertyName("last30days")]
    public AggregatedStatisticResponse Last30Days { get; init; }

    [JsonPropertyName("last15days")]
    public AggregatedStatisticResponse Last15Days { get; init; }

    [JsonPropertyName("last7days")]
    public AggregatedStatisticResponse Last7Days { get; init; }

    public static implicit operator LatestStatisticsResponse(StatisticDataPointDocument doc)
    {
        if (doc is null)
            return null;

        return new LatestStatisticsResponse
        {
            ReferenceDate = doc.ReferenceDate,
            Full = doc.Full,
            Last30Days = doc.Last30Days,
            Last15Days = doc.Last15Days,
            Last7Days = doc.Last7Days
        };
    }
}