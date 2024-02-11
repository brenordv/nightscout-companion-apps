using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Extensions.MongoDb.Models;

namespace Raccoon.Ninja.Extensions.MongoDb.ExtensionMethods;

public static class NightScoutMongoDocumentExtensions
{
    /// <summary>
    /// Converts a single Nightscout MongoDB document to a <see cref="GlucoseReading"/> entity.
    /// </summary>
    /// <param name="nightScoutMongoDocument">Document that will be converted</param>
    /// <param name="previousValue">Previous glucose reading value. Used to calculate the delta</param>
    /// <returns>Converted document</returns>
    public static GlucoseReading ToGlucoseReading(this NightScoutMongoDocument nightScoutMongoDocument,
        float? previousValue = null)
    {
        if (nightScoutMongoDocument is null)
            return null;
        
        return new GlucoseReading
        {
            ReadTimestampUtc = nightScoutMongoDocument.ReadingTimestamp,
            Trend = nightScoutMongoDocument.Trend,
            Value = nightScoutMongoDocument.Value,
            Delta = ShouldCalculateDelta(previousValue) ? nightScoutMongoDocument.Value - previousValue : null
        };
    }

    /// <summary>
    /// We should calculate the delta if the previous value exists and is greater than 0.
    /// </summary>
    /// <remarks>The glucose value should never be negative, but safeguards are always good.</remarks>
    /// <param name="previousValue">Value to be checked.</param>
    /// <returns>true if should calculate delta</returns>
    private static bool ShouldCalculateDelta(float? previousValue)
    {
        return previousValue is > 0;
    }
}
