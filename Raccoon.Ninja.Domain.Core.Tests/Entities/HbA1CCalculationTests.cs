using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Tests.Entities;

public class HbA1CCalculationTests
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
        var calculation = new HbA1CCalculation
        {
            Id = id,
            Value = value,
            Delta = delta,
            ReferenceDate = referenceDate,
            Status = status,
            Error = error
        };

        // Assert
        calculation.DocType.Should().Be(AggregateType.HbA1CCalculation);
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
        var calculation = HbA1CCalculation.FromError(error, referenceDate);

        // Assert
        calculation.Id.Should().NotBeNullOrWhiteSpace();
        calculation.Value.Should().Be(default);
        calculation.Delta.Should().BeNull();
        calculation.Status.Should().Be(HbA1CCalculationStatus.Error);
        calculation.Error.Should().Be(error);
        calculation.ReferenceDate.Should().Be(referenceDate);
        calculation.DocType.Should().Be(AggregateType.HbA1CCalculation);
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
        

        var calculation1 = new HbA1CCalculation
        {
            Id = id,
            Value = value,
            Delta = delta,
            ReferenceDate = referenceDate,
            Status = status,
            Error = error
        };

        var calculation2 = new HbA1CCalculation
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
    public void GetHashCode_RecordsWithSameValues_ShouldHaveSameHashCode()
    {
        // Arrange
        var referenceDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var id = Guid.NewGuid().ToString();
        const HbA1CCalculationStatus status = HbA1CCalculationStatus.Error;
        const string error = "Sample error";
        const float value = 5.5f;
        const float delta = 0.5f;

        var calculation1 = new HbA1CCalculation
        {
            Id = id,
            Value = value,
            Delta = delta,
            ReferenceDate = referenceDate,
            Status = status,
            Error = error
        };

        var calculation2 = new HbA1CCalculation
        {
            Id = id,
            Value = value,
            Delta = delta,
            ReferenceDate = referenceDate,
            Status = status,
            Error = error
        };

        // Act & Assert
        calculation1.GetHashCode().Should().Be(calculation2.GetHashCode());
    }
    
    [Fact]
    public void Constructor_WithNoArguments_ShouldSetDefaultValues()
    {
        // Act
        var calculation = new HbA1CCalculation();

        // Assert
        calculation.Id.Should().NotBeNullOrWhiteSpace();
        calculation.Value.Should().Be(default);
        calculation.Delta.Should().BeNull();
        calculation.DocType.Should().Be(AggregateType.HbA1CCalculation);
        calculation.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow.ToUnixTimestamp(), 1000);
        calculation.Status.Should().Be(HbA1CCalculationStatus.NotCalculated);
        calculation.Error.Should().BeNull();
        calculation.ReferenceDate.Should().Be(default);
    }
}
