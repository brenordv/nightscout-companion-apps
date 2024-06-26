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
            Delta = CalculateDelta(nightScoutMongoDocument.Value, previousValue)
        };
    }

    private static float? CalculateDelta(float currentValue, float? previousValue)
    {
        var validPreviousValue = previousValue is > 0;

        if (!validPreviousValue)
            return null;
        
        return currentValue - previousValue;
    }
}
