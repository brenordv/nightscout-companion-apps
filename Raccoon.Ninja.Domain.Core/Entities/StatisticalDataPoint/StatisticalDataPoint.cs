using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;
using Raccoon.Ninja.Domain.Core.Models;

namespace Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;

public record StatisticalDataPoint : BaseEntity
{
    [JsonProperty("docType")]
    public DocumentType DocType { get; init; } = DocumentType.StatisticalData;

    [JsonProperty("referenceDate")]
    public DateOnly ReferenceDate { get; init; }

    [JsonProperty("daysSinceLastCalculation")]
    public int DaysSinceLastCalculation { get; init; }

    [JsonProperty("createdAt")]
    public long CreatedAt { get; init; } = DateTime.UtcNow.ToUnixTimestamp();

    [JsonProperty("status")]
    public StatisticalDataPointDocStatus Status { get; init; } = StatisticalDataPointDocStatus.NotCalculated;

    [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
    public StatisticsPipelineError Error { get; init; }

    [JsonProperty("average")]
    public StatisticSimpleFloatValue Average { get; init; }

    [JsonProperty("median")]
    public StatisticSimpleFloatValue Median { get; init; }

    [JsonProperty("min")]
    public StatisticSimpleFloatValue Min { get; init; }

    [JsonProperty("max")]
    public StatisticSimpleFloatValue Max { get; init; }

    [JsonProperty("mage")]
    public StatisticMageValue Mage { get; init; }

    [JsonProperty("standardDeviation")]
    public StatisticSimpleFloatValue StandardDeviation { get; init; }

    [JsonProperty("coefficientOfVariation")]
    public StatisticSimpleFloatValue CoefficientOfVariation { get; init; }

    [JsonProperty("hbA1c")]
    public StatisticHbA1CValue HbA1C { get; init; }

    [JsonProperty("timeInRange")]
    public StatisticTimeInRangeValue TimeInRange { get; init; }

    [JsonProperty("percentile")]
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
            Error = Error?.ErrorMessage
        };
    }
}