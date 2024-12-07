using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Tools.Geometry;

[SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Rider")]
public class Path<T>(
    Index2D Start,
    Func<Index2D, Grid<T>, Index2D> Next,
    Grid<T> Grid,
    int Count) : IEnumerable<(Index2D Index, T Value)> where T : struct
{
    public HashSet<Index2D> Points = [];

    public Path<T> Determine()
    {
        foreach ((Index2D index, T value) in this)
        {
            Points.Add(index);
        }
        return this;
    }

    public IEnumerator<(Index2D Index, T Value)> GetEnumerator()
    {
        var index = Start;
        for (int i = 0; i <= Count; ++i)
        {
            if (Grid.ValueOrDefault(index) is { } value)
                yield return (index, value);
            index = Next(index, Grid);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}