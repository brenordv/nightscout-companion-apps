using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Calculators.Handlers;

namespace Raccoon.Ninja.Domain.Core.Tests.Calculators.Handlers;

public class MedianCalculatorHandlerTests
{
    //Failure tests covered by the base class
    private readonly MedianCalculatorHandler _sut = new();

    [Fact]
    public void Handle_WhenDataIsValid_ShouldReturnSuccess()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
        };
        
        const float expectedResult = 5.5f;
        
        // Act
        var result = _sut.Handle(data);
        
        // Assert
        result.Status.Success.Should().BeTrue();
        result.Median.Should().Be(expectedResult);
    }
}