using System.Collections.Generic;
using System.Linq;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Extensions.MongoDb.ExtensionMethods;
using Raccoon.Ninja.Extensions.MongoDb.Models;

namespace Raccoon.Ninja.AzFn.ScheduledTasks.ExtensionMethods;

public static class ListExtensions
{
    /// <summary>
    ///     Converts a list of native Nightscout MongoDB documents to a list of entities owned by
    ///     this repo (<see cref="GlucoseReading" />).
    /// </summary>
    /// <param name="documents">Documents that will be converted</param>
    /// <param name="previousReading">Previous GlucoseReading. The value will be used to calculate the delta</param>
    /// <returns>Converted list</returns>
    public static IEnumerable<GlucoseReading> ToGlucoseReadings(this IList<NightScoutMongoDocument> documents,
        GlucoseReading previousReading)
    {
        var previous = previousReading ?? new GlucoseReading();

        foreach (var mongoDbDoc in documents)
        {
            var current = mongoDbDoc.ToGlucoseReading(previous.Value);

            if (current is null) continue;

            yield return current;
            previous = current;
        }
    }

    public static IList<GlucoseReading> GetLastDays(this IList<GlucoseReading> readings, int days)
    {
        var lastDate = readings.Max(r => r.ReadTimestampUtcAsDateTime);
        var firstDate = lastDate.AddDays(-days);

        return readings.Where(r => r.ReadTimestampUtcAsDateTime >= firstDate).ToList();
    }
}