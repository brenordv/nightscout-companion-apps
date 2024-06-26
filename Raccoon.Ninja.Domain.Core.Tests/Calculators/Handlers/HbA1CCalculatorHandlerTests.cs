using Raccoon.Ninja.Domain.Core.Calculators.Handlers;
using Raccoon.Ninja.Domain.Core.Constants;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.Domain.Core.Tests.Calculators.Handlers;

public class HbA1CCalculatorHandlerTests
{
    private readonly HbA1CCalculatorHandler _sut = new();

    [Theory]
    [MemberData(nameof(TheoryGenerator.InvalidFloatListsWithNull), MemberType = typeof(TheoryGenerator))]
    public void Handle_WhenListIsInvalid_ShouldReturnError(IList<float> glucoseValues)
    {
        // Act
        var result = _sut.Handle(Generators.CalculationDataMockSingle(glucoseValues));

        // Assert
        var status = result.Status;
        status.Success.Should().BeFalse();
        status.FailedAtStep.Should().Be(nameof(HbA1CCalculatorHandler));
        status.Message.Should().Be("This calculation requires a valid average glucose value.");
    }

    [Fact]
    public void Handle_WhenListHasMoreThanExpected_ShouldReturnError()
    {
        // Arrange
        const int actualReadingCount = HbA1CConstants.ReadingsIn115Days + 1;

        var glucoseValues = Generators.ListWithFloats(actualReadingCount, 100f).ToList();

        // Act
        var result = _sut.Handle(Generators.CalculationDataMockSingle(glucoseValues));

        // Assert
        var status = result.Status;
        status.Success.Should().BeFalse();
        status.FailedAtStep.Should().Be(nameof(HbA1CCalculatorHandler));
        status.Message.Should()
            .Be($"Too many readings to calculate HbA1c reliably. Expected (max) 33120 but got {actualReadingCount}");
    }

    [Theory]
    [MemberData(nameof(TheoryGenerator.PartiallyValidHb1AcDataSets), MemberType = typeof(TheoryGenerator))]
    public void Handle_WhenListHasLessThanExpectedReadings_ShouldReturnPartialSuccess(IList<float> glucoseValues,
        float expectedResult)
    {
        // Act
        var result = _sut.Handle(Generators.CalculationDataMockSingle(glucoseValues));

        // Assert
        result.Status.Success.Should().BeTrue();
        result.CurrentHbA1C.Should().NotBeNull();
        result.CurrentHbA1C.Value.Should().Be(expectedResult);
        result.CurrentHbA1C.Status.Should().Be(HbA1CCalculationStatus.Partial);
    }

    [Theory]
    [MemberData(nameof(TheoryGenerator.ValidHb1AcDataSets), MemberType = typeof(TheoryGenerator))]
    public void CalculateHbA1c_WhenListHasExactNumberOfReadings_ShouldReturnSuccess(IList<float> glucoseValues,
        float expectedResult)
    {
        // Act
        var result = _sut.Handle(Generators.CalculationDataMockSingle(glucoseValues));

        // Assert
        result.Status.Success.Should().BeTrue();
        result.CurrentHbA1C.Should().NotBeNull();
        result.CurrentHbA1C.Value.Should().Be(expectedResult);
        result.CurrentHbA1C.Status.Should().Be(HbA1CCalculationStatus.Complete);
    }
}