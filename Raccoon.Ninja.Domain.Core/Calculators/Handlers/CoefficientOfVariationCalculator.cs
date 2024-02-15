﻿using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

namespace Raccoon.Ninja.Domain.Core.Calculators.Handlers;

/// <summary> 
/// Coefficient of Variation (CV) is a standardized measure of dispersion of a probability
/// distribution or frequency distribution. It's particularly useful because it allows for
/// comparison of variability across different mean glucose levels.
/// </summary>
public class CoefficientOfVariationCalculator: BaseCalculatorHandler
{
    public CoefficientOfVariationCalculator(BaseCalculatorHandler nextHandler) : base(nextHandler)
    {
    }

    protected override bool CanHandle(CalculationData data)
    {
        return data.StandardDeviation > 0 && data.Average > 0;
    }

    protected override CalculationData HandleError(CalculationData data)
    {
        return data with
        {
            Status = new CalculationDataStatus
            {
                Success = false,
                Message = "Cannot calculate Coefficient of Variation without Standard Deviation and Average.",
                FirstFailedStep = nameof(CoefficientOfVariationCalculator)
            }
        };
    }

    public override CalculationData Handle(CalculationData data)
    {
        if (!CanHandle(data))
        {
            return HandleError(data);
        }
        
        return HandleNext(data with
        {
            CoefficientOfVariation = data.StandardDeviation / data.Average
        });
    }
}