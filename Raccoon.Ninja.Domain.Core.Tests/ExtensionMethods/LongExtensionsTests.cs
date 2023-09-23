using FluentAssertions;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.Domain.Core.Tests.ExtensionMethods;

public class LongExtensionsTests
{
    private static readonly TimeSpan MillisecondsAccountingFor3FirstPlaces = TimeSpan.FromMilliseconds(100);

    [Theory]
    [MemberData(nameof(TheoryGenerator.TimeStampAndCorrespondingDateTimes), MemberType = typeof(TheoryGenerator))]
    public void ToUtcDateTime_Success(long timestamp, DateTime expected)
    {
        //Arrange
        var actual = timestamp.ToUtcDateTime();
        
        //Assert
        actual.Should().BeCloseTo(expected, MillisecondsAccountingFor3FirstPlaces);
    }
}