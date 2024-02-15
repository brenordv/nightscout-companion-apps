using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

namespace Raccoon.Ninja.Domain.Core.Calculators.Handlers;

/// <summary>
/// Standard Deviation and Variance offer a measure of the variability or spread of glucose levels
/// around the mean. A higher standard deviation indicates greater variability, which could be
/// significant for managing diabetes.
/// </summary>
public class StandardDeviationCalculator: BaseCalculatorHandler
{
    public StandardDeviationCalculator(BaseCalculatorHandler nextHandler) : base(nextHandler)
    {
    }

    public override CalculationData Handle(CalculationData data)
    {
        if (!CanHandle(data))
        {
            return HandleError(data);
        }

        var glucoseValues = data.GlucoseValues;

        var average = data.Average;

        var standardDeviation = (float)Math.Sqrt(glucoseValues.Sum(r => Math.Pow(r - average, 2)) / glucoseValues.Count);

        return HandleNext(data with
        {
            StandardDeviation = standardDeviation
        });
    }
}