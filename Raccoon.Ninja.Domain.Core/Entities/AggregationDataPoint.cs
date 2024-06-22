using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Entities;

[Obsolete("This is going to be removed on a newer version.")]
public record AggregationDataPoint : BaseValueEntity
{
    [JsonProperty("docType")]
    public DocumentType DocType { get; init; }

    [JsonProperty("referenceDate")]
    public DateOnly ReferenceDate { get; init; }

    [JsonProperty("createdAt")]
    public long CreatedAtUtc { get; init; } = DateTime.UtcNow.ToUnixTimestamp();

    [JsonProperty("status")]
    public HbA1CCalculationStatus Status { get; init; } = HbA1CCalculationStatus.NotCalculated;

    [JsonProperty("error")]
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