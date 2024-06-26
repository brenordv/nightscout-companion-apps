using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Calculators.Handlers;

namespace Raccoon.Ninja.Domain.Core.Tests.Calculators.Handlers;

public class TimeInRangeCalculatorHandlerTests
{
    private readonly TimeInRangeCalculatorHandler _sut = new();

    [Fact]
    public void RunCalculation_ShouldReturnCorrectPercentages_WithValidData()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 70, 90, 150, 200, 260 }
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.TimeInRange.Low.Should().Be(20);
        result.TimeInRange.Normal.Should().Be(40);
        result.TimeInRange.High.Should().Be(20);
        result.TimeInRange.VeryHigh.Should().Be(20);
    }

    [Fact]
    public void RunCalculation_ShouldHandleSingleValue()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 150 }
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.TimeInRange.Low.Should().Be(0);
        result.TimeInRange.Normal.Should().Be(100);
        result.TimeInRange.High.Should().Be(0);
        result.TimeInRange.VeryHigh.Should().Be(0);
    }

    [Fact]
    public void RunCalculation_ShouldHandleAllLowValues()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 70, 75, 80, 60, 85 }
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.TimeInRange.Low.Should().Be(80);
        result.TimeInRange.Normal.Should().Be(20);
        result.TimeInRange.High.Should().Be(0);
        result.TimeInRange.VeryHigh.Should().Be(0);
    }

    [Fact]
    public void RunCalculation_ShouldHandleAllNormalValues()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 90, 100, 150, 160, 170 }
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.TimeInRange.Low.Should().Be(0);
        result.TimeInRange.Normal.Should().Be(100);
        result.TimeInRange.High.Should().Be(0);
        result.TimeInRange.VeryHigh.Should().Be(0);
    }

    [Fact]
    public void RunCalculation_ShouldHandleAllHighValues()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 180, 185, 200, 240, 250 }
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.TimeInRange.Low.Should().Be(0);
        result.TimeInRange.Normal.Should().Be(0);
        result.TimeInRange.High.Should().Be(100);
        result.TimeInRange.VeryHigh.Should().Be(0);
    }

    [Fact]
    public void RunCalculation_ShouldHandleAllVeryHighValues()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 260, 270, 280, 290, 300 }
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.TimeInRange.Low.Should().Be(0);
        result.TimeInRange.Normal.Should().Be(0);
        result.TimeInRange.High.Should().Be(0);
        result.TimeInRange.VeryHigh.Should().Be(100);
    }
}