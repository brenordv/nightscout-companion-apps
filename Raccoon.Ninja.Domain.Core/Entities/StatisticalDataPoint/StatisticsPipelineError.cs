using Newtonsoft.Json;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticsPipelineError
{
    [JsonProperty("failedStep")] public string FailedStep { get; init; }

    [JsonProperty("errorMessage")] public string ErrorMessage { get; init; }
}