using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using MongoDB.Bson;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Extensions.MongoDb.ExtensionMethods;
using Raccoon.Ninja.Extensions.MongoDb.Models;

namespace Raccoon.Ninja.AzFn.DataTransfer.ExtensionMethods;

public static class ListExtensions
{
    public static IEnumerable<GlucoseReading> ToGlucoseReadings(this IList<NightScoutMongoDocument> documents, GlucoseReading previousReading)
    {
        var previous = previousReading ?? new GlucoseReading();

        foreach (var mongoDbDoc in documents)
        {
            var current = mongoDbDoc.ToGlucoseReading(previous.Value);
            yield return current;
            previous = current;
        }
    }
}