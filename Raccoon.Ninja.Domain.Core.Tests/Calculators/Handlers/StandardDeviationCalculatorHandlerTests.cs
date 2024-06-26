using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Calculators.Handlers;

namespace Raccoon.Ninja.Domain.Core.Tests.Calculators.Handlers;

public class StandardDeviationCalculatorHandlerTests
{
    private readonly StandardDeviationCalculatorHandler _sut = new ();

    [Fact]
    public void RunCalculation_ShouldReturnCorrectStandardDeviation_WithValidData()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 1, 2, 3, 4, 5 },
            Average = 3
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.StandardDeviation.Should().BeApproximately(1.4142f, 0.0001f);
    }

    [Fact]
    public void RunCalculation_ShouldHandleSingleValue()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 10 },
            Average = 10
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.StandardDeviation.Should().Be(0);
    }

    [Fact]
    public void RunCalculation_ShouldHandleNegativeValues()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { -10, -20, 0, 5, 10 },
            Average = -3
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.StandardDeviation.Should().BeApproximately(10.7703f, 0.0001f);
    }

    [Fact]
    public void RunCalculation_ShouldHandleMixedValues()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 5, 7, -3, 2, 8, -1 },
            Average = 3
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.StandardDeviation.Should().BeApproximately(4.0414f, 0.0001f);
    }

    [Fact]
    public void RunCalculation_ShouldHandleSameValues()
    {
        // Arrange
        var data = new CalculationData
        {
            GlucoseValues = new List<float> { 5, 5, 5, 5, 5 },
            Average = 5
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.StandardDeviation.Should().Be(0);
    }
}