using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.WForm.GlucoseIcon.Models;

public record DataFetchResult(float GlucoseValue, Trend Trend, bool Success = true, string ErrorMessage = "")
{
    
    public static DataFetchResult FromError(string errorMessage) => new(-1, Trend.NotComputable, false, errorMessage);
}