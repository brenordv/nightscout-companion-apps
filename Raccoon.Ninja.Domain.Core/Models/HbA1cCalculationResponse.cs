using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Models;

public record HbA1CCalculationResponse
{
    [JsonProperty("id")]
    public string Id { get; init; }

    [JsonProperty("value")]
    public float Value { get; init; }

    [JsonProperty("delta")]
    public float? Delta { get; init; }

    [JsonProperty("docType")]
    public DocumentType DocType { get; init; }

    [JsonProperty("referenceDate")]
    public DateOnly ReferenceDate { get; init; }

    [JsonProperty("createdAt")]
    public long CreatedAtUtc { get; init; }

    [JsonProperty("status")]
    public HbA1CCalculationStatus Status { get; init; }

    [JsonProperty("error")]
    public string Error { get; init; }

    /// <summary>
    ///     A calculation is considered stale if it's more than a day old.
    /// </summary>
    [JsonProperty("isStale")]
    public bool IsStale => DateTime.UtcNow.Date.AddDays(-1) > CreatedAtUtc.ToUtcDateTime().Date;
}