using System.Text.Json.Serialization;
using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.Domain.Core.Entities;

[Obsolete("This is going to be removed on a newer version.")]
public record AggregationDataPoint : BaseValueEntity
{
    [JsonPropertyName("docType")]
    public DocumentType DocType { get; init; }

    [JsonPropertyName("referenceDate")]
    public DateOnly ReferenceDate { get; init; }

    [JsonPropertyName("status")]
    public HbA1CCalculationStatus Status { get; init; } = HbA1CCalculationStatus.NotCalculated;

    [JsonPropertyName("error")]
    public string Error { get; init; }

    /// <summary>
    ///     Creates an instance marked as error.
    /// </summary>
    /// <param name="error">Error message</param>
    /// <param name="referenceDate">Reference date for the calculation</param>
    /// <returns>Instance configured with error message</returns>
    public static AggregationDataPoint FromError(string error, DateOnly referenceDate)
    {
        return new AggregationDataPoint
        {
            Status = HbA1CCalculationStatus.Error,
            Error = error,
            ReferenceDate = referenceDate
        };
    }
}