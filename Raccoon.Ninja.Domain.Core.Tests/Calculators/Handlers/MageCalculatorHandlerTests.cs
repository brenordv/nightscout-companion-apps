using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Calculators.Handlers;
using Raccoon.Ninja.TestHelpers;

namespace Raccoon.Ninja.Domain.Core.Tests.Calculators.Handlers
{
    public class MageCalculatorHandlerTests
    {
        private readonly MageCalculatorHandler _sut = new();

        [Theory]
        [MemberData(nameof(TheoryGenerator.ValidMageDataSets), MemberType = typeof(TheoryGenerator))]
        public void Handle_DataHasSpikes_ShouldCalculateSuccessfully(CalculationData calculationData,
            float expectedResult)
        {
            //Act
            var result = _sut.Handle(calculationData);
            
            //Assert
            result.Status.Success.Should().BeTrue();
            result.Mage.Should().Be(expectedResult);
        }

        [Theory]
        [MemberData(nameof(TheoryGenerator.GetInvalidMageCalculationData), MemberType = typeof(TheoryGenerator))]
        public void Handle_DataHasZeroStandardDeviation_ShouldReturnCalculationDataWithErrorStatus(CalculationData calculationData)
        {
            // Act
            var result = _sut.Handle(calculationData);

            // Assert
            result.Status.Message.Should()
                .Be("This calculation requires the list of glucose readings, and standard deviation glucose values.");
        }

    }
}