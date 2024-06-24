using System.Text.Json.Serialization;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;
using Raccoon.Ninja.Domain.Core.Models;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticalDataPoint : BaseEntity
{
    [JsonPropertyName("docType")]
    public DocumentType DocType { get; init; } = DocumentType.StatisticalData;

    [JsonPropertyName("referenceDate")]
    public DateOnly ReferenceDate { get; init; }

    [JsonPropertyName("daysSinceLastCalculation")]
    public int DaysSinceLastCalculation { get; init; }

    [JsonPropertyName("createdAt")]
    public long CreatedAt { get; init; } = DateTime.UtcNow.ToUnixTimestamp();

    [JsonPropertyName("status")]
    public StatisticalDataPointDocStatus Status { get; init; } = StatisticalDataPointDocStatus.NotCalculated;

    [JsonPropertyName("error")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public StatisticsPipelineError Error { get; init; }

    [JsonPropertyName("average")]
    public StatisticSimpleFloatValue Average { get; init; }

    [JsonPropertyName("median")]
    public StatisticSimpleFloatValue Median { get; init; }

    [JsonPropertyName("min")]
    public StatisticSimpleFloatValue Min { get; init; }

    [JsonPropertyName("max")]
    public StatisticSimpleFloatValue Max { get; init; }

    [JsonPropertyName("mage")]
    public StatisticMageValue Mage { get; init; }

    [JsonPropertyName("standardDeviation")]
    public StatisticSimpleFloatValue StandardDeviation { get; init; }

    [JsonPropertyName("coefficientOfVariation")]
    public StatisticSimpleFloatValue CoefficientOfVariation { get; init; }

    [JsonPropertyName("hbA1c")]
    public StatisticHbA1CValue HbA1C { get; init; }

    [JsonPropertyName("timeInRange")]
    public StatisticTimeInRangeValue TimeInRange { get; init; }

    [JsonPropertyName("percentile")]
    public StatisticPercentileValue Percentile { get; init; }

    public static StatisticalDataPoint FromError(
        DateOnly referenceDate,
        string failedStep,
        string errorMessage)
    {
        return new StatisticalDataPoint
        {
            Status = StatisticalDataPointDocStatus.Error,
            ReferenceDate = referenceDate,
            Error = new StatisticsPipelineError
            {
                FailedStep = failedStep,
                ErrorMessage = errorMessage
            }
        };
    }

    public HbA1CCalculationResponse ToLegacyHbA1cCalculation()
    {
        if (HbA1C is null) return null;

        return new HbA1CCalculationResponse
        {
            Id = Id,
            ReferenceDate = ReferenceDate,
            CreatedAtUtc = CreatedAt,
            Status = HbA1C.Status,
            Error = Error?.ErrorMessage,
            Delta = HbA1C.Delta,
            DocType = (DocumentType)1, // Fixed value for obsolete enum. This will be removed in future version.
            Value = HbA1C.Value
        };
    }
}