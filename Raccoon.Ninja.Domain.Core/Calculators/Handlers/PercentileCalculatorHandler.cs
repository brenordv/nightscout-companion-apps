using Raccoon.Ninja.Domain.Core.Calculators.Abstractions;

namespace Raccoon.Ninja.Domain.Core.Calculators.Handlers;

/// <summary>
/// Percentiles (e.g., 10th, 25th, 75th, 90th) provide another way to understand the distribution of
/// glucose values beyond the median and mean, showing how glucose levels distribute across different
/// points of the data set.
///
/// 10th Percentile: Lower Bound of Glucose Variability: Helps identify hypoglycemia risk by showing
/// the glucose level below which only 10% of readings fall.
///
/// 25th Percentile (Q1): Marks the bottom end of the middle 50% of readings, providing insight into
/// lower glucose levels but above the very low extremes.
///
/// 75th Percentile (Q3): Upper Quartile represents the top end of the middle 50% of readings,
/// giving an indication of higher glucose levels but excluding the highest extremes.
///
/// 90th Percentile: Upper Bound of Glucose Variability: Helps identify hyperglycemia risk by showing
/// the glucose level above which only 10% of readings fall.
///
/// Interquartile Range (IQR): IQR measures the spread of the middle 50% of values. It's useful for
/// understanding the variability of glucose levels while being less sensitive to outliers than the
/// range.
/// </summary>
public class PercentileCalculatorHandler: BaseCalculatorHandler
{
    protected override CalculationData RunCalculation(CalculationData data)
    {
        var p10 = CalculatePercentile(data.GlucoseValues, 10);
        var p25 = CalculatePercentile(data.GlucoseValues, 25);
        var p75 = CalculatePercentile(data.GlucoseValues, 75);
        var p90 = CalculatePercentile(data.GlucoseValues, 90);

        return data with
        {
            Percentile = new CalculationDataPercentile
            {
                P10 = p10,
                P25 = p25,
                P75 = p75,
                P90 = p90,
                Iqr = p75 - p25
            }
        };
    }

    private static float CalculatePercentile(IList<float> values, float percentile)
    {
        var n = (int)Math.Ceiling((percentile / 100.0) * values.Count) - 1;
        return values[n];
    }
}