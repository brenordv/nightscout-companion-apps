using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Calculators.Handlers;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.Domain.Core.Tests.Calculators.Handlers;

public class MageCalculatorHandlerTests
{
    private readonly MageCalculatorHandler _sut = new();

    [Theory]
    [MemberData(nameof(TheoryGenerator.ValidMageDataSets), MemberType = typeof(TheoryGenerator))]
    public void Handle_DataHasSpikes_ShouldCalculateSuccessfully(CalculationData calculationData,
        CalculationDataMage expectedResult)
    {
        //Act
        var result = _sut.Handle(calculationData);

        //Assert
        result.Status.Success.Should().BeTrue();
        result.Mage.Should().NotBeNull();
        result.Mage.Threshold10.Should().NotBeNull();
        result.Mage.Threshold20.Should().NotBeNull();
        result.Mage.Absolute.Should().NotBeNull();
        result.Mage.Threshold10.ExcursionsDetected.Should().Be(expectedResult.Threshold10.ExcursionsDetected);
        result.Mage.Threshold20.ExcursionsDetected.Should().Be(expectedResult.Threshold20.ExcursionsDetected);
        result.Mage.Absolute.ExcursionsDetected.Should().Be(expectedResult.Absolute.ExcursionsDetected);
        result.Mage.Threshold10.Value.Should().BeApproximately(expectedResult.Threshold10.Value, 0.0001f);
        result.Mage.Threshold20.Value.Should().BeApproximately(expectedResult.Threshold20.Value, 0.0001f);
        result.Mage.Absolute.Value.Should().BeApproximately(expectedResult.Absolute.Value, 0.0001f);
    }

    [Theory]
    [MemberData(nameof(TheoryGenerator.GetInvalidMageCalculationData), MemberType = typeof(TheoryGenerator))]
    public void Handle_DataHasZeroStandardDeviation_ShouldReturnCalculationDataWithErrorStatus(
        CalculationData calculationData)
    {
        // Act
        var result = _sut.Handle(calculationData);

        // Assert
        result.Status.Message.Should()
            .Be(
                "This calculation requires the list of glucose values of at least 3 readings, average, and standard deviation glucose values.");
    }
}