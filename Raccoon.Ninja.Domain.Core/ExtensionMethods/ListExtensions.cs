using Raccoon.Ninja.Domain.Core.Entities;
using Raccoon.Ninja.Domain.Core.Enums;

namespace Raccoon.Ninja.Domain.Core.ExtensionMethods;

public static class ListExtensions
{
    private const int ReadingsIn115Days = 33120;

    /// <summary>
    /// A more performative way to check if a list has elements.
    /// </summary>
    /// <param name="list">List to be checked</param>
    /// <typeparam name="T">Type of the list</typeparam>
    /// <returns>True if the list is not none and contains at least 1 element</returns>
    public static bool HasElements<T>(this ICollection<T> list)
    {
        return list is not null && list.Count > 0;
    }
    
    public static HbA1cCalculation CalculateHbA1c(this IEnumerable<GlucoseReading> list, DateOnly referenceDate)
    {
        if (list is null)
            return HbA1cCalculation.FromError("No readings to calculate HbA1c", referenceDate);

        var count = 0;
        var sum = 0f;
        foreach (var glucoseReading in list)
        {
            count++;
            sum += glucoseReading.Value;
        }

        if (count == 0)
        {
            return HbA1cCalculation.FromError("No readings returned from Db to calculate HbA1c", referenceDate);
        }
        
        var avg = sum / count;
        var hbA1c = (avg + 46.7f) / 28.7f;
        
        //Should never happen, but just in case...
#pragma warning disable S2583
        if (count > ReadingsIn115Days)
#pragma warning restore S2583
        {
            return HbA1cCalculation.FromError(
                $"Too many readings to calculate HbA1c reliably. Expected {ReadingsIn115Days} but got {count}", referenceDate);
        }

        return new HbA1cCalculation
        {
            Value = hbA1c,
            ReferenceDate = referenceDate,
            Status = count == ReadingsIn115Days 
                ? HbA1cCalculationStatus.Success 
                : HbA1cCalculationStatus.SuccessPartial
        };
    }
}