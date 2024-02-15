using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

namespace Raccoon.Ninja.Domain.Core.Calculators.Handlers;

/// <summary>
///  Average Glucose: The average glucose level is a measure of central tendency that can be used to assess
/// overall glucose control.
/// </summary>
public class AverageCalculator: BaseCalculatorHandler
{
    public AverageCalculator(BaseCalculatorHandler nextHandler) : base(nextHandler)
    {
    }

    public override CalculationData Handle(CalculationData data)
    {
        if (!CanHandle(data))
        {
            return HandleError(data);
        }

        var average = data.GlucoseValues.Average();

        return HandleNext(data with
        {
            Average = average,
        });
    }
}