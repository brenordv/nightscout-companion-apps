using Newtonsoft.Json;
using Raccoon.Ninja.NightScout.Core.Models;

namespace Raccoon.Ninja.Fn.GlucoseMonitor.ExtensionMethods;

public static class StreamExtensions
{
    public static async Task<string> ExtractKeyAsync(this Stream stream)
    {
        var requestBodyString = await new StreamReader(stream).ReadToEndAsync();
        var request = JsonConvert.DeserializeObject<GetDataRequest>(requestBodyString);
        return request?.Key ?? string.Empty;
    }
}
