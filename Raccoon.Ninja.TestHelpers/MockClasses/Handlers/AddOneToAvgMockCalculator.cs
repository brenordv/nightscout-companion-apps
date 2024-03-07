using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

namespace Raccoon.Ninja.TestHelpers.MockClasses.Handlers;

public class AddOneToAvgMockCalculator: BaseCalculatorHandler
{
    public override CalculationData Handle(CalculationData data)
    {
        return HandleNext(data with
        {
            Average = data.Average + 1
        });
    }
}