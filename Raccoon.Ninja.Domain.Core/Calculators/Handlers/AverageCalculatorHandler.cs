using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

namespace Raccoon.Ninja.Domain.Core.Calculators.Handlers;

/// <summary>
///  Average Glucose: The average glucose level is a measure of central tendency that can be used to assess
/// overall glucose control.
/// </summary>
public class AverageCalculatorHandler: BaseCalculatorHandler
{
    protected override CalculationData RunCalculation(CalculationData data)
    {
        var average = data.GlucoseValues.Average();

        return data with
        {
            Average = average,
        };
    }
}