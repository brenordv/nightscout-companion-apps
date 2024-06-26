using Raccoon.Ninja.Domain.Core.Calculators.Handlers;

namespace Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

/// <summary>
///     Base class to handle the calculation of a specific metric.
///     The overall result will be converted do a CosmosDb document
///     and stored in the aggregation collection.
/// </summary>
/// <remarks>
///     Why not use a simple function that calculates everything at once?
///     - The idea is to have a pipeline of handlers, each one responsible for a specific calculation.
///     - This way, we can easily add new calculations without changing the main class.
///     - Also, we can easily test each calculation separately.
/// </remarks>
public abstract class BaseCalculatorHandler
{
    private string _errorMessage = "No glucose values were provided.";
    private BaseCalculatorHandler _nextHandler;

    private CalculationData HandleNext(CalculationData data)
    {
        return _nextHandler is null
            ? data
            : _nextHandler.Handle(data);
    }

    protected void SetErrorMessage(string message)
    {
        _errorMessage = message;
    }

    protected virtual bool CanHandle(CalculationData data)
    {
        return data.GlucoseValues is not null && data.GlucoseValues.Count > 0;
    }

    protected CalculationData HandleError(CalculationData data)
    {
        return data with
        {
            Status = new CalculationDataStatus
            {
                Message = _errorMessage,
                Success = false,
                FailedAtStep = GetType().Name
            }
        };
    }

    protected abstract CalculationData RunCalculation(CalculationData data);

    public void SetNextHandler(BaseCalculatorHandler nextHandler)
    {
        _nextHandler = nextHandler;
    }

    public CalculationData Handle(CalculationData data)
    {
        if (!CanHandle(data)) return HandleError(data);

        var calculationResult = RunCalculation(data);

        return HandleNext(calculationResult);
    }

    /// <summary>
    ///     Build the default chain of calculations.
    /// </summary>
    /// <returns>First link in the chain.</returns>
    public static BaseCalculatorHandler BuildChain()
    {
        //First step (link) of the chain
        var avgCalculator = new AverageCalculatorHandler();
        var medianCalculator = new MedianCalculatorHandler();
        var rangeCalculator = new RangeCalculatorHandler(); //Min + Max values
        var sdCalculator = new StandardDeviationCalculatorHandler();
        var cvCalculator = new CoefficientOfVariationCalculatorHandler();
        var mageCalculator = new MageCalculatorHandler();
        var hbA1CCalculator = new HbA1CCalculatorHandler();
        var tirCalculator = new TimeInRangeCalculatorHandler();
        var percentileCalculator = new PercentileCalculatorHandler();
        // Last step (link) of the chain

        // Chaining the links (first to last). They don't have to be instantiated in order,
        // but it helps keep track when we're chaining the execution. I could have created
        // an auto-link method, but it would just made everything unnecessarily complex for
        // this use case.
        avgCalculator.SetNextHandler(medianCalculator);
        medianCalculator.SetNextHandler(rangeCalculator);
        rangeCalculator.SetNextHandler(sdCalculator);
        sdCalculator.SetNextHandler(cvCalculator);
        cvCalculator.SetNextHandler(mageCalculator);
        mageCalculator.SetNextHandler(hbA1CCalculator);
        hbA1CCalculator.SetNextHandler(tirCalculator);
        tirCalculator.SetNextHandler(percentileCalculator);

        // Returning first link of the chain, that has a reference to the rest.
        return avgCalculator;
    }
}