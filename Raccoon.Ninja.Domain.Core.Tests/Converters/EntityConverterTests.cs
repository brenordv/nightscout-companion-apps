using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Converters;
using Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.Domain.Core.Tests.Converters;

public class EntityConverterTests
{
    private static readonly DateOnly ReferenceDate = DateOnly.FromDateTime(DateTime.UtcNow);
    private static readonly StatisticalDataPoint PreviousCalculation = Generators.StatisticalDataPointMockSingle();

    [Fact]
    public void ToStatisticDataPoint_ShouldReturnNull_WhenInputIsNull()
    {
        //Arrange
        CalculationData input = null;

        //Act
        var actual = EntityConverter.ToStatisticDataPoint(input, ReferenceDate, PreviousCalculation);

        //Assert
        actual.Should().BeNull();
    }

    [Fact]
    public void ToStatisticDataPoint_ShouldReturnError_WhenInputHasFailedStatus()
    {
        //Arrange
        const string failedStep = "Some step";
        const string failedMessage = "Some message";

        var input = new CalculationData
        {
            Status = new CalculationDataStatus
            {
                Success = false,
                FailedAtStep = failedStep,
                Message = failedMessage
            }
        };

        //Act
        var actual = EntityConverter.ToStatisticDataPoint(input, ReferenceDate, PreviousCalculation);

        //Assert
        actual.Should().NotBeNull();
        actual.Status.Should().Be(StatisticalDataPointDocStatus.Error);
        actual.Error.FailedStep.Should().Be(failedStep);
        actual.Error.ErrorMessage.Should().Be(failedMessage);
    }

    [Fact]
    public void ToStatisticDataPoint_ShouldReturnConverted_WhenInputIsSuccess()
    {
        //Arrange
        var glucoseValues = Generators.ListWithFloats(30).ToList();
        var input = Generators.CalculationDataMockSingle(glucoseValues);

        //Act
        var actual = EntityConverter.ToStatisticDataPoint(input, ReferenceDate, PreviousCalculation);

        //Assert
        actual.Should().NotBeNull();
        actual.Status.Should().Be(StatisticalDataPointDocStatus.Success);
        actual.ReferenceDate.Should().Be(ReferenceDate);
        actual.Average.Value.Should().Be(input.Average);
        actual.Median.Value.Should().Be(input.Median);
        actual.Min.Value.Should().Be(input.Min);
        actual.Max.Value.Should().Be(input.Max);
        actual.Mage.Threshold10.Value.Should().Be(input.Mage.Threshold10.Value);
        actual.Mage.Threshold20.Value.Should().Be(input.Mage.Threshold20.Value);
        actual.Mage.Absolute.Value.Should().Be(input.Mage.Absolute.Value);
        actual.StandardDeviation.Value.Should().Be(input.StandardDeviation);
        actual.CoefficientOfVariation.Value.Should().Be(input.CoefficientOfVariation);
        actual.HbA1C.Value.Should().Be(input.CurrentHbA1C.Value);
        actual.TimeInRange.Low.Value.Should().Be(input.TimeInRange.Low);
        actual.TimeInRange.Normal.Value.Should().Be(input.TimeInRange.Normal);
        actual.TimeInRange.High.Value.Should().Be(input.TimeInRange.High);
        actual.TimeInRange.VeryHigh.Value.Should().Be(input.TimeInRange.VeryHigh);
        actual.Percentile.P10.Value.Should().Be(input.Percentile.P10);
        actual.Percentile.P25.Value.Should().Be(input.Percentile.P25);
        actual.Percentile.P75.Value.Should().Be(input.Percentile.P75);
        actual.Percentile.P90.Value.Should().Be(input.Percentile.P90);
        actual.Percentile.Iqr.Value.Should().Be(input.Percentile.Iqr);
    }
}