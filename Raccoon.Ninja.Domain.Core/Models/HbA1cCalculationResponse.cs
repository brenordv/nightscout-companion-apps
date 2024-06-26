using System.Text.Json.Serialization;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Models;

public record HbA1CCalculationResponse
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("value")]
    public float Value { get; init; }

    [JsonPropertyName("delta")]
    public float? Delta { get; init; }

    [JsonPropertyName("docType")]
    public DocumentType DocType { get; init; }

    [JsonPropertyName("referenceDate")]
    public DateOnly ReferenceDate { get; init; }

    [JsonPropertyName("createdAt")]
    public long CreatedAtUtc { get; init; }

    [JsonPropertyName("status")]
    public HbA1CCalculationStatus Status { get; init; }

    [JsonPropertyName("error")]
    public string Error { get; init; }

    /// <summary>
    ///     A calculation is considered stale if it's more than a day old.
    /// </summary>
    [JsonPropertyName("isStale")]
    public bool IsStale => DateTime.UtcNow.Date.AddDays(-1) > CreatedAtUtc.ToUtcDateTime().Date;
}