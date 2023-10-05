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
using Raccoon.Ninja.AzFn.DataApi.Utils;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Models;

namespace Raccoon.Ninja.AzFn.DataApi;

public static class DataLatestHbA1cFunc
{
    [FunctionName("DataLatestHbA1cFunc")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
        HttpRequest req,
        [CosmosDB(databaseName: "%CosmosDatabaseName%", containerName: "%CosmosAggregateContainerName%",
            Connection = "CosmosConnectionString",
            CreateIfNotExists = false,
            SqlQuery = "SELECT TOP 1 * FROM c WHERE c.docType = 1 and c.status = 1 ORDER BY c.createdAt DESC"
        )]
        IEnumerable<HbA1cCalculation> latestSuccessCalculations,
        [CosmosDB(databaseName: "%CosmosDatabaseName%", containerName: "%CosmosAggregateContainerName%",
            Connection = "CosmosConnectionString",
            CreateIfNotExists = false,
            SqlQuery = "SELECT TOP 1 * FROM c WHERE c.docType = 1 and c.status = 2 ORDER BY c.createdAt DESC"
        )]
        IEnumerable<HbA1cCalculation> latestPartialSuccessCalculations,
        ILogger log)
    {
        log.LogInformation("Data Latest HbA1c API call received. Request by IP: {Ip}",
            req.HttpContext.Connection.RemoteIpAddress);
        HbA1cCalculation latestSuccessful = null;
        HbA1cCalculation latestPartialSuccessful = null;

        try
        {
            if (!Validators.IsKeyValid(await req.Body.ExtractKey()))
                return new UnauthorizedResult();

            latestSuccessful = latestSuccessCalculations.FirstOrDefault();
            latestPartialSuccessful = latestPartialSuccessCalculations.FirstOrDefault();

            if (latestSuccessful is null && latestPartialSuccessful is null)
                return new NoContentResult();

            return new OkObjectResult(new DataSeriesApiFuncResponse
            {
                LatestSuccessful = latestSuccessful,
                LatestPartialSuccessful = latestPartialSuccessful
            });
        }
        catch (Exception e)
        {
            log.LogError(e,
                "Error while processing request from IP: {Ip} | Latest success : {LatestSuccessReading} / Latest Partial Success: {LatestPartialSuccess}",
                req.HttpContext.Connection.RemoteIpAddress, latestSuccessful, latestPartialSuccessful);
            return new StatusCodeResult(500);
        }
    }
}