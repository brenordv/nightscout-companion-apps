using FluentAssertions;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.Domain.Core.Tests.ExtensionMethods;

public class LongExtensionsTests
{
    private static readonly TimeSpan MillisecondsAccountingFor3FirstPlaces = TimeSpan.FromMilliseconds(100);
    private static readonly DateOnly ReferenceDate = DateOnly.FromDateTime(DateTime.UtcNow);
    
    [Theory]
    [MemberData(nameof(TheoryGenerator.TimeStampAndCorrespondingDateTimes), MemberType = typeof(TheoryGenerator))]
    public void ToUtcDateTime_Success(long timestamp, DateTime expected)
    {
        //Arrange
        var actual = timestamp.ToUtcDateTime();
        
        //Assert
        actual.Should().BeCloseTo(expected, MillisecondsAccountingFor3FirstPlaces);
    }
    
    [Fact]
    public void CalculateHbA1c_WhenListIsNull_ShouldReturnError()
    {
        // Arrange
        IEnumerable<GlucoseReading> list = null;

        // Act
        var result = list.CalculateHbA1c(ReferenceDate);

        // Assert
        result.Status.Should().Be(HbA1cCalculationStatus.Error);
        result.Error.Should().Be("No readings to calculate HbA1c");
    }

    [Fact]
    public void CalculateHbA1c_WhenListIsEmpty_ShouldReturnError()
    {
        // Arrange
        var list = new List<GlucoseReading>();

        // Act
        var result = list.CalculateHbA1c(ReferenceDate);

        // Assert
        result.Status.Should().Be(HbA1cCalculationStatus.Error);
        result.Error.Should().Be("No readings returned from Db to calculate HbA1c");
    }

    [Fact]
    public void CalculateHbA1c_WhenListHasMoreThanExpected_ShouldReturnError()
    {
        // Arrange
        var actualReadingCount = Constants.ReadingsIn115Days + 1;
        var readings = Generators.GenerateList(actualReadingCount, 100);

        // Act
        var result = readings.CalculateHbA1c(ReferenceDate);

        // Assert
        result.Status.Should().Be(HbA1cCalculationStatus.Error);
        result.Error.Should().Be($"Too many readings to calculate HbA1c reliably. Expected 33120 but got {actualReadingCount}");
    }
    
    [Theory]
    [MemberData(nameof(TheoryGenerator.PartiallyValidHb1AcDataSets), MemberType = typeof(TheoryGenerator))]
    public void CalculateHbA1c_WhenListHasOneReading_ShouldReturnPartialSuccess(List<GlucoseReading> readings, float expectedResult)
    {
        // Arrange & Act
        var result = readings.CalculateHbA1c(ReferenceDate);

        // Assert
        result.Status.Should().Be(HbA1cCalculationStatus.SuccessPartial);
        result.Value.Should().Be(expectedResult);
    }
   
    [Theory]
    [MemberData(nameof(TheoryGenerator.ValidHb1AcDataSets), MemberType = typeof(TheoryGenerator))]
    public void CalculateHbA1c_WhenListHasExactNumberOfReadings_ShouldReturnSuccess(List<GlucoseReading> readings, float expectedResult)
    {
        // Arrange & Act
        var result = readings.CalculateHbA1c(ReferenceDate);

        // Assert
        result.Status.Should().Be(HbA1cCalculationStatus.Success);
        result.Value.Should().Be(expectedResult);
    }
}