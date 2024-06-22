using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;
using Raccoon.Ninja.Domain.Core.Constants;
using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.Domain.Core.Calculators.Handlers;

/// <summary>
///     HbA1C: The HbA1C test measures the average blood glucose level over the past 2-3 months.
///     It is used to monitor the effectiveness of diabetes treatment.
/// </summary>
public class HbA1CCalculatorHandler : BaseCalculatorHandler
{
    private const float GlucoseConversionFactor = 46.7f;
    private const float HbA1CDivisor = 28.7f;

    protected override bool CanHandle(CalculationData data)
    {
        SetErrorMessage(data.Count <= HbA1CConstants.ReadingsIn115Days
            ? "This calculation requires a valid average glucose value."
            : $"Too many readings to calculate HbA1c reliably. Expected (max) {HbA1CConstants.ReadingsIn115Days} but got {data.Count}");
        return data.Average > 0 && data.Count is > 0 and <= HbA1CConstants.ReadingsIn115Days;
    }

    protected override CalculationData RunCalculation(CalculationData data)
    {
        var average = data.Average;

        var hba1C = (average + GlucoseConversionFactor) / HbA1CDivisor;

        return data with
        {
            CurrentHbA1C = new CalculationDataHbA1C
            {
                Value = hba1C,
                Status = GetStatusByReadingCount(data.Count)
            }
        };
    }

    private static HbA1CCalculationStatus GetStatusByReadingCount(int count)
    {
        return count == HbA1CConstants.ReadingsIn115Days
            ? HbA1CCalculationStatus.Complete
            : HbA1CCalculationStatus.Partial;
    }
}