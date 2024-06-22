using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Tests.Entities;

public class AggregationDataPointTests
{
    [Fact]
    public void PropInit_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var referenceDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var id = Guid.NewGuid().ToString();
        const HbA1CCalculationStatus status = HbA1CCalculationStatus.Error;
        const string error = "Sample error";
        const float value = 5.5f;
        const float delta = 0.5f;

        // Act
        var calculation = new AggregationDataPoint
        {
            Id = id,
            Value = value,
            Delta = delta,
            ReferenceDate = referenceDate,
            Status = status,
            Error = error
        };

        // Assert
        calculation.ReferenceDate.Should().Be(referenceDate);
        calculation.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow.ToUnixTimestamp(), 1000);
        calculation.Status.Should().Be(status);
        calculation.Error.Should().Be(error);
    }

    [Fact]
    public void FromError_ShouldCreateErrorInstanceCorrectly()
    {
        // Arrange
        const string error = "Error message";
        var referenceDate = DateOnly.FromDateTime(DateTime.UtcNow);

        // Act
        var calculation = AggregationDataPoint.FromError(error, referenceDate);

        // Assert
        calculation.Id.Should().NotBeNullOrWhiteSpace();
        calculation.Value.Should().Be(default);
        calculation.Delta.Should().BeNull();
        calculation.Status.Should().Be(HbA1CCalculationStatus.Error);
        calculation.Error.Should().Be(error);
        calculation.ReferenceDate.Should().Be(referenceDate);
    }

    [Fact]
    public void Records_WithSameValues_ShouldBeEqual()
    {
        // Arrange
        var referenceDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var id = Guid.NewGuid().ToString();
        const HbA1CCalculationStatus status = HbA1CCalculationStatus.Error; // Assuming Error is a valid enum value
        const string error = "Error message";
        const float value = 5.5f;
        const float delta = 0.5f;
        

        var calculation1 = new AggregationDataPoint
        {
            Id = id,
            Value = value,
            Delta = delta,
            ReferenceDate = referenceDate,
            Status = status,
            Error = error
        };

        var calculation2 = new AggregationDataPoint
        {
            Id = id,
            Value = value,
            Delta = delta,
            ReferenceDate = referenceDate,
            Status = status,
            Error = error
        };

        // Act & Assert
        calculation1.Should().BeEquivalentTo(calculation2);
    }

    [Fact]
    public void Constructor_WithNoArguments_ShouldSetDefaultValues()
    {
        // Act
        var calculation = new AggregationDataPoint();

        // Assert
        calculation.Id.Should().NotBeNullOrWhiteSpace();
        calculation.Value.Should().Be(default);
        calculation.Delta.Should().BeNull();
        calculation.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow.ToUnixTimestamp(), 1000);
        calculation.Status.Should().Be(HbA1CCalculationStatus.NotCalculated);
        calculation.Error.Should().BeNull();
        calculation.ReferenceDate.Should().Be(default);
    }
}
