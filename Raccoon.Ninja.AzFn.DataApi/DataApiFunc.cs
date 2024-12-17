using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.AzFn.DataApi.ExtensionMethods;
using Raccoon.Ninja.AzFn.DataApi.Utils;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Models;

namespace Raccoon.Ninja.AzFn.DataApi;

public class DataApiFunc
{
    private readonly ILogger _logger;

    public DataApiFunc(ILogger<DataApiFunc> logger)
    {
        _logger = logger;
    }

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
        _logger.LogInformation("Data API call received. Request by IP: {Ip}",
            req.HttpContext.Connection.RemoteIpAddress);

        var results = new List<GlucoseReadingResponse>();

        try
        {
            if (!Validators.IsKeyValid(await req.Body.ExtractKey()))
                return new UnauthorizedResult();

            var readSince = req.TryGetReadSinceParam();

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
            _logger.LogError(e, "Error while processing request from IP: {Ip} | Latest reading : {LatestReading}",
                req.HttpContext.Connection.RemoteIpAddress, results);

            return new StatusCodeResult(500);
        }
    }
}