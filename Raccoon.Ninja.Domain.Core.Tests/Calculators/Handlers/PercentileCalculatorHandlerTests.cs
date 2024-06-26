using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Calculators.Handlers;

namespace Raccoon.Ninja.Domain.Core.Tests.Calculators.Handlers;

public class PercentileCalculatorHandlerTests
{
    private readonly PercentileCalculatorHandler _sut = new ();
    
    [Fact]
    public void RunCalculation_ShouldCalculateCorrectPercentiles()
    {
        // Arrange
        var glucoseValues = new List<float> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var data = new CalculationData { GlucoseValues = glucoseValues };

        // Act
        var result = _sut.Handle(data);

        // Assert
        result.Percentile.P10.Should().BeApproximately(1.9f, 0.1f);
        result.Percentile.P25.Should().BeApproximately(3.25f, 0.1f);
        result.Percentile.P75.Should().BeApproximately(7.75f, 0.1f);
        result.Percentile.P90.Should().BeApproximately(9.1f, 0.1f);
        result.Percentile.Iqr.Should().BeApproximately(4.5f, 0.1f);
    }
}