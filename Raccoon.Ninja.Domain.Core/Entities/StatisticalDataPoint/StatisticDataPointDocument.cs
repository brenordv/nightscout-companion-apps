using System.Text.Json.Serialization;
using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticDataPointDocument : BaseControlledEntity
{
    [JsonPropertyName("docType")]
    public DocumentType DocType { get; init; } = DocumentType.StatisticalData;

    [JsonPropertyName("referenceDate")]
    public DateOnly ReferenceDate { get; init; }

    [JsonPropertyName("full")]
    public StatisticDataPoint Full { get; init; }

    [JsonPropertyName("last30days")]
    public StatisticDataPoint Last30Days { get; init; }

    [JsonPropertyName("last15days")]
    public StatisticDataPoint Last15Days { get; init; }

    [JsonPropertyName("last7days")]
    public StatisticDataPoint Last7Days { get; init; }
}