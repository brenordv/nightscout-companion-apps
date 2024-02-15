using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

namespace Raccoon.Ninja.Domain.Core.Calculators.Handlers;

/// <summary>
/// Range of glucose levels (difference between the maximum and minimum values) can provide insights into the
/// variability of glucose levels over a period.
/// </summary>
public class RangeCalculator: BaseCalculatorHandler
{
    public RangeCalculator(BaseCalculatorHandler nextHandler) : base(nextHandler)
    {
    }

    public override CalculationData Handle(CalculationData data)
    {
        if (!CanHandle(data))
        {
            return HandleError(data);
        }

        return HandleNext(data with
        {
            Min = data.GlucoseValues.Min(),
            Max = data.GlucoseValues.Max(),
        });
    }
}