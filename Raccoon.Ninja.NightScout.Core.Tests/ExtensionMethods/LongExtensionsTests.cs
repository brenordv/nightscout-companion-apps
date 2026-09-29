using Raccoon.Ninja.NightScout.Core.ExtensionMethods;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.NightScout.Core.Tests.ExtensionMethods;

public class LongExtensionsTests
{
    private static readonly TimeSpan MillisecondsAccountingFor3FirstPlaces = TimeSpan.FromMilliseconds(100);


    [Theory]
    [MemberData(nameof(TheoryGenerator.TimeStampAndCorrespondingDateTimes), MemberType = typeof(TheoryGenerator))]
    public void ToUtcDateTime_Success(long timestamp, DateTime expected)
    {
        // Arrange
        var actual = timestamp.ToUtcDateTime();

        // Assert
        Assert.Equal(expected, actual, MillisecondsAccountingFor3FirstPlaces);
    }
}
