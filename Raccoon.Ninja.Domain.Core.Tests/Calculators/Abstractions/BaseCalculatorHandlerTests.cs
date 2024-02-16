using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;
using Raccoon.Ninja.Domain.Core.Calculators.Handlers;
using Raccoon.Ninja.TestHelpers;
using Raccoon.Ninja.TestHelpers.MockClasses.Handlers;

namespace Raccoon.Ninja.Domain.Core.Tests.Calculators.Abstractions;

public class BaseCalculatorHandlerTests
{
    [Fact]
    public void BuildChain_ShouldReturnChain()
    {
        // Arrange
        // Act
        var result = BaseCalculatorHandler.BuildChain();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<AverageCalculator>();
    }
    
    [Fact]
    public void HandleNext_WithMultipleHandlers_ShouldCallNextHandler()
    {
        // Arrange
        const float expectedAverage = 3;

        var lastHandler = new AddOneToAvgMockCalculator(null);

        var middleHandler = new AddOneToAvgMockCalculator(lastHandler);

        var firstHandler = new AddOneToAvgMockCalculator(middleHandler);
        
        // Act
        var result = firstHandler.Handle(new CalculationData());
        
        // Assert
        result.Average.Should().Be(expectedAverage);
    }
}