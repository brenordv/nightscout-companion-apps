using FluentAssertions;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.ExtensionMethods;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.Domain.Core.Tests.ExtensionMethods;

public class ListExtensionsTests
{
    private static readonly DateOnly ReferenceDate = DateOnly.FromDateTime(DateTime.UtcNow);

    [Fact]
    public void HasElements_Should_Return_True_When_List_Has_Elements()
    {
        //Arrange
        var list = new List<string> {"a", "b", "c"};

        //Act
        var result = list.HasElements();

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasElements_Should_Return_False_When_List_Is_Null()
    {
        //Arrange
        List<string> list = null;

        //Act
        var result = list.HasElements();

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HasElements_Should_Return_False_When_List_Is_Empty()
    {
        //Arrange
        var list = new List<string>();

        //Act
        var result = list.HasElements();

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CalculateHbA1c_WhenListIsNull_ShouldReturnError()
    {
        // Arrange
        IEnumerable<GlucoseReading> list = null;

        // Act
        var result = list.CalculateHbA1C(ReferenceDate);

        // Assert
        result.Status.Should().Be(HbA1CCalculationStatus.Error);
        result.Error.Should().Be("No readings to calculate HbA1c");
    }

    [Fact]
    public void CalculateHbA1c_WhenListIsEmpty_ShouldReturnError()
    {
        // Arrange
        var list = new List<GlucoseReading>();

        // Act
        var result = list.CalculateHbA1C(ReferenceDate);

        // Assert
        result.Status.Should().Be(HbA1CCalculationStatus.Error);
        result.Error.Should().Be("No readings returned from Db to calculate HbA1c");
    }

    [Fact]
    public void CalculateHbA1c_WhenListHasMoreThanExpected_ShouldReturnError()
    {
        // Arrange
        var actualReadingCount = Constants.ReadingsIn115Days + 1;
        var readings = Generators.GlucoseReadingMockList(actualReadingCount, 100);

        // Act
        var result = readings.CalculateHbA1C(ReferenceDate);

        // Assert
        result.Status.Should().Be(HbA1CCalculationStatus.Error);
        result.Error.Should().Be($"Too many readings to calculate HbA1c reliably. Expected (max) 33120 but got {actualReadingCount}");
    }

    [Theory]
    [MemberData(nameof(TheoryGenerator.PartiallyValidHb1AcDataSets), MemberType = typeof(TheoryGenerator))]
    public void CalculateHbA1c_WhenListHasOneReading_ShouldReturnPartialSuccess(IList<GlucoseReading> readings, float expectedResult)
    {
        // Arrange & Act
        var result = readings.CalculateHbA1C(ReferenceDate);

        // Assert
        result.Status.Should().Be(HbA1CCalculationStatus.SuccessPartial);
        result.Value.Should().Be(expectedResult);
    }

    [Theory]
    [MemberData(nameof(TheoryGenerator.ValidHb1AcDataSets), MemberType = typeof(TheoryGenerator))]
    public void CalculateHbA1c_WhenListHasExactNumberOfReadings_ShouldReturnSuccess(IList<GlucoseReading> readings, float expectedResult)
    {
        // Arrange & Act
        var result = readings.CalculateHbA1C(ReferenceDate);

        // Assert
        result.Status.Should().Be(HbA1CCalculationStatus.Success);
        result.Value.Should().Be(expectedResult);
    }
}