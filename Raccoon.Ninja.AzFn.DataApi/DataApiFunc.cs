using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.AzFn.DataApi.ExtensionMethods;
using Raccoon.Ninja.AzFn.DataApi.Utils;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Models;
using Microsoft.Azure.Functions.Worker;

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
            databaseName: "%CosmosDatabaseName%", 
            containerName: "%CosmosContainerName%",
            Connection = "CosmosConnectionString",
            SqlQuery = "SELECT TOP 1 * FROM c ORDER BY c.readAt DESC"
        )]
        IEnumerable<GlucoseReading> previousReadings)
    {
        _logger.LogInformation("Data API call received. Request by IP: {Ip}", req.HttpContext.Connection.RemoteIpAddress);
        GlucoseReading latestReading = null;

        try
        {
            if (!Validators.IsKeyValid(await req.Body.ExtractKey()))
                return new UnauthorizedResult();

            latestReading = previousReadings.FirstOrDefault();
            GlucoseReadingResponse response = latestReading;

            return latestReading is null
                ? new NoContentResult()
                : new OkObjectResult(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while processing request from IP: {Ip} | Latest reading : {LatestReading}",
                            req.HttpContext.Connection.RemoteIpAddress, latestReading);
            return new StatusCodeResult(500);
        }
    }

}