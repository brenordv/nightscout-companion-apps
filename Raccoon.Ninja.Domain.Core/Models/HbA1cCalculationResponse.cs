using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Models;

public record HbA1cCalculationResponse
{
    [JsonProperty("id")] public string Id { get; init; }

    [JsonProperty("value")] public float Value { get; init; }

    [JsonProperty("delta", NullValueHandling = NullValueHandling.Ignore)]
    public float? Delta { get; init; }

    [JsonProperty("docType")] public AggregateType DocType { get; init; }
    [JsonProperty("referenceDate")] public DateOnly ReferenceDate { get; init; }
    [JsonProperty("createdAt")] public long CreatedAtUtc { get; init; }
    [JsonProperty("status")] public HbA1cCalculationStatus Status { get; init; }

    [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
    public string Error { get; init; }
    
    /// <summary>
    /// A calculation is considered stale if it's more than a day old.
    /// </summary>
    [JsonProperty("isStale")] public bool IsStale => DateTime.UtcNow.Date.AddDays(-1) > CreatedAtUtc.ToUtcDateTime().Date;

    public static implicit operator HbA1cCalculationResponse(HbA1cCalculation calculation)
    {
        if (calculation is null)
            return null;
        
        return new HbA1cCalculationResponse
        {
            Id = calculation.Id,
            Value = calculation.Value,
            Delta = calculation.Delta,
            DocType = calculation.DocType,
            ReferenceDate = calculation.ReferenceDate,
            CreatedAtUtc = calculation.CreatedAtUtc,
            Status = calculation.Status,
            Error = calculation.Error
        };
    }
}