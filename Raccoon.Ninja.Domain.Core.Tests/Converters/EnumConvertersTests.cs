using Raccoon.Ninja.NightScout.Core.Converters;
using Raccoon.Ninja.NightScout.Core.Enums;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.Domain.Core.Tests.Converters;

public class EnumConvertersTests
{
     [Theory]
     [MemberData(nameof(TheoryGenerator.AllTrendsWithExpectedStrings), MemberType = typeof(TheoryGenerator))]
     public void ToTrendString_Success(Trend trend, string expected)
     {
         // Arrange
         var actual = Converter.ToTrendString(trend);

         // Assert
         Assert.Equal(expected, actual);
     }
}
