using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

namespace Raccoon.Ninja.Domain.Core.Calculators.Handlers;

/// <summary>
///     MAGE (Mean Amplitude of Glycemic Excursions): MAGE is a measure of glycemic variability.
///     Used to quantify the major swings in glucose levels, particularly the peaks and troughs, over a period of time.
///     It's primarily used in diabetes management to assess the volatility or variability of blood glucose levels,
///     which is an important aspect beyond just the average blood glucose levels. High variability can be indicative
///     of greater risk of hypoglycemia, even if average glucose levels are within a target range. Understanding
///     and managing glucose variability can help in minimizing the risk of complications associated with diabetes.
///     Lower MAGE values = lower glycemic variability.
///     Higher MAGE values = higher glycemic variability.
///     Lower is better.
///     In here we're calculating the variability using as threshold 10% and 20% of the average glucose level + a
///     simplified calculation that don't use thresholds. This should provide a good overview of the glucose variability.
/// </summary>
public class MageCalculatorHandler : BaseCalculatorHandler
{
    protected override bool CanHandle(CalculationData data)
    {
        SetErrorMessage(
            "This calculation requires the list of glucose values of at least 3 readings, average, and standard deviation glucose values.");
        return base.CanHandle(data) &&
               data.GlucoseValues.Count > 2 &&
               data.Average > 0 &&
               data.StandardDeviation > 0;
    }

    protected override CalculationData RunCalculation(CalculationData data)
    {
        var glucoseLevels = data.GlucoseValues;
        var avg = data.Average;
        var t10 = data.Average * 0.1f;
        var t20 = data.Average * 0.2f;

        var (mageT10, detectedT10) = GetMageWithThreshold(glucoseLevels, avg, t10);
        var (mageT20, detectedT20) = GetMageWithThreshold(glucoseLevels, avg, t20);
        var mageSimple = CalculateSimpleMage(glucoseLevels, avg);

        return data with
        {
            Mage = new CalculationDataMage
            {
                Threshold10 = new CalculationDataMageResult { Value = mageT10, ExcursionsDetected = detectedT10 },
                Threshold20 = new CalculationDataMageResult { Value = mageT20, ExcursionsDetected = detectedT20 },
                Absolute = new CalculationDataMageResult { Value = mageSimple, ExcursionsDetected = true }
            }
        };
    }

    private static float CalculateSimpleMage(IList<float> glucoseLevels, float meanGlucose)
    {
        var deviations = glucoseLevels.Select(glucose => Math.Abs(glucose - meanGlucose)).ToList();
        var mage = deviations.Average();
        return mage;
    }

    private static (float mage, bool excursionDetected) GetMageWithThreshold(IList<float> glucoseLevels,
        float meanGlucose, float threshold)
    {
        var excursions = new List<float>();

        for (var i = 1; i < glucoseLevels.Count - 1; i++)
        {
            var prev = glucoseLevels[i - 1];
            var curr = glucoseLevels[i];
            var next = glucoseLevels[i + 1];

            if (curr > prev && curr > next && Math.Abs(curr - meanGlucose) > threshold)
                excursions.Add(curr);
            else if (curr < prev && curr < next && Math.Abs(curr - meanGlucose) > threshold) excursions.Add(curr);
        }

        if (excursions.Count < 2) return (0, false);

        var mage = excursions.Select(excursion => Math.Abs(excursion - meanGlucose)).Average();
        return (mage, true);
    }
}