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
        //testDate = new DateTime(9999, testDate.Month, testDate.Day, testDate.Hour, testDate.Minute, testDate.Second, DateTimeKind.Utc);
        yield return new object[] { testDate.ToUnixTimestamp(), testDate };
        
        testDate = DateTime.UtcNow;
        yield return new object[] { testDate.ToUnixTimestamp(), testDate };
        
        //Leap year day test.
        testDate = new DateTime(2020, 2, 29, 23, 59, 59, DateTimeKind.Utc);
        yield return new object[] { testDate.ToUnixTimestamp(), testDate };
    }
}