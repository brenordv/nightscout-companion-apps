using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.Domain.Core.Converters;

public static partial class Converter
{
    public static Trend ToTrend(string trend)
    {
        if (string.IsNullOrWhiteSpace(trend))
            throw new ArgumentNullException(nameof(trend), "Value cannot be null or whitespace.");
        
        return trend.Trim().ToLowerInvariant() switch
        {
            "zooming skyward" => Trend.TripleUp,
            "rapid ascent" => Trend.DoubleUp,
            "steep climb" => Trend.SingleUp,
            "going up, captain" => Trend.FortyFiveUp,
            "nice and easy" => Trend.Flat,
            "descending swiftly" => Trend.FortyFiveDown,
            "plummet mode" => Trend.SingleDown,
            "double the plunge" => Trend.DoubleDown,
            "free-fall frenzy" => Trend.TripleDown,
            "wandering in the unknown" => Trend.NotComputable,
            _ => throw new ArgumentOutOfRangeException(nameof(trend), trend, $"Value '{trend}' is not mapped.")
        };
    }
}