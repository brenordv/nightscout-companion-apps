﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.AzFn.DataApi.ExtensionMethods;
using Raccoon.Ninja.AzFn.DataApi.Utils;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Models;

namespace Raccoon.Ninja.AzFn.DataApi;

public static class DataSeriesApiFunc
{
    // 14 days.
    private const int DefaultLimit = 4032;
    private static int? _dataSeriesMaxRecords;
    private static int DataSeriesMaxRecords
    {
        get
        {
            _dataSeriesMaxRecords ??= GetDataSeriesMaxRecords();
            return _dataSeriesMaxRecords.Value;
        }
    }

    [FunctionName("DataSeriesApiFunc")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "DataSeriesApiFunc/{limit:int}")]
        HttpRequest req,
        [CosmosDB(Connection = "CosmosConnectionString")] CosmosClient client, 
        ILogger log, 
        int limit = 1)
    {
        if (!Validators.IsKeyValid(await req.Body.ExtractKey()))
            return new UnauthorizedResult();
        
        var (limitIsValid, adjustedLimit) = ValidateLimit(limit);
        
        if (!limitIsValid)
            return new BadRequestResult();

        var container = client
            .GetDatabase(Environment.GetEnvironmentVariable("CosmosDatabaseName"))
            .GetContainer(Environment.GetEnvironmentVariable("CosmosContainerName"));

        var queryDefinition = new QueryDefinition("SELECT TOP @limit * FROM c ORDER BY c.readAt DESC")
            .WithParameter("@limit", adjustedLimit);

        using var resultSet = container.GetItemQueryIterator<GlucoseReading>(queryDefinition);
        var response = new List<GlucoseReadingResponse>();
        
        while (resultSet.HasMoreResults)
        {
            response.AddRange((await resultSet.ReadNextAsync()).Select(x => (GlucoseReadingResponse)x));
        }

        return response.Count == 0
            ? new NoContentResult()
            : new OkObjectResult(response);
    }

    private static int GetDataSeriesMaxRecords()
    {
        var maxRecords = Environment.GetEnvironmentVariable("DataSeriesMaxRecords");
        return int.TryParse(maxRecords, out var result) ? result : DefaultLimit;
    }
    
    private static (bool limitIsValid, int adjustedLimit) ValidateLimit(int limit)
    {
        return limit <= 0
            ? (false, 1)
            : (true, Math.Min(limit, DataSeriesMaxRecords));
    }
}