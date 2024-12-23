﻿using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.Domain.Core.Tests.Entities;

public class GlucoseReadingTests
{
    [Fact]
    public void Ctor_WhenInstantiated_ShouldHaveDefaultPropertyValues()
    {
        //Arrange
        const Trend expectedTrend = Trend.TripleUp;
        const long expectedReadTimestampUtc = 0;

        //Act
        var sut = new GlucoseReading();

        //Assert
        sut.Should().NotBeNull();
        sut.Trend.Should().Be(expectedTrend);
        sut.ReadTimestampUtc.Should().Be(expectedReadTimestampUtc);
    }
}