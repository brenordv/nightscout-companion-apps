using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.Domain.Core.Calculators;

public record CalculationData
{
    public IList<float> GlucoseValues { get; init; }
    public int Count => GlucoseValues?.Count ?? 0;
    public float Average { get; init; }
    public float Median { get; init; }
    public float Min { get; init; }
    public float Max { get; init; }
    public float Mage { get; init; }
    public float StandardDeviation { get; init; }
    public float CoefficientOfVariation { get; init; }
    public CalculationDataHbA1C CurrentHbA1C { get; init; }
    public CalculationDataTimeInRange TimeInRange { get; init; } = new();
    public CalculationDataPercentile Percentile { get; init; } = new();
    public CalculationDataStatus Status { get; init; } = new();
}

public record CalculationDataHbA1C
{
    public float Value { get; init; }
    public HbA1CCalculationStatus Status { get; init; }
}

public record CalculationDataPercentile
{
    public float P10 { get; init; }
    public float P25 { get; init; }
    public float P75 { get; init; }
    public float P90 { get; init; }
    public float Iqr { get; init; }
}

public record CalculationDataStatus
{
    public bool Success { get; init; } = true;
    public string FailedAtStep { get; init; }
    public string Message { get; init; }
}

public record CalculationDataTimeInRange
{
    public float Low { get; init; }
    public float Normal { get; init; }
    public float High { get; init; }
    public float VeryHigh { get; init; }
}