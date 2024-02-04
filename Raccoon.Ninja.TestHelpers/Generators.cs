using Bogus;
using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;
using Raccoon.Ninja.Domain.Core.Models;

namespace Raccoon.Ninja.TestHelpers;

public static class Generators
{
    public static IList<GlucoseReading> GlucoseReadingMockList(int qty, float value)
    {
        var faker = new Faker<GlucoseReading>()
            .RuleFor(x => x.Id, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Value, f => value)
            .RuleFor(x => x.Trend, f => f.Random.Enum<Trend>())
            .RuleFor(x => x.ReadTimestampUtc, f => f.Date.Past().ToUnixTimestamp());

        return faker.Generate(qty);
    }
    
    public static IList<HbA1CCalculationResponse> HbA1CCalculationResponseMockList(int qty, float? value = null, 
        float? delta = null, HbA1CCalculationStatus? status = null, string error = null)
    {
        var faker = new Faker<HbA1CCalculationResponse>()
            .RuleFor(x => x.Id, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Value, f => value ?? f.Random.Float(4, 8))
            .RuleFor(x => x.Delta, f => delta ?? f.Random.Float(-1, 2))
            .RuleFor(x => x.DocType, f => AggregateType.HbA1cCalculation)
            .RuleFor(x => x.ReferenceDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .RuleFor(x => x.CreatedAtUtc, f => f.Date.Past().ToUnixTimestamp())
            .RuleFor(x => x.Status, f => GetRandomHbA1CCalculationStatusWithinReason(f, status, error))
            .RuleFor(x => x.Error, f => error);

        return faker.Generate(qty);
    }

    public static HbA1CCalculationResponse HbA1CCalculationResponseMockSingle(float? value = null,
        float? delta = null, HbA1CCalculationStatus? status = null, string error = null)
    {
        return HbA1CCalculationResponseMockList(1, value, delta, status, error)[0];
    }
    
    public static IList<HbA1CCalculation> HbA1CCalculationMockList(int qty, HbA1CCalculationStatus? status = null, 
        string error = null)
    {
        var faker = new Faker<HbA1CCalculation>()
            .RuleFor(x => x.Id, f => f.Random.Guid().ToString())
            .RuleFor(x => x.ReferenceDate, f => DateOnly.FromDateTime(f.Date.Past()))
            .RuleFor(x => x.CreatedAtUtc, f => f.Date.Past().ToUnixTimestamp())
            .RuleFor(x => x.Status, f => GetRandomHbA1CCalculationStatusWithinReason(f, status, error))
            .RuleFor(x => x.Error, f => error);

        return faker.Generate(qty);
    }
    
    public static HbA1CCalculation HbA1CCalculationMockSingle(HbA1CCalculationStatus? status = null, 
        string error = null)
    {
        return HbA1CCalculationMockList(1, status, error)[0];
    }
    
    private static HbA1CCalculationStatus GetRandomHbA1CCalculationStatusWithinReason(Faker f, HbA1CCalculationStatus? inputStatus,
        string error)
    {
        if (inputStatus.HasValue)
            return inputStatus.Value;

        return string.IsNullOrWhiteSpace(error) 
            ? f.Random.Enum<HbA1CCalculationStatus>()
            : HbA1CCalculationStatus.Error;
    }
}