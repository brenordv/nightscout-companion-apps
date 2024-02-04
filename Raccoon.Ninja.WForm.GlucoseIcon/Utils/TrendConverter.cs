using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.WForm.GlucoseIcon.Utils;

public static class TrendConverter
{
    public static string ToNotifyIconText(this Trend trend)
    {
        return trend switch
        {
            Trend.TripleUp => "UUU",
            Trend.DoubleUp => " UU",
            Trend.SingleUp => "  U",
            Trend.FortyFiveUp => "- -U",
            Trend.Flat => "- - -",
            Trend.FortyFiveDown => "- -D",
            Trend.SingleDown => "  D",
            Trend.DoubleDown => " DD",
            Trend.TripleDown => "DDD",
            Trend.NotComputable => "? ?",
            _ => throw new ArgumentOutOfRangeException($"{trend} is not a valid trend.")
        };
    }
    
    public static string ToTaskbarIconText(this Trend trend)
    {
        return trend switch
        {
            Trend.TripleUp => "\u2b06\u2b06\u2b06",
            Trend.DoubleUp => " \u2b06\u2b06",
            Trend.SingleUp => "\u2b06",
            Trend.FortyFiveUp => "\u2b08",
            Trend.Flat => "\u279d",
            Trend.FortyFiveDown => "\u2b0a",
            Trend.SingleDown => "\u2193",
            Trend.DoubleDown => "\u2193\u2193",
            Trend.TripleDown => "\u2b07\u2b07\u2b07",
            Trend.NotComputable => "? ?",
            _ => throw new ArgumentOutOfRangeException($"{trend} is not a valid trend.")
        };
    }
}