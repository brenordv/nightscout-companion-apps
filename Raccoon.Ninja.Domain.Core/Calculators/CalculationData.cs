using Raccoon.Ninja.Domain.Core.Entities;

namespace Raccoon.Ninja.Domain.Core.Calculators;

public record CalculationData
{
    public IList<float> GlucoseValues { get; init; }
    public float Average { get; init; }
    public int Count => GlucoseValues?.Count ?? 0;
    public float Median { get; init; }
    public float Min { get; init; }
    public float Max { get; init; }
    public float Mage { get; init; }
    public float StandardDeviation { get; init; }
    public float CoefficientOfVariation { get; init; }
    public HbA1CCalculation CurrentHbA1C { get; init; }
    public HbA1CCalculation PreviousHbA1C { get; init; }

    public CalculationDataTimeInRange TimeInRange { get; init; } = new ();
    public CalculationDataPercentile Percentile { get; init; } = new ();
    public CalculationDataStatus Status { get; init; } = new ();
}

public record CalculationDataTimeInRange
{
    public float Low { get; init; }
    public float Normal { get; init; }
    public float High { get; init; }
    public float VeryHigh { get; init; }
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
    public string FirstFailedStep { get; set; }
    public string Message { get; init; }
}