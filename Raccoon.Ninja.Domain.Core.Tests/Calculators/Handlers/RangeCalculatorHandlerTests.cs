using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Calculators.Handlers;

namespace Raccoon.Ninja.Domain.Core.Tests.Calculators.Handlers;

public class RangeCalculatorHandlerTests
{
    private readonly RangeCalculatorHandler _sut = new ();

    [Fact]
    public void Handle_ShouldReturnCorrectMinAndMax_WithValidData()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 1, 2, 3, 4, 5 }
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.Min.Should().Be(1);
        result.Max.Should().Be(5);
    }

    [Fact]
    public void Handle_ShouldHandleSingleValue()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 10 }
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.Min.Should().Be(10);
        result.Max.Should().Be(10);
    }

    [Fact]
    public void Handle_ShouldHandleNegativeValues()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { -10, -20, 0, 5, 10 }
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.Min.Should().Be(-20);
        result.Max.Should().Be(10);
    }

    [Fact]
    public void Handle_ShouldHandleMixedValues()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 5, 7, -3, 2, 8, -1 }
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.Min.Should().Be(-3);
        result.Max.Should().Be(8);
    }

    [Fact]
    public void Handle_ShouldHandleSameValues()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 5, 5, 5, 5, 5 }
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.Min.Should().Be(5);
        result.Max.Should().Be(5);
    }
}
