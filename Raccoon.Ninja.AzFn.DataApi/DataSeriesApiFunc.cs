using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.AzFn.DataApi.ExtensionMethods;
using Raccoon.Ninja.AzFn.DataApi.Utils;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Models;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;

namespace Raccoon.Ninja.AzFn.DataApi;

public class DataSeriesApiFunc
{
    private readonly ILogger _logger;

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

    public DataSeriesApiFunc(ILogger<DataSeriesApiFunc> logger)
    {
        _logger = logger;
    }

    [Function("DataSeriesApiFunc")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "DataSeriesApiFunc/{limit:int}")]
        HttpRequest req, 
        [CosmosDBInput(Connection = "CosmosConnectionString")] CosmosClient client, 
        int limit = 1)
    {
        _logger.LogInformation("Data Series API call received. Request by IP: {Ip}", req.HttpContext.Connection.RemoteIpAddress);

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
            : new OkObjectResult(JsonConvert.SerializeObject(response));
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