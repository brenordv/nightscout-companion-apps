using Raccoon.Ninja.Domain.Core.Calculators.Handlers;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.Domain.Core.Tests.Calculators.Handlers;

public class AverageCalculatorHandlerTests
{
    private readonly AverageCalculatorHandler _sut = new ();
    
    [Theory]
    [MemberData(nameof(TheoryGenerator.InvalidFloatListsWithNull), MemberType = typeof(TheoryGenerator))]
    public void Handle_WhenDataIsInvalid_ShouldReturnError(IList<float> glucoseReadings)
    {
        // Arrange
        var data = Generators.CalculationDataMockSingle(glucoseReadings);
        
        // Act
        var result = _sut.Handle(data);
        
        // Assert
        var status = result.Status;
        status.Success.Should().BeFalse();
        status.FailedAtStep.Should().Be(nameof(AverageCalculatorHandler));
        status.Message.Should().Be("No glucose values were provided.");
    }
    
    [Fact]
    public void Handle_WhenDataIsValid_ShouldReturnSuccess()
    {
        // Arrange
        var data = Generators.CalculationDataMockSingle(Generators.ListWithFloats(10, 100f).ToList());
        
        // Act
        var result = _sut.Handle(data);
        
        // Assert
        result.Status.Success.Should().BeTrue();
        result.Average.Should().Be(100f);
    }
}