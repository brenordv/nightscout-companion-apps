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
            { Trend.NotComputable, "Wandering in the Unknown" },
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
            { Generators.ListWithFloats(HbA1CConstants.ReadingsIn1Day, 400f).ToList(), 15.56446f },
        };
    }

    public static TheoryData<CalculationData, float> ValidMageDataSets()
    {
        var dataSet = new TheoryData<CalculationData, float>();
        
        var glucoseReadings = new List<float>
        {
            70, 76, 82, 88, 94, 100, 106, 112, 118, 124, // Spike 1
            70, 76, 82, 88, 94, 100, 106, 112, 118, 124, // Spike 2
            70, 76, 82, 88, 94, 100, 106, 112, 118, 124, // Spike 3 
            70, 76, 82, 88, 94, 100, 106, 112, 118, 124, // Spike 4
            70, 76, 82, 88, 94, 100, 106, 112, 118, 124, // Spike 5
            70, 76, 82, 88, 94, 100, 106, 112, 118, 124, // Spike 6
            70, 76, 82, 88, 94, 100, 106, 112, 118, 124, // Spike 7
            70, 76, 82, 88, 94, 100, 106, 112, 118, 124, // Spike 8
            70, 76, 82, 88, 94, 100, 106, 112, 118, 124, // Spike 9
            70, 76, 82, 88, 94, 100, 106, 112, 118, 124  // Spike 10
        };
        var standardDev = TestUtils.CalculateStandardDeviation(glucoseReadings);
        var calculationData = new CalculationData
        {
            GlucoseValues = glucoseReadings,
            StandardDeviation = standardDev
        };

        
        dataSet.Add(calculationData, 54f);
        
        glucoseReadings = new List<float>
        {
            30, 62, 94, 126, 158, 190, 222, 254, 286, 318, // Spike 1
            30, 62, 94, 126, 158, 190, 222, 254, 286, 318, // Spike 2
            30, 62, 94, 126, 158, 190, 222, 254, 286, 318, // Spike 3
            30, 62, 94, 126, 158, 190, 222, 254, 286, 318, // Spike 4
            30, 62, 94, 126, 158, 190, 222, 254, 286, 318, // Spike 5
            30, 62, 94, 126, 158, 190, 222, 254, 286, 318, // Spike 6
            30, 62, 94, 126, 158, 190, 222, 254, 286, 318, // Spike 7
            30, 62, 94, 126, 158, 190, 222, 254, 286, 318, // Spike 8
            30, 62, 94, 126, 158, 190, 222, 254, 286, 318, // Spike 9
            30, 62, 94, 126, 158, 190, 222, 254, 286, 318 // Spike 10
        };
        standardDev = TestUtils.CalculateStandardDeviation(glucoseReadings);
        calculationData = new CalculationData
        {
            GlucoseValues = glucoseReadings,
            StandardDeviation = standardDev
        };
        
        dataSet.Add(calculationData, 288f);

        return dataSet;
    }

    public static TheoryData<CalculationData> GetInvalidMageCalculationData()
    {
        return new TheoryData<CalculationData>
        {
            new CalculationData
            {
                GlucoseValues = Generators.ListWithFloats(10).ToList()
            },
            new CalculationData
            {
                GlucoseValues = null,
                StandardDeviation = 0
            },
            new CalculationData
            {
                GlucoseValues = new List<float>(),
                StandardDeviation = 0
            },
        };
    }
}