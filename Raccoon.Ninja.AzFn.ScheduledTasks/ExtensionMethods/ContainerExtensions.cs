using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.AzFn.ScheduledTasks.ExtensionMethods;

public static class ContainerExtensions
{
    public static async Task<Dictionary<DocumentType, AggregationDataPoint>> GetLatestAggregationDataPointsAsync(
        this Container container)
    {
        var result = new Dictionary<DocumentType, AggregationDataPoint>();
        foreach (var docType in GetAllAggregationDocTypes())
        {
            var query = new QueryDefinition("SELECT TOP 1 * FROM c WHERE c.docType = @docType ORDER BY c.createdAt DESC")
                .WithParameter("@docType", (int)docType);
            var iterator = container.GetItemQueryIterator<AggregationDataPoint>(query);
            var fetched = await iterator.ReadNextAsync();
            var doc = fetched.FirstOrDefault();
            if (doc is null) continue;
            result.Add(docType, doc);
        }

        return result;
    }
    
    private static IList<DocumentType> GetAllAggregationDocTypes()
    {
        var values = (DocumentType[])Enum.GetValues(typeof(DocumentType));
        var result = values.Skip(1).ToArray();
        return result;
    }
}