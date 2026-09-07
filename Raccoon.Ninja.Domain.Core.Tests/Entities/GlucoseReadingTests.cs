using Raccoon.Ninja.NightScout.Core.Entities;
using Raccoon.Ninja.NightScout.Core.Enums;

namespace Raccoon.Ninja.Domain.Core.Tests.Entities;

public class GlucoseReadingTests
{
    [Fact]
    public void Ctor_WhenInstantiated_ShouldHaveDefaultPropertyValues()
    {
        // Arrange
        const Trend expectedTrend = Trend.TripleUp;
        const long expectedReadTimestampUtc = 0;

        // Act
        var sut = new GlucoseReading();

        // Assert
        Assert.NotNull(sut);
        Assert.Equal(expectedTrend, sut.Trend);
        Assert.Equal(expectedReadTimestampUtc, sut.ReadTimestampUtc);
    }
}
