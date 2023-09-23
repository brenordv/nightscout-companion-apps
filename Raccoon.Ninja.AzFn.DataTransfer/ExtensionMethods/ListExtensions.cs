using System.Collections.Generic;
using System.Linq;
using Raccoon.Ninja.Domain.Core.Entities;

namespace Raccoon.Ninja.AzFn.DataTransfer.ExtensionMethods;

public static class ListExtensions
{
    /// <summary>
    /// Returns the latest timestamp from a list of <see cref="GlucoseReading"/> or zero if none can be found.
    /// </summary>
    /// <param name="previousReadings">Previous readings</param>
    /// <returns>Latest timestamp</returns>
    public static long GetLatestTimestamp(this IEnumerable<GlucoseReading> previousReadings)
    {
        if (previousReadings is null)
            return 0;
        
        return previousReadings.FirstOrDefault()?.ReadTimestampUtc ?? 0; 
    }
}