namespace Raccoon.Ninja.NightScout.Core.ExtensionMethods;

public static class DateTimeExtensions
{
    public static long ToUnixTimestamp(this DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
    }
}