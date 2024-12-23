﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.AzFn.DataApi.ExtensionMethods;
using Raccoon.Ninja.AzFn.DataApi.Utils;
using Raccoon.Ninja.Domain.Core.Entities.StatisticalDataPoint;
using Raccoon.Ninja.Domain.Core.Models;

namespace Raccoon.Ninja.AzFn.DataApi;

public class DataLatestDailyReportFunc
{
    private readonly ILogger _logger;

    public DataLatestDailyReportFunc(ILogger<DataLatestDailyReportFunc> logger)
    {
        _logger = logger;
    }

    [Function("DataLatestDailyReportFunc")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
        HttpRequest req, [CosmosDBInput(
            "%CosmosDatabaseName%",
            "%CosmosAggregateContainerName%",
            Connection = "CosmosConnectionString",
            SqlQuery =
                "SELECT TOP 1 * FROM c WHERE c.docType = 1 and c.full.status = 1 and c.full.hbA1c.status = 1 ORDER BY c.createdAt DESC"
        )]
        IEnumerable<StatisticDataPointDocument> latestSuccessCalculations, [CosmosDBInput(
            "%CosmosDatabaseName%",
            "%CosmosAggregateContainerName%",
            Connection = "CosmosConnectionString",
            SqlQuery =
                "SELECT TOP 1 * FROM c WHERE c.docType = 1 and c.full.status = 1 and c.full.hbA1c.status = 2 ORDER BY c.createdAt DESC"
        )]
        IEnumerable<StatisticDataPointDocument> latestPartialSuccessCalculations)
    {
        _logger.LogInformation("Data Latest HbA1c API call received. Request by IP: {Ip}",
            req.HttpContext.Connection.RemoteIpAddress);

        StatisticDataPointDocument latestSuccessful = null;

        StatisticDataPointDocument latestPartialSuccessful = null;

        try
        {
            if (!Validators.IsKeyValid(await req.Body.ExtractKey()))
                return new UnauthorizedResult();

            latestSuccessful = latestSuccessCalculations.FirstOrDefault();

            latestPartialSuccessful = latestPartialSuccessCalculations.FirstOrDefault();

            if (latestSuccessful is null && latestPartialSuccessful is null)
                return new NoContentResult();

            return new OkObjectResult(new DataLatestStatisticalAggregationFuncResponse
            {
                Latest = latestSuccessful,
                LatestPartial = latestPartialSuccessful
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