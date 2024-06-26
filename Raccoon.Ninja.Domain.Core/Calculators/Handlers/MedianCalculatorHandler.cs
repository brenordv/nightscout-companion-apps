using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

namespace Raccoon.Ninja.Domain.Core.Calculators.Handlers;

/// <summary>
///  Median: The median is the middle value of a set of values. If the set has an even number of values,
/// the median is the average of the two middle values.
/// For control of blood glucose, the median is a measure of central tendency that can be used to.
/// </summary>
public class MedianCalculatorHandler: BaseCalculatorHandler
{
    protected override CalculationData RunCalculation(CalculationData data)
    {
        var glucoseValues = data.GlucoseValues;
        var count = glucoseValues.Count;
        var isEven = count % 2 == 0;
        var middle = count / 2;

        return data with
        {
            Median = isEven
                ? (glucoseValues[(count - 1) / 2] + glucoseValues[count / 2]) / 2.0f
                : glucoseValues[middle],
        };
    }
}