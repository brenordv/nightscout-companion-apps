using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Extensions.MongoDb.Models;

namespace Raccoon.Ninja.Extensions.MongoDb.ExtensionMethods;

public static class NightScoutMongoDocumentExtensions
{
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

    private static bool ShouldCalculateDelta(float? previousValue)
    {
        return previousValue is > 0;
    }
}
