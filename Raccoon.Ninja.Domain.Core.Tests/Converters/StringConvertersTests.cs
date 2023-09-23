using FluentAssertions;
using Raccoon.Ninja.Domain.Core.Converters;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.Domain.Core.Tests.Converters;

public class StringConvertersTests
{
    [Theory]
    [MemberData(nameof(TheoryGenerator.VariantStringsWithCorrespondingTrend), MemberType = typeof(TheoryGenerator))]
    public void ToTrend_Success(string label, Trend expected)
    {
        //Arrange
        var actual = Converter.ToTrend(label);

        //Assert
        actual.Should().Be(expected);
    }
}