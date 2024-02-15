using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;
using Raccoon.Ninja.Domain.Core.Calculators.Handlers;

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
}