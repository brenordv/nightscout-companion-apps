using Raccoon.Ninja.Domain.Core.ExtensionMethods;

namespace Raccoon.Ninja.Domain.Core.Tests.ExtensionMethods;

public class DateTimeExtensionsTests
{
    [Fact]
    public void ToUnixTimestamp_KnownUtcDate_ReturnsExpectedMilliseconds()
    {
        // Arrange
        var date = new DateTime(2020, 2, 29, 23, 59, 59, DateTimeKind.Utc);
        const long expected = 1583020799000;

        // Act
        var result = date.ToUnixTimestamp();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToUnixTimestamp_RoundTripsWithToUtcDateTime()
    {
        // Arrange
        var date = new DateTime(2026, 9, 7, 3, 56, 24, DateTimeKind.Utc);

        // Act
        var result = date.ToUnixTimestamp().ToUtcDateTime();

        // Assert
        Assert.Equal(date, result);
    }
}
