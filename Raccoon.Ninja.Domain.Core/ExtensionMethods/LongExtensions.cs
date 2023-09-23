namespace Raccoon.Ninja.Domain.Core.ExtensionMethods;

public static class LongExtensions
{
    public static DateTime ToUtcDateTime(this long timestamp)
        => DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
}