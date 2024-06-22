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
        result.Should().BeOfType<AverageCalculatorHandler>();
    }
    
    [Fact]
    public void HandleNext_WithMultipleHandlers_ShouldCallNextHandler()
    {
        // Arrange
        const float expectedAverage = 3;

        var firstHandler = new AddOneToAvgMockCalculator();
        var middleHandler = new AddOneToAvgMockCalculator();
        var lastHandler = new AddOneToAvgMockCalculator();

        firstHandler.SetNextHandler(middleHandler);
        middleHandler.SetNextHandler(lastHandler);

        // Act
        var result = firstHandler.Handle(new CalculationData());
        
        // Assert
        result.Average.Should().Be(expectedAverage);
    }
    
    [Theory]
    [MemberData(nameof(TheoryGenerator.InvalidFloatListsWithNull), MemberType = typeof(TheoryGenerator))]
    public void HandleNext_WhenDataIsInvalid_ShouldReturnError(IList<float> glucoseReadings)
    {
        // Arrange
        var data = Generators.CalculationDataMockSingle(glucoseReadings);
        var handler = new DoNothingMockCalculator();
        
        // Act
        var result = handler.Handle(data);
        
        // Assert
        var status = result.Status;
        status.Success.Should().BeFalse();
        status.FailedAtStep.Should().Be(nameof(DoNothingMockCalculator));
        status.Message.Should().Be("No glucose values were provided.");
    }
}