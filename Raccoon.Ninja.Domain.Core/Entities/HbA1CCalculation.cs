﻿using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Entities;

public record HbA1CCalculation: BaseEntity
{
    [JsonProperty("docType")] public AggregateType DocType { get; init; } = AggregateType.HbA1CCalculation;
    [JsonProperty("referenceDate")] public DateOnly ReferenceDate { get; init; }
    [JsonProperty("createdAt")] public long CreatedAtUtc { get; init; } = DateTime.UtcNow.ToUnixTimestamp();
    [JsonProperty("status")] public HbA1CCalculationStatus Status { get; init; } = HbA1CCalculationStatus.NotCalculated;
    [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)] public string Error { get; init; }

    /// <summary>
    /// Creates an instance marked as error.
    /// </summary>
    /// <param name="error">Error message</param>
    /// <param name="referenceDate">Reference date for the calculation</param>
    /// <returns>Instance configured with error message</returns>
    public static HbA1CCalculation FromError(string error, DateOnly referenceDate)
    {
        return new HbA1CCalculation
        {
            Status = HbA1CCalculationStatus.Error,
            Error = error,
            ReferenceDate = referenceDate
        };
    }
}