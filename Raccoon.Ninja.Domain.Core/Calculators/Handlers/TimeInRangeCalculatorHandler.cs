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
        var normal = data.GlucoseValues.Count(v => v is >= GlucoseConstants.LowGlucoseThreshold and < GlucoseConstants.HighGlucoseThreshold);
        var high = data.GlucoseValues.Count(v => v is >= GlucoseConstants.HighGlucoseThreshold and <= GlucoseConstants.VeryHighGlucoseThreshold);
        var veryHigh = data.GlucoseValues.Count(v => v > GlucoseConstants.VeryHighGlucoseThreshold);
        
        return data with
        {
            TimeInRange = new CalculationDataTimeInRange
            {
                Low = ToPercent(low, data.Count),
                Normal = ToPercent(normal, data.Count),
                High = ToPercent(high, data.Count),
                VeryHigh = ToPercent(veryHigh, data.Count)
            }
        };
    }

    private static float ToPercent(float value, float total)
    {
        return value / total * 100;
    }
}