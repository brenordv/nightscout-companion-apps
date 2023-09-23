using FluentAssertions;
using Raccoon.Ninja.Domain.Core.Converters;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.Domain.Core.Tests.Converters;

public class EnumConvertersTests
{
     [Theory]
     [MemberData(nameof(TheoryGenerator.AllTrendsWithExpectedStrings), MemberType = typeof(TheoryGenerator))]
     public void ToTrendString_Success(Trend trend, string expected)
     {
         //Arrange
         var actual = Converter.ToTrendString(trend);
         
         //Assert
         actual.Should().Be(expected);
     }
}