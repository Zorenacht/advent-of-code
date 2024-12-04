using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Tools.Geometry;

public class Index2DIterator<T>(
    Index2D Start, 
    Func<Index2D, Index2D> Next,
    Grid<T> Grid,
    int Count) : IEnumerable<(Index2D Index, T Value)> where T : struct
{
    public IEnumerator<(Index2D Index, T Value)> GetEnumerator()
    {
        var index = Start;
        for (int i = 0; i <= Count; ++i)
        {
            if(Grid.ValueOrDefault(index) is {} value)
                yield return (index, value);
            index = Next(index);
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}