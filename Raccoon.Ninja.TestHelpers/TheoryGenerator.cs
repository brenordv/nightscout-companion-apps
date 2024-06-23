using Raccoon.Ninja.Domain.Core.Calculators;
using Raccoon.Ninja.Domain.Core.Constants;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;
using Xunit;

namespace Raccoon.Ninja.TestHelpers;

public static class TheoryGenerator
{
    public static TheoryData<IList<float>> InvalidFloatListsWithNull()
    {
        return new TheoryData<IList<float>>
        {
            null,
            new List<float>()
        };
    }

    public static TheoryData<Trend, string> AllTrendsWithExpectedStrings()
    {
        return new TheoryData<Trend, string>
        {
            { Trend.TripleUp, "Zooming Skyward" },
            { Trend.DoubleUp, "Rapid Ascent" },
            { Trend.SingleUp, "Steep Climb" },
            { Trend.FortyFiveUp, "Going Up, Captain" },
            { Trend.Flat, "Nice and easy" },
            { Trend.FortyFiveDown, "Descending Swiftly" },
            { Trend.SingleDown, "Plummet Mode" },
            { Trend.DoubleDown, "Double the plunge" },
            { Trend.TripleDown, "Free-fall Frenzy" },
            { Trend.NotComputable, "Wandering in the Unknown" }
        };
    }

    public static TheoryData<Trend, string> VariantStringsWithCorrespondingTrend()
    {
        var data = new TheoryData<Trend, string>();

        foreach (var valueLabelPair in AllTrendsWithExpectedStrings())
        {
            var trend = (Trend)valueLabelPair[0];
            var label = (string)valueLabelPair[1];

            data.Add(trend, label);
            data.Add(trend, label.ToLowerInvariant());
            data.Add(trend, label.ToUpperInvariant());
        }

        return data;
    }

    public static TheoryData<long, DateTime> TimeStampAndCorrespondingDateTimes()
    {
        var data = new TheoryData<long, DateTime>();

        var testDate = DateTime.MinValue.ToUniversalTime();
        data.Add(testDate.ToUnixTimestamp(), testDate);

        testDate = DateTime.MaxValue.ToUniversalTime();
        data.Add(testDate.ToUnixTimestamp(), testDate);

        testDate = DateTime.UtcNow;
        data.Add(testDate.ToUnixTimestamp(), testDate);

        //Leap year day test.
        testDate = new DateTime(2020, 2, 29, 23, 59, 59, DateTimeKind.Utc);
        data.Add(testDate.ToUnixTimestamp(), testDate);

        return data;
    }

    public static TheoryData<IList<float>, float> ValidHb1AcDataSets()
    {
        var data = new TheoryData<IList<float>, float>
        {
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn115Days, 80f).ToList(), 4.4146338f },
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn115Days, 100f).ToList(), 5.111498f },
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn115Days, 150f).ToList(), 6.853658f },
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn115Days, 170f).ToList(), 7.5505223f },
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn115Days, 210f).ToList(), 8.944251f },
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn115Days, 300f).ToList(), 12.080139f },
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn115Days, 400f).ToList(), 15.56446f }
        };

        return data;
    }

    public static TheoryData<IList<float>, float> PartiallyValidHb1AcDataSets()
    {
        return new TheoryData<IList<float>, float>
        {
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn1Day, 80f).ToList(), 4.4146338f },
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn1Day, 100f).ToList(), 5.111498f },
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn1Day, 150f).ToList(), 6.853658f },
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn1Day, 170f).ToList(), 7.5505223f },
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn1Day, 210f).ToList(), 8.944251f },
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn1Day, 300f).ToList(), 12.080139f },
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn1Day, 400f).ToList(), 15.56446f }
        };
    }

    public static TheoryData<CalculationData, CalculationDataMage> ValidMageDataSets()
    {
        var dataSet = new TheoryData<CalculationData, CalculationDataMage>();

        // var glucoseReadings = new List<float> { 50, 55, 60, 58, 62, 65, 63, 70, 75, 80 };
        // var standardDev = TestUtils.CalculateStandardDeviation(glucoseReadings);
        //
        // var calculationData = new CalculationData
        // {
        //     GlucoseValues = glucoseReadings,
        //     StandardDeviation = standardDev,
        //     Average = glucoseReadings.Average()
        // };
        //
        // dataSet.Add(calculationData, new CalculationDataMage
        // {
        //     Threshold10 = new CalculationDataMageResult
        //     {
        //         Value = 15.0f,
        //         ExcursionsDetected = true
        //     },
        //     Threshold20 = new CalculationDataMageResult
        //     {
        //         Value = 17.5f,
        //         ExcursionsDetected = true
        //     },
        //     Threshold30 = new CalculationDataMageResult
        //     {
        //         Value = 20.0f,
        //         ExcursionsDetected = true
        //     },
        //     Absolute = new CalculationDataMageResult
        //     {
        //         Value = 10.5f,
        //         ExcursionsDetected = true
        //     }
        // });

        var glucoseReadings = new List<float> { 100, 110, 90, 105, 95, 120, 85, 130, 80, 115 };
        var standardDev = TestUtils.CalculateStandardDeviation(glucoseReadings);
        var calculationData = new CalculationData
        {
            GlucoseValues = glucoseReadings,
            StandardDeviation = standardDev,
            Average = glucoseReadings.Average()
        };

        dataSet.Add(calculationData, new CalculationDataMage
        {
            Threshold10 = new CalculationDataMageResult
            {
                Value = 19.6f,
                ExcursionsDetected = true
            },
            Threshold20 = new CalculationDataMageResult
            {
                Value = 25.0f,
                ExcursionsDetected = true
            },
            Absolute = new CalculationDataMageResult
            {
                Value = 13.0f,
                ExcursionsDetected = true
            }
        });

        return dataSet;
    }

    public static TheoryData<CalculationData> GetInvalidMageCalculationData()
    {
        return new TheoryData<CalculationData>
        {
            new CalculationData
            {
                GlucoseValues = new List<float>(),
                StandardDeviation = 100.0f,
                Average = 10.0f
            },
            new CalculationData
            {
                GlucoseValues = Generators.ListWithFloats(1).ToList(),
                StandardDeviation = 100.0f,
                Average = 10.0f
            },
            new CalculationData
            {
                GlucoseValues = Generators.ListWithFloats(2).ToList(),
                StandardDeviation = 100.0f,
                Average = 10.0f
            },
            new CalculationData
            {
                GlucoseValues = null,
                StandardDeviation = 0,
                Average = 0
            },
            new CalculationData
            {
                GlucoseValues = new List<float>(),
                StandardDeviation = 0,
                Average = 0
            },
            new CalculationData
            {
                GlucoseValues = Generators.ListWithFloats(20).ToList(),
                StandardDeviation = 100.0f,
                Average = 0
            },
            new CalculationData
            {
                GlucoseValues = Generators.ListWithFloats(20).ToList(),
                StandardDeviation = 0,
                Average = 10.0f
            }
        };
    }

    public static TheoryData<IList<GlucoseReading>, IList<float>> GetUnsortedAndExpectedSortedReadings()
    {
        var data = new TheoryData<IList<GlucoseReading>, IList<float>>();

        var unsortedReadings = new List<GlucoseReading>
        {
            new() { Value = 100 },
            new() { Value = 90 },
            new() { Value = 110 },
            new() { Value = 80 },
            new() { Value = 120 },
            new() { Value = 70 },
            new() { Value = 130 },
            new() { Value = 60 },
            new() { Value = 140 },
            new() { Value = 50 }
        };

        var sortedReadings = new List<float>
        {
            50, 60, 70, 80, 90, 100, 110, 120, 130, 140
        };

        data.Add(unsortedReadings, sortedReadings);

        return data;
    }
}