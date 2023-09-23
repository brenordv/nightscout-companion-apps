using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Core.Models;

namespace Raccoon.Ninja.AzFn.DataApi.ExtensionMethods;

public static class StreamExtensions
{
    public static async Task<string> ExtractKey(this Stream stream)
    {
        var requestBodyString = await new StreamReader(stream).ReadToEndAsync();
        var request = JsonConvert.DeserializeObject<GetDataRequest>(requestBodyString);
        return request?.Key ?? string.Empty;
    }
}