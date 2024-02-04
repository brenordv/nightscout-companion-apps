using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;
using Raccoon.Ninja.Domain.Core.Models;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.Domain.Core.Tests.Models;

public class HbA1CCalculationResponseTests
{
    [Fact]
    public void PropInit_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        const float value = 5.5f;
        const float delta = 0.5f;
        const AggregateType docType = AggregateType.HbA1CCalculation;
        const HbA1CCalculationStatus status = HbA1CCalculationStatus.Error;
        const string error = "No error";

        var id = Guid.NewGuid().ToString();
        var referenceDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var createdAtUtc = DateTimeExtensions.ToUnixTimestamp(DateTime.UtcNow);

        // Act
        var response = new HbA1CCalculationResponse
        {
            Id = id,
            Value = value,
            Delta = delta,
            DocType = docType,
            ReferenceDate = referenceDate,
            CreatedAtUtc = createdAtUtc,
            Status = status,
            Error = error
        };

        // Assert
        response.Id.Should().Be(id);
        response.Value.Should().Be(value);
        response.Delta.Should().Be(delta);
        response.DocType.Should().Be(docType);
        response.ReferenceDate.Should().Be(referenceDate);
        response.CreatedAtUtc.Should().Be(createdAtUtc);
        response.Status.Should().Be(status);
        response.Error.Should().Be(error);
    }

    [Fact]
    public void IsStale_ShouldBeTrue_IfMoreThanADayOld()
    {
        // Arrange
        var response = new HbA1CCalculationResponse
        {
            CreatedAtUtc = DateTimeExtensions.ToUnixTimestamp(DateTime.UtcNow.AddDays(-2))
        };

        // Act & Assert
        response.IsStale.Should().BeTrue();
    }

    [Fact]
    public void IsStale_ShouldBeFalse_IfLessThanADayOld()
    {
        // Arrange
        var response = new HbA1CCalculationResponse
        {
            CreatedAtUtc = DateTimeExtensions.ToUnixTimestamp(DateTime.UtcNow) // Now
        };

        // Act & Assert
        response.IsStale.Should().BeFalse();
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertCorrectly_FromHbA1CCalculation()
    {
        // Arrange
        var calculation = Generators.HbA1CCalculationMockSingle();

        // Act
        HbA1CCalculationResponse response = calculation; // Implicit conversion

        // Assert
        response.Id.Should().Be(calculation.Id);
        response.Value.Should().Be(calculation.Value);
        response.Delta.Should().Be(calculation.Delta);
        response.DocType.Should().Be(calculation.DocType);
        response.ReferenceDate.Should().Be(calculation.ReferenceDate);
        response.CreatedAtUtc.Should().Be(calculation.CreatedAtUtc);
        response.Status.Should().Be(calculation.Status);
        response.Error.Should().Be(calculation.Error);
    }
    
    [Fact]
    public void ImplicitOperator_ShouldConvertCorrectly_FromNullHbA1CCalculation()
    {
        // Arrange
        HbA1CCalculation calculation = null;

        // Act
        HbA1CCalculationResponse response = calculation; // Implicit conversion

        // Assert
        response.Should().BeNull();
    }
    
    [Fact]
    public void Constructor_WithNoArguments_ShouldSetDefaultValues()
    {
        // Act
        var response = new HbA1CCalculationResponse();

        // Assert
        response.Id.Should().BeNull();
        response.Value.Should().Be(default);
        response.Delta.Should().BeNull();
        response.DocType.Should().Be(AggregateType.Unknown);
        response.ReferenceDate.Should().Be(default);
        response.CreatedAtUtc.Should().Be(default);
        response.Status.Should().Be(HbA1CCalculationStatus.NotCalculated);
        response.Error.Should().BeNull();
        response.IsStale.Should().BeTrue();
    }
    
    [Fact]
    public void EqualityOperator_ShouldReturnFalse_IfNotEqual()
    {
        // Arrange
        var response1 = Generators.HbA1CCalculationResponseMockSingle();
        var response2 = Generators.HbA1CCalculationResponseMockSingle();

        // Act & Assert
        (response1 == response2).Should().BeFalse();
    }
    
    [Fact]
    public void EqualityOperator_ShouldReturnTrue_IfEqual()
    {
        // Arrange
        // Arrange
        const float value = 5.5f;
        const float delta = 0.5f;
        const AggregateType docType = AggregateType.HbA1CCalculation;
        const HbA1CCalculationStatus status = HbA1CCalculationStatus.Error;
        const string error = "No error";

        var id = Guid.NewGuid().ToString();
        var referenceDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var createdAtUtc = DateTimeExtensions.ToUnixTimestamp(DateTime.UtcNow);
        
        var response1 = new HbA1CCalculationResponse
        {
            Id = id,
            Value = value,
            Delta = delta,
            DocType = docType,
            ReferenceDate = referenceDate,
            CreatedAtUtc = createdAtUtc,
            Status = status,
            Error = error
        };
        
        var response2 = new HbA1CCalculationResponse
        {
            Id = id,
            Value = value,
            Delta = delta,
            DocType = docType,
            ReferenceDate = referenceDate,
            CreatedAtUtc = createdAtUtc,
            Status = status,
            Error = error
        };

        // Act & Assert
        (response1 == response2).Should().BeTrue();
    }
}
