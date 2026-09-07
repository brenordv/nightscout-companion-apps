using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.Models;

namespace Raccoon.Ninja.Domain.Core.Tests.Models;

public class GlucoseReadingResponseTests
{
    [Fact]
    public void ImplicitConversion_FromGlucoseReading_MapsAllFields()
    {
        // Arrange
        var reading = new GlucoseReading
        {
            Id = "abc-123",
            Trend = Trend.SingleUp,
            Value = 120f,
            Delta = 3.5f,
            ReadTimestampUtc = 1583020799000
        };

        // Act
        GlucoseReadingResponse response = reading;

        // Assert
        Assert.Equal("abc-123", response.Id);
        Assert.Equal(Trend.SingleUp, response.Trend);
        Assert.Equal(120f, response.Value);
        Assert.Equal(3.5f, response.Delta);
        Assert.Equal(1583020799000, response.ReadTimestampUtc);
    }

    [Fact]
    public void TrendLabel_ReflectsTrend()
    {
        // Arrange
        var reading = new GlucoseReading { Trend = Trend.Flat };

        // Act
        GlucoseReadingResponse response = reading;

        // Assert
        Assert.Equal("Nice and easy", response.TrendLabel);
    }

    [Fact]
    public void ReadTimestampUtcAsDateTime_ConvertsFromTimestamp()
    {
        // Arrange
        var reading = new GlucoseReading { ReadTimestampUtc = 1583020799000 };

        // Act
        GlucoseReadingResponse response = reading;

        // Assert
        Assert.Equal(new DateTime(2020, 2, 29, 23, 59, 59, DateTimeKind.Utc), response.ReadTimestampUtcAsDateTime);
    }
}
