using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

namespace Raccoon.Ninja.Domain.Core.Calculators.Handlers;

/// <summary>
/// MAGE (Mean Amplitude of Glycemic Excursions): MAGE is a measure of glycemic variability.
/// Used to quantify the major swings in glucose levels, particularly the peaks and troughs, over a period of time.
/// It's primarily used in diabetes management to assess the volatility or variability of blood glucose levels,
/// which is an important aspect beyond just the average blood glucose levels. High variability can be indicative
/// of greater risk of hypoglycemia, even if average glucose levels are within a target range. Understanding
/// and managing glucose variability can help in minimizing the risk of complications associated with diabetes.
///
/// Lower MAGE values = lower glycemic variability.
/// Higher MAGE values = higher glycemic variability.
///
/// Lower is better.
/// </summary>
public class MageCalculatorHandler: BaseCalculatorHandler
{
    protected override bool CanHandle(CalculationData data)
    {
        SetErrorMessage("This calculation requires the list of glucose readings, and standard deviation glucose values.");
        return base.CanHandle(data) && data.StandardDeviation > 0;
    }

    protected override CalculationData RunCalculation(CalculationData data)
    {
        var standardDeviation = data.StandardDeviation;
        var glucoseReadings = data.GlucoseValues;

        // Identify peaks and troughs
        var excursions = new List<float>();
        for (var i = 1; i < glucoseReadings.Count - 1; i++)
        {
            if ((glucoseReadings[i] > glucoseReadings[i - 1] && glucoseReadings[i] > glucoseReadings[i + 1]) ||
                (glucoseReadings[i] < glucoseReadings[i - 1] && glucoseReadings[i] < glucoseReadings[i + 1]))
            {
                excursions.Add(glucoseReadings[i]);
            }
        }

        // Calculate amplitudes and filter by significant excursions
        var significantExcursions = new List<float>();
        for (var i = 0; i < excursions.Count - 1; i++)
        {
            var amplitude = Math.Abs(excursions[i + 1] - excursions[i]);
            if (amplitude > standardDeviation)
            {
                significantExcursions.Add(amplitude);
            }
        }

        var mage = significantExcursions.Count > 0
            ? significantExcursions.Average()
            : 0;

        return data with { Mage = mage };
    }
}