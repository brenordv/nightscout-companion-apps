using Bogus;
using MongoDB.Bson;
using Raccoon.Ninja.Fn.GlucoseMonitor.Mongo;
using Raccoon.Ninja.NightScout.Core.Enums;
using Raccoon.Ninja.NightScout.Core.ExtensionMethods;

namespace Raccoon.Ninja.Fn.GlucoseMonitor.Tests;

internal static class MongoDocumentGenerators
{
    internal static IList<NightScoutMongoDocument> NightScoutMongoDocumentMockList(int qty, int? value = null)
    {
        var faker = new Faker<NightScoutMongoDocument>()
            .RuleFor(x => x.Id, f => new ObjectId())
            .RuleFor(x => x.Value, f => value ?? f.Random.Number(60, 399))
            .RuleFor(x => x.ReadingTimestamp, f => f.Date.Past().ToUnixTimestamp())
            .RuleFor(x => x.ReadingTimestampAsString, f => f.Date.Past().ToString("yyyy-MM-ddTHH:mm:ssZ"))
            .RuleFor(x => x.Trend, f => f.Random.Enum<Trend>())
            .RuleFor(x => x.Direction, f => f.Random.String2(5))
            .RuleFor(x => x.Device, f => f.Random.String2(5))
            .RuleFor(x => x.Type, f => f.Random.String2(5))
            .RuleFor(x => x.UtcOffset, f => f.Random.Number(-12, 12))
            .RuleFor(x => x.SystemTime, f => f.Date.Past().ToString("yyyy-MM-ddTHH:mm:ssZ"));

        return faker.Generate(qty).OrderBy(doc => doc.ReadingTimestamp).ToList();
    }
}
