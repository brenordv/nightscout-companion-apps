using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.Domain.Core.Converters;

public static partial class Converter
{
    public static string ToTrendString(Trend trend)
    {
        return trend switch
        {
            Trend.TripleUp => "Zooming Skyward",
            Trend.DoubleUp => "Rapid Ascent",
            Trend.SingleUp => "Steep Climb",
            Trend.FortyFiveUp => "Going Up, Captain",
            Trend.Flat => "Nice and easy",
            Trend.FortyFiveDown => "Descending Swiftly",
            Trend.SingleDown => "Plummet Mode",
            Trend.DoubleDown => "Double the plunge",
            Trend.TripleDown => "Free-fall Frenzy",
            Trend.NotComputable => "Wandering in the Unknown",
            _ => throw new ArgumentOutOfRangeException(nameof(trend), trend, $"Value '{trend}' is not mapped.")
        };
    }
}