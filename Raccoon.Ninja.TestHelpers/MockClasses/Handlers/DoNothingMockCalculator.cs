using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

namespace Raccoon.Ninja.TestHelpers.MockClasses.Handlers;

public class DoNothingMockCalculator: BaseCalculatorHandler
{
    protected override CalculationData RunCalculation(CalculationData data)
    {
        return data;
    }
}