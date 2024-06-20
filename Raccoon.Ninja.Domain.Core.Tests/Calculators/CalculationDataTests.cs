using Raccoon.Ninja.Domain.Core.Calculators;

namespace Raccoon.Ninja.Domain.Core.Tests.Calculators;

public class CalculationDataTests
{
    [Fact]
    public void CalculationData_Ctor_ShouldHaveDefaultValues()
    {
        // Arrange + Act
        var data = new CalculationData();
        
        // Assert
        data.GlucoseValues.Should().BeNull();
        data.Average.Should().Be(default);
        data.Count.Should().Be(0);
        data.Median.Should().Be(default);
        data.Min.Should().Be(default);
        data.Max.Should().Be(default);
        data.Mage.Should().Be(default);
        data.StandardDeviation.Should().Be(default);
        data.CoefficientOfVariation.Should().Be(default);
        data.CurrentHbA1C.Should().BeNull();
        data.PreviousHbA1C.Should().BeNull();
        data.TimeInRange.Should().NotBeNull();
        data.Percentile.Should().NotBeNull();
        data.Status.Should().NotBeNull();
    }
    
    [Fact]
    public void CalculationDataTimeInRange_Ctor_ShouldHaveDefaultValues()
    {
        // Arrange + Act
        var data = new CalculationDataTimeInRange();
        
        // Assert
        data.Low.Should().Be(default);
        data.Normal.Should().Be(default);
        data.High.Should().Be(default);
        data.VeryHigh.Should().Be(default);
    }
    
    [Fact]
    public void CalculationDataPercentile_Ctor_ShouldHaveDefaultValues()
    {
        // Arrange + Act
        var data = new CalculationDataPercentile();
        
        // Assert
        data.P10.Should().Be(default);
        data.P25.Should().Be(default);
        data.P75.Should().Be(default);
        data.P90.Should().Be(default);
        data.Iqr.Should().Be(default);
    }
    
    [Fact]
    public void CalculationDataStatus_Ctor_ShouldHaveDefaultValues()
    {
        // Arrange + Act
        var data = new CalculationDataStatus();
        
        // Assert
        data.Success.Should().BeTrue();
        data.FirstFailedStep.Should().BeNull();
        data.Message.Should().Be(default);
    }
}