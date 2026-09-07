using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Extensions.MongoDb.ExtensionMethods;
using Raccoon.Ninja.Extensions.MongoDb.Models;

namespace Raccoon.Ninja.Extensions.MongoDb.Tests.ExtensionMethods;

public class NightScoutMongoDocumentExtensionsTests
{
    [Fact]
    public void ToGlucoseReading_NullDocument_ReturnsNull()
    {
        // Arrange
        NightScoutMongoDocument document = null;

        // Act
        var result = document.ToGlucoseReading();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ToGlucoseReading_MapsValueTrendAndTimestamp()
    {
        // Arrange
        var document = new NightScoutMongoDocument
        {
            Value = 140,
            Trend = Trend.DoubleUp,
            ReadingTimestamp = 1583020799000
        };

        // Act
        var result = document.ToGlucoseReading();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(140f, result.Value);
        Assert.Equal(Trend.DoubleUp, result.Trend);
        Assert.Equal(1583020799000, result.ReadTimestampUtc);
    }

    [Fact]
    public void ToGlucoseReading_WithValidPreviousValue_CalculatesDelta()
    {
        // Arrange
        var document = new NightScoutMongoDocument { Value = 120 };

        // Act
        var result = document.ToGlucoseReading(100f);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(20f, result.Delta);
    }

    [Fact]
    public void ToGlucoseReading_WithoutPreviousValue_LeavesDeltaNull()
    {
        // Arrange
        var document = new NightScoutMongoDocument { Value = 120 };

        // Act
        var result = document.ToGlucoseReading();

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Delta);
    }

    [Fact]
    public void ToGlucoseReading_WithNonPositivePreviousValue_LeavesDeltaNull()
    {
        // Arrange
        var document = new NightScoutMongoDocument { Value = 120 };

        // Act
        var result = document.ToGlucoseReading(0f);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Delta);
    }
}
