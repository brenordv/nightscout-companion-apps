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

public class DataLatestHbA1CFunc
{
    private readonly ILogger _logger;

    public DataLatestHbA1CFunc(ILogger<DataLatestHbA1CFunc> logger)
    {
        _logger = logger;
    }

    [Function("DataLatestHbA1cFunc")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
        HttpRequest req, [CosmosDBInput(databaseName: "%CosmosDatabaseName%", containerName: "%CosmosAggregateContainerName%",
            Connection = "CosmosConnectionString",
            SqlQuery = "SELECT TOP 1 * FROM c WHERE c.docType = 1 and c.status = 1 ORDER BY c.createdAt DESC"
        )]
        IEnumerable<HbA1CCalculation> latestSuccessCalculations, [CosmosDBInput(databaseName: "%CosmosDatabaseName%", containerName: "%CosmosAggregateContainerName%",
            Connection = "CosmosConnectionString",
            SqlQuery = "SELECT TOP 1 * FROM c WHERE c.docType = 1 and c.status = 2 ORDER BY c.createdAt DESC"
        )]
        IEnumerable<HbA1CCalculation> latestPartialSuccessCalculations)
    {
        _logger.LogInformation("Data Latest HbA1c API call received. Request by IP: {Ip}",
                    req.HttpContext.Connection.RemoteIpAddress);
        HbA1CCalculation latestSuccessful = null;
        HbA1CCalculation latestPartialSuccessful = null;

        try
        {
            if (!Validators.IsKeyValid(await req.Body.ExtractKey()))
                return new UnauthorizedResult();

            latestSuccessful = latestSuccessCalculations.FirstOrDefault();
            latestPartialSuccessful = latestPartialSuccessCalculations.FirstOrDefault();

            if (latestSuccessful is null && latestPartialSuccessful is null)
                return new NoContentResult();

            return new OkObjectResult(new DataLatestHbA1CFuncResponse
            {
                LatestSuccessful = latestSuccessful,
                LatestPartialSuccessful = latestPartialSuccessful
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                            "Error while processing request from IP: {Ip} | Latest success : {LatestSuccessReading} / Latest Partial Success: {LatestPartialSuccess}",
                            req.HttpContext.Connection.RemoteIpAddress, latestSuccessful, latestPartialSuccessful);
            return new StatusCodeResult(500);
        }
    }
}