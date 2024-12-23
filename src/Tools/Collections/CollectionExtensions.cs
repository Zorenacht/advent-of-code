using System.Diagnostics.CodeAnalysis;

namespace Collections;

[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
public static class CollectionExtensions
{
    public static string StringJoin<T>(this IEnumerable<T>? values, string sep)
    {
        return string.Join(sep, values ?? []);
    }

    public static (int Index, long Value) MaxWithIndex(this IEnumerable<long> enumerable)
    {
        long maxValue = long.MinValue;
        int maxIndex = -1;
        int index = 0;
        foreach (var item in enumerable)
        {
            if (maxValue < item || item == maxValue && maxIndex == -1)
            {
                maxValue = item;
                maxIndex = index;
            }
            ++index;
        }
        return (maxIndex, maxValue);
    }

    public static (int Index, long Value) MinWithIndex(this IEnumerable<long> enumerable)
    {
        long minValue = int.MaxValue;
        int minIndex = -1;
        int index = 0;
        foreach (var item in enumerable)
        {
            if (item < minValue || item == minValue && minIndex == -1)
            {
                minValue = item;
                minIndex = index;
            }
            ++index;
        }
        return (minIndex, minValue);
    }

    public static int? MaxOrNull(this IEnumerable<int> enumerable) => enumerable.Any()
        ? enumerable.Max()
        : null;

    public static int? MinOrNull(this IEnumerable<int> enumerable) => enumerable.Any()
        ? enumerable.Min()
        : null;
}