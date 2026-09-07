using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Models;
using Raccoon.Ninja.Fn.GlucoseMonitor.ExtensionMethods;
using Raccoon.Ninja.Fn.GlucoseMonitor.Utils;

namespace Raccoon.Ninja.Fn.GlucoseMonitor;

public class DataApiFunc(ILogger<DataApiFunc> logger)
{
    private readonly ILogger _logger = logger;

    [Function("DataApiFunc")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
        HttpRequest req,
        [CosmosDBInput(
            "%CosmosDatabaseName%",
            "%CosmosContainerName%",
            Connection = "CosmosConnectionString"
        )]
        Container container)
    {
        var results = new List<GlucoseReadingResponse>();
        long? readSince = null;

        try
        {
            if (!Validators.IsKeyValid(await req.Body.ExtractKeyAsync()))
                return new UnauthorizedResult();

            readSince = req.TryGetReadSinceParam();

            if (readSince.HasValue)
            {
                // Build a parameterized query
                var queryDefinition = new QueryDefinition(
                    "SELECT * FROM c WHERE c.readAt > @readSince ORDER BY c.readAt DESC"
                ).WithParameter("@readSince", readSince);

                using var iteratorReadsince = container.GetItemQueryIterator<GlucoseReading>(queryDefinition);
                while (iteratorReadsince.HasMoreResults)
                {
                    var response = await iteratorReadsince.ReadNextAsync();
                    results.AddRange(response.Select(item => (GlucoseReadingResponse)item));
                }

                return results.Count != 0 ? new OkObjectResult(results) : new NoContentResult();
            }

            // If no readSince parameter is provided, return the latest reading
            using var iteratorLatestReading = container.GetItemQueryIterator<GlucoseReading>(
                new QueryDefinition("SELECT TOP 1 * FROM c ORDER BY c.readAt DESC")
            );

            while (iteratorLatestReading.HasMoreResults)
            {
                var response = await iteratorLatestReading.ReadNextAsync();
                var latestReading = response.FirstOrDefault();

                if (latestReading is null) continue;

                results.Add(latestReading);
                return new OkObjectResult(results);
            }

            return new NoContentResult();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Data API request failed (readSince: [{ReadSince}]).", readSince);

            return new StatusCodeResult(500);
        }
    }
}
