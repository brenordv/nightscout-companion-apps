using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

namespace Raccoon.Ninja.Domain.Core.Calculators.Handlers;

/// <summary> 
/// Coefficient of Variation (CV) is a standardized measure of dispersion of a probability
/// distribution or frequency distribution. It's particularly useful because it allows for
/// comparison of variability across different mean glucose levels.
/// </summary>
public class CoefficientOfVariationCalculatorHandler: BaseCalculatorHandler
{
    protected override bool CanHandle(CalculationData data)
    {
        SetErrorMessage("Cannot calculate Coefficient of Variation without Standard Deviation and Average.");
        return data.StandardDeviation > 0 && data.Average > 0;
    }

    protected override CalculationData RunCalculation(CalculationData data)
    {
        return data with
        {
            CoefficientOfVariation = data.StandardDeviation / data.Average
        };
    }
}