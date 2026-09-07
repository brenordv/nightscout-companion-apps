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
            { Trend.NotComputable, "Wandering in the Unknown" }
        };
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
}
