namespace Raccoon.Ninja.TestHelpers;

public static class TestUtils
{
    public static long ToUnixTimestamp(this DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
    }
}
