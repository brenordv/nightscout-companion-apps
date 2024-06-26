﻿using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Calculators.Handlers;

namespace Raccoon.Ninja.Domain.Core.Tests.Calculators.Handlers;

public class CoefficientOfVariationCalculatorHandlerTests
{
    private readonly CoefficientOfVariationCalculatorHandler _sut = new ();
    
    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, -1)]
    [InlineData(-1, 0)]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void Handle_WhenDataIsInvalid_ShouldReturnError(float standardDeviation, float average)
    {
        // Arrange
        var data = new CalculationData
        {
            StandardDeviation = standardDeviation,
            Average = average
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        var status = result.Status;
        status.Success.Should().BeFalse();
        status.FailedAtStep.Should().Be(nameof(CoefficientOfVariationCalculatorHandler));
        status.Message.Should().Be("Cannot calculate Coefficient of Variation without Standard Deviation and Average.");
    }
    
    
    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(2, 1, 2)]
    [InlineData(1, 2, 0.5f)]
    [InlineData(2, 2, 1)]
    public void Handle_WhenDataIsValid_ShouldReturnCorrectValue(float standardDeviation, float average, float expected)
    {
        // Arrange
        var data = new CalculationData
        {
            StandardDeviation = standardDeviation,
            Average = average
        };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.CoefficientOfVariation.Should().Be(expected);
    }
}