namespace Raccoon.Ninja.TestHelpers;

public static class TestUtils
{
    public static long ToUnixTimestamp(this DateTime dateTime)
        => new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
}