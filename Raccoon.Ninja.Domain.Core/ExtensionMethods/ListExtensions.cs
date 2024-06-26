using Raccoon.Ninja.Domain.Core.Entities;

namespace Raccoon.Ninja.Domain.Core.ExtensionMethods;

public static class ListExtensions
{
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

    /// <summary>
    /// Extracts the glucose values from the readings and returns them as a sorted (ascending) array.
    /// </summary>
    /// <param name="readings">List of GlucoseReadings to be used</param>
    /// <returns>Array of values</returns>
    public static IList<float> ToSortedValueArray(this IEnumerable<GlucoseReading> readings)
    {
        return readings.Select(r => r.Value).OrderBy(v => v).ToArray();
    }
}