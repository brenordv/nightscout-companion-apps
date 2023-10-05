using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.TestHelpers;

public static class TheoryGenerator
{


    public static IEnumerable<object[]> AllTrendsWithExpectedStrings()
    {
        yield return new object[] { Trend.TripleUp, "Zooming Skyward" };
        yield return new object[] { Trend.DoubleUp, "Rapid Ascent" };
        yield return new object[] { Trend.SingleUp, "Steep Climb" };
        yield return new object[] { Trend.FortyFiveUp, "Going Up, Captain" };
        yield return new object[] { Trend.Flat, "Nice and easy" };
        yield return new object[] { Trend.FortyFiveDown, "Descending Swiftly" };
        yield return new object[] { Trend.SingleDown, "Plummet Mode" };
        yield return new object[] { Trend.DoubleDown, "Double the plunge" };
        yield return new object[] { Trend.TripleDown, "Free-fall Frenzy" };
        yield return new object[] { Trend.NotComputable, "Wandering in the Unknown" };
    }

    public static IEnumerable<object[]> VariantStringsWithCorrespondingTrend()
    {
        foreach (var valueLabelPair in AllTrendsWithExpectedStrings())
        {
            var trend = (Trend) valueLabelPair[0];
            var label = (string) valueLabelPair[1];

            yield return new object[] { label, trend };
            yield return new object[] { label.ToLowerInvariant(), trend };
            yield return new object[] { label.ToUpperInvariant(), trend };
        }
    }

    public static IEnumerable<object[]> TimeStampAndCorrespondingDateTimes()
    {
        var testDate = DateTime.MinValue.ToUniversalTime();
        yield return new object[] { testDate.ToUnixTimestamp(), testDate };
        
        testDate = DateTime.MaxValue.ToUniversalTime();
        yield return new object[] { testDate.ToUnixTimestamp(), testDate };
        
        testDate = DateTime.UtcNow;
        yield return new object[] { testDate.ToUnixTimestamp(), testDate };
        
        //Leap year day test.
        testDate = new DateTime(2020, 2, 29, 23, 59, 59, DateTimeKind.Utc);
        yield return new object[] { testDate.ToUnixTimestamp(), testDate };
    }

    public static IEnumerable<object[]> ValidHb1AcDataSets()
    {
        yield return new object[] { Generators.GenerateList(Constants.ReadingsIn115Days, 80), 4.4146338f };
        yield return new object[] { Generators.GenerateList(Constants.ReadingsIn115Days, 100), 5.111498f };
        yield return new object[] { Generators.GenerateList(Constants.ReadingsIn115Days, 150), 6.853658f };
        yield return new object[] { Generators.GenerateList(Constants.ReadingsIn115Days, 170), 7.5505223f };
        yield return new object[] { Generators.GenerateList(Constants.ReadingsIn115Days, 210), 8.944251f };
        yield return new object[] { Generators.GenerateList(Constants.ReadingsIn115Days, 300), 12.080139f };
        yield return new object[] { Generators.GenerateList(Constants.ReadingsIn115Days, 400), 15.56446f };
    }

    public static IEnumerable<object[]> PartiallyValidHb1AcDataSets()
    {
        yield return new object[] { Generators.GenerateList(Constants.ReadingsIn1Day, 80), 4.4146338f };
        yield return new object[] { Generators.GenerateList(Constants.ReadingsIn1Day, 100), 5.111498f };
        yield return new object[] { Generators.GenerateList(Constants.ReadingsIn1Day, 150), 6.853658f };
        yield return new object[] { Generators.GenerateList(Constants.ReadingsIn1Day, 170), 7.5505223f };
        yield return new object[] { Generators.GenerateList(Constants.ReadingsIn1Day, 210), 8.944251f };
        yield return new object[] { Generators.GenerateList(Constants.ReadingsIn1Day, 300), 12.080139f };
        yield return new object[] { Generators.GenerateList(Constants.ReadingsIn1Day, 400), 15.56446f };
    }
    

}