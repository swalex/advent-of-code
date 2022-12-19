namespace AdventOfCode;

internal static class Extensions
{
    internal static int IndexOf<T>(this IEnumerable<T> collection, T item) where T : class
    {
        var index = 0;
        foreach (T current in collection)
        {
            if (Equals(current, item)) return index;
            ++index;
        }

        return -1;
    }
}