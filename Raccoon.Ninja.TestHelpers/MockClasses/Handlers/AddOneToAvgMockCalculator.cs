using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

namespace Raccoon.Ninja.TestHelpers.MockClasses.Handlers;

public class AddOneToAvgMockCalculator: BaseCalculatorHandler
{
    protected override bool CanHandle(CalculationData data)
    {
        return data is not null;
    }

    protected override CalculationData RunCalculation(CalculationData data)
    {
        return data with
        {
            Average = data.Average + 1
        };
    }
}