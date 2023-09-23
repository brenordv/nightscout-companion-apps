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
}