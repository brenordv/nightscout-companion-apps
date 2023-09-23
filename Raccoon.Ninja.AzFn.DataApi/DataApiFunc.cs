using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.AzFn.DataApi.ExtensionMethods;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Models;

namespace Raccoon.Ninja.AzFn.DataApi;

public static class DataApiFunc
{
    [FunctionName("DataApiFunc")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
        HttpRequest req,
        [CosmosDB(databaseName: "%CosmosDatabaseName%", containerName: "%CosmosContainerName%",
            Connection = "CosmosConnectionString",
            CreateIfNotExists = false,
            SqlQuery = "SELECT TOP 1 * FROM c ORDER BY c.readAt DESC"
        )]
        IEnumerable<GlucoseReading> previousReadings,
        ILogger log)
    {
        log.LogInformation("Data API call received. Request by IP: {Ip}", req.HttpContext.Connection.RemoteIpAddress);
        GlucoseReading latestReading = null;

        try
        {
            var key = await req.Body.ExtractKey();
            var secret = GetSecret(); 
            
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(secret))
                return new BadRequestResult();

            if (!key.Equals(secret))
                return new UnauthorizedResult();
            
            latestReading = previousReadings.FirstOrDefault();
            GlucoseReadingResponse response = latestReading; 

            return latestReading is null
                ? new NoContentResult()
                : new OkObjectResult(response);
        }
        catch (Exception e)
        {
            log.LogError(e, "Error while processing request from IP: {Ip} | Latest reading : {LatestReading}",
                req.HttpContext.Connection.RemoteIpAddress, latestReading);
            return new StatusCodeResult(500);
        }
    }

    private static string GetSecret()
    {
        return Environment.GetEnvironmentVariable("SillySecret");
    }
}