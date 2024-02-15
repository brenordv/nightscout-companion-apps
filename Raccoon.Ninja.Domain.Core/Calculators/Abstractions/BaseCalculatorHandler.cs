using Raccoon.Ninja.Domain.Core.Calculators.Handlers;

namespace Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

/// <summary>
/// Base class to handle the calculation of a specific metric.
/// The overall result will be converted do a CosmosDb document
/// and stored in the aggregation collection.
/// </summary>
/// <remarks>
/// Why not use a simple function that calculates everything at once?
///  - The idea is to have a pipeline of handlers, each one responsible for a specific calculation.
///  - This way, we can easily add new calculations without changing the main class.
///  - Also, we can easily test each calculation separately.
/// </remarks>
public abstract class BaseCalculatorHandler
{
    private readonly BaseCalculatorHandler _nextHandler;

    protected BaseCalculatorHandler(BaseCalculatorHandler nextHandler)
    {
        _nextHandler = nextHandler;
    }

    protected virtual bool CanHandle(CalculationData data)
    {
        return data.GlucoseValues is not null && data.GlucoseValues.Count > 0;
    }

    protected virtual CalculationData HandleError(CalculationData data)
    {
        return data with
        {
            Status = new CalculationDataStatus
            {
                Message = "No glucose values were provided.",
                Success = false,
                FirstFailedStep = GetType().Name
            }
        };
    }

    protected CalculationData HandleNext(CalculationData data)
    {
        return _nextHandler is null 
            ? data 
            : _nextHandler.Handle(data);
    }

    public abstract CalculationData Handle(CalculationData data);

    /// <summary>
    /// Build the default chain of calculations.
    /// </summary>
    /// <returns>First link in the chain.</returns>
    public static BaseCalculatorHandler BuildChain()
    {
        // Last step of the chain
        var mageCalculator = new MageCalculator(null);
        var tirCalculator = new TimeInRangeCalculator(mageCalculator);
        var hbA1CCalculator = new HbA1CCalculator(tirCalculator);
        var glucoseVariabilityCalculator = new RangeCalculator(hbA1CCalculator);
        var medianCalculator = new MedianCalculator(glucoseVariabilityCalculator);
        var percentileCalculator = new PercentileCalculator(medianCalculator);
        var sdCalculator = new StandardDeviationCalculator(percentileCalculator);
        var cvCalculator = new CoefficientOfVariationCalculator(sdCalculator);
        var avgCalculator = new AverageCalculator(cvCalculator);
        //First step of the chain

        return avgCalculator;
    }
}