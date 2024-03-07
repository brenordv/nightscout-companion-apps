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
                FirstFailedStep = GetType().Name
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
        if (!CanHandle(data))
        {
            return HandleError(data);
        }

        var calculationResult = RunCalculation(data);

        return HandleNext(calculationResult);
    }

    /// <summary>
    /// Build the default chain of calculations.
    /// </summary>
    /// <returns>First link in the chain.</returns>
    public static BaseCalculatorHandler BuildChain()
    {
        //First step (link) of the chain
        var avgCalculator = new AverageCalculatorHandler();
        var cvCalculator = new CoefficientOfVariationCalculatorHandler();
        var sdCalculator = new StandardDeviationCalculatorHandler();
        var percentileCalculator = new PercentileCalculatorHandler();
        var medianCalculator = new MedianCalculatorHandler();
        var glucoseVariabilityCalculator = new RangeCalculatorHandler();
        var hbA1CCalculator = new HbA1CCalculatorHandler();
        var tirCalculator = new TimeInRangeCalculatorHandler();        
        var mageCalculator = new MageCalculatorHandler();
        // Last step (link) of the chain
        
        // Chaining the links
        avgCalculator.SetNextHandler(cvCalculator);
        cvCalculator.SetNextHandler(sdCalculator);
        sdCalculator.SetNextHandler(percentileCalculator);
        percentileCalculator.SetNextHandler(medianCalculator);
        medianCalculator.SetNextHandler(glucoseVariabilityCalculator);
        glucoseVariabilityCalculator.SetNextHandler(hbA1CCalculator);
        hbA1CCalculator.SetNextHandler(tirCalculator);
        tirCalculator.SetNextHandler(mageCalculator);
        
        // Returning first link of the chain, that has a reference to the rest.
        return avgCalculator;
    }
}