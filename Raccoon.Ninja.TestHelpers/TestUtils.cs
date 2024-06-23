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
    
    public static float CalculateMedian(IList<float> glucoseValues)
    {
        var count = glucoseValues.Count;
        var sortedList = glucoseValues.OrderBy(v => v).ToList();
        var mid = count / 2;
        return count % 2 != 0 ? sortedList[mid] : (sortedList[mid - 1] + sortedList[mid]) / 2;
    }
}