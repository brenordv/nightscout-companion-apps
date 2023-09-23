using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.Models;

namespace Raccoon.Ninja.WForm.GlucoseIcon.ExtensionMethods;

public static class GlucoseReadingResponseExtensions
{
    public static string ToIconText(this GlucoseReadingResponse reading)
    {
        return reading.Trend switch
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
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}