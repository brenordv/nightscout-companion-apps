using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;
using Raccoon.Ninja.Domain.Core.Constants;

namespace Raccoon.Ninja.Domain.Core.Calculators.Handlers;

/// <summary>
/// Time in Range (TIR): TIR refers to the percentage of time glucose levels are within a target range.
/// This measure is increasingly used in diabetes management to assess how well blood glucose levels
/// are controlled.
/// </summary>
public class TimeInRangeCalculatorHandler: BaseCalculatorHandler
{
    protected override CalculationData RunCalculation(CalculationData data)
    {
        var low = data.GlucoseValues.Count(v => v < GlucoseConstants.LowGlucoseThreshold);
        var normal = data.GlucoseValues.Count(v => v >= GlucoseConstants.LowGlucoseThreshold && v <= GlucoseConstants.HighGlucoseThreshold);
        var high = data.GlucoseValues.Count(v => v >= GlucoseConstants.HighGlucoseThreshold && v <= GlucoseConstants.VeryHighGlucoseThreshold);
        var veryHigh = data.GlucoseValues.Count(v => v > GlucoseConstants.VeryHighGlucoseThreshold);

        return data with
        {
            TimeInRange = new CalculationDataTimeInRange
            {
                Low = ToPercents(low, data.Count),
                Normal = ToPercents(normal, data.Count),
                High = ToPercents(high, data.Count),
                VeryHigh = ToPercents(veryHigh, data.Count)
            }
        };
    }

    private static float ToPercents(float value, float total)
    {
        return value / total * 100;
    }
}