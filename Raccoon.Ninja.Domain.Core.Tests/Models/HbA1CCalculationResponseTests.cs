using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;
using Raccoon.Ninja.Domain.Core.Models;

namespace Raccoon.Ninja.Domain.Core.Tests.Models;

public class HbA1CCalculationResponseTests
{
    [Fact]
    public void PropInit_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        const float value = 5.5f;
        const float delta = 0.5f;
        const DocumentType docType = DocumentType.StatisticalData;
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
    public void Constructor_WithNoArguments_ShouldSetDefaultValues()
    {
        // Act
        var response = new HbA1CCalculationResponse();

        // Assert
        response.Id.Should().BeNull();
        response.Value.Should().Be(default);
        response.Delta.Should().BeNull();
        response.DocType.Should().Be(DocumentType.Unknown);
        response.ReferenceDate.Should().Be(default);
        response.CreatedAtUtc.Should().Be(default);
        response.Status.Should().Be(HbA1CCalculationStatus.NotCalculated);
        response.Error.Should().BeNull();
        response.IsStale.Should().BeTrue();
    }
}