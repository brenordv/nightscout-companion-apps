using Bogus;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.TestHelpers;

public static class Generators
{
    public static List<GlucoseReading> GenerateList(int qty, float value)
    {
        var faker = new Faker<GlucoseReading>()
            .RuleFor(x => x.Id, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Value, f => value)
            .RuleFor(x => x.Trend, f => f.Random.Enum<Trend>())
            .RuleFor(x => x.ReadTimestampUtc, f => f.Date.Past().ToUnixTimestamp());

        return faker.Generate(qty);
    }
}