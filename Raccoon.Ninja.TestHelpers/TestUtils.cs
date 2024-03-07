namespace Raccoon.Ninja.TestHelpers;

public static class TestUtils
{
    public static long ToUnixTimestamp(this DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
    }

    public static float CalculateStandardDeviation(IList<float> glucoseValues)
    {
        var average = glucoseValues.Average();
        
        var standardDeviation = (float)Math.Sqrt(glucoseValues.Sum(r => Math.Pow(r - average, 2)) / glucoseValues.Count);
        return standardDeviation;
    }
}