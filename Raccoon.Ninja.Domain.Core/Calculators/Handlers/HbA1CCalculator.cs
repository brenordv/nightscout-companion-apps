using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;
using Raccoon.Ninja.Domain.Core.Constants;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.Domain.Core.Calculators.Handlers;

/// <summary>
///  HbA1C: The HbA1C test measures the average blood glucose level over the past 2-3 months.
///  It is used to monitor the effectiveness of diabetes treatment.
/// </summary>
public class HbA1CCalculator: BaseCalculatorHandler
{
    private const float GlucoseConversionFactor = 46.7f;
    private const float HbA1CDivisor = 28.7f;

    public HbA1CCalculator(BaseCalculatorHandler nextHandler) : base(nextHandler)
    {
    }

    protected override bool CanHandle(CalculationData data)
    {
        return data.Average > 0 && data.Count is > 0 and <= HbA1CConstants.ReadingsIn115Days;
    }

    protected override CalculationData HandleError(CalculationData data)
    {
        return data with
        {
            Status = new CalculationDataStatus
            {
                Success = false,
                FirstFailedStep = nameof(HbA1CCalculator),
                Message = data.Count <= HbA1CConstants.ReadingsIn115Days 
                    ? "This calculation requires a valid average glucose value." 
                    : $"Too many readings to calculate HbA1c reliably. Expected (max) {HbA1CConstants.ReadingsIn115Days} but got {data.Count}",
            }
        };
    }

    public override CalculationData Handle(CalculationData data)
    {
        if (!CanHandle(data))
        {
            return HandleError(data);
        }

        var average = data.Average;

        var hba1C = (average + GlucoseConversionFactor) / HbA1CDivisor;

        return HandleNext(data with
        {
            CurrentHbA1C = new HbA1CCalculation
            {
                Value = hba1C,
                ReferenceDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Delta = hba1C - data.PreviousHbA1C?.Value, // Null if data.PreviousHbA1C is null
                Status = GetStatusByReadingCount(data.Count)
            }
        });
    }

    private static HbA1CCalculationStatus GetStatusByReadingCount(int count)
    {
        return count == HbA1CConstants.ReadingsIn115Days 
            ? HbA1CCalculationStatus.Success 
            : HbA1CCalculationStatus.SuccessPartial;
    }
}