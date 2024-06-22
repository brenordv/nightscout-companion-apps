using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.Domain.Core.ExtensionMethods;

public static class DictionaryExtensions
{
    public static float? CalculateDelta(this Dictionary<DocumentType, AggregationDataPoint> previousCalculations, DocumentType type, float currentValue)
    {
        return currentValue - previousCalculations.GetValueOrDefault(type)?.Value;
    }
}