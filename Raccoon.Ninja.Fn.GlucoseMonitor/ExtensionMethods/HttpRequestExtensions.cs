using Microsoft.AspNetCore.Http;

namespace Raccoon.Ninja.Fn.GlucoseMonitor.ExtensionMethods;

public static class HttpRequestExtensions
{
    public static long? TryGetReadSinceParam(this HttpRequest req)
    {
        if (!req.Query.ContainsKey("readSince")) return null;

        if (long.TryParse(req.Query["readSince"], out var readSince))
            return readSince;

        return null;
    }
}
