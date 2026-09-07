using Bogus;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.TestHelpers;

public static class Generators
{
    public static IList<GlucoseReading> GlucoseReadingMockList(int qty, float? value = null)
    {
        var faker = new Faker<GlucoseReading>()
            .RuleFor(x => x.Id, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Value, f => value ?? f.Random.Number(60, 399))
            .RuleFor(x => x.Trend, f => f.Random.Enum<Trend>())
            .RuleFor(x => x.ReadTimestampUtc, f => f.Date.Past().ToUnixTimestamp());

        return faker.Generate(qty);
    }

    public static GlucoseReading GlucoseReadingMockSingle(float? value = null)
    {
        return GlucoseReadingMockList(1, value)[0];
    }
}
