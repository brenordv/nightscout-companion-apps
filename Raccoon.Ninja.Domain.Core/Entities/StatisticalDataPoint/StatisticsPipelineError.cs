using System.Text.Json.Serialization;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticsPipelineError
{
    [JsonPropertyName("failedStep")]
    public string FailedStep { get; init; }

    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; init; }
}