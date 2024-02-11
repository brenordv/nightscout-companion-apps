using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;
using Xunit;

namespace Raccoon.Ninja.TestHelpers;

public static class TheoryGenerator
{
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

    public static TheoryData<IList<GlucoseReading>, float> ValidHb1AcDataSets()
    {
        var data = new TheoryData<IList<GlucoseReading>, float>
        {
            { Generators.GlucoseReadingMockList(Constants.ReadingsIn115Days, 80), 4.4146338f },
            { Generators.GlucoseReadingMockList(Constants.ReadingsIn115Days, 100), 5.111498f },
            { Generators.GlucoseReadingMockList(Constants.ReadingsIn115Days, 150), 6.853658f },
            { Generators.GlucoseReadingMockList(Constants.ReadingsIn115Days, 170), 7.5505223f },
            { Generators.GlucoseReadingMockList(Constants.ReadingsIn115Days, 210), 8.944251f },
            { Generators.GlucoseReadingMockList(Constants.ReadingsIn115Days, 300), 12.080139f },
            { Generators.GlucoseReadingMockList(Constants.ReadingsIn115Days, 400), 15.56446f }
        };

        return data;
    }

    public static TheoryData<IList<GlucoseReading>, float> PartiallyValidHb1AcDataSets()
    {
        return new TheoryData<IList<GlucoseReading>, float>
        {
            { Generators.GlucoseReadingMockList(Constants.ReadingsIn1Day, 80), 4.4146338f },
            { Generators.GlucoseReadingMockList(Constants.ReadingsIn1Day, 100), 5.111498f },
            { Generators.GlucoseReadingMockList(Constants.ReadingsIn1Day, 150), 6.853658f },
            { Generators.GlucoseReadingMockList(Constants.ReadingsIn1Day, 170), 7.5505223f },
            { Generators.GlucoseReadingMockList(Constants.ReadingsIn1Day, 210), 8.944251f },
            { Generators.GlucoseReadingMockList(Constants.ReadingsIn1Day, 300), 12.080139f },
            { Generators.GlucoseReadingMockList(Constants.ReadingsIn1Day, 400), 15.56446f },
        };
    }
}