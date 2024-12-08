using System.Collections;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Tools.Geometry;

public partial class Grid<T> : IEnumerable, IEnumerable<T> where T : struct
{
    public bool IsBorder(string chars, Index2D index) => ValueOrDefault(index) is char ch && chars.Contains(ch);

    public Areas<T> FloodFill(string chars, bool diagonals = false)
    {
        Dictionary<int, Area> keyedAreas = [];
        Dictionary<Index2D, Area> indexAreas = [];

        //var visited = new HashSet<Index2D>();
        foreach (var (index, value) in EnumerableWithIndex())
        {
            if (IsBorder(chars, index) || indexAreas.ContainsKey(index)) continue;
            var flooded = new Area();
            var toBeFlooded = new Queue<Index2D>();
            toBeFlooded.Enqueue(index);
            while (toBeFlooded.TryDequeue(out var next))
            {
                if (indexAreas.ContainsKey(next)) throw new NotSupportedException("should not be reachable");
                if (!flooded.Add(next)) continue;
                var directions = diagonals ? Directions.All : Directions.Cardinal;
                var toBeEnqueued = directions
                    .Select(dir => next + dir)
                    .Where(index => IsValid(index) && !IsBorder(chars, index));
                foreach (var toEnqueue in toBeEnqueued)
                {
                    toBeFlooded.Enqueue(toEnqueue);
                }
            }
            int areaIdentifier = keyedAreas.Count;
            keyedAreas[areaIdentifier] = flooded;
            foreach (var ind2 in flooded) indexAreas.Add(ind2, flooded);
        }
        return new Areas<T>()
        {
            KeyedAreas = keyedAreas,
            IndexAreas = indexAreas,
        };
    }
}

public class Areas<T> where T : struct
{
    public Dictionary<int, Area> KeyedAreas { get; init; } = [];
    public Dictionary<Index2D, Area> IndexAreas { get; init; } = [];
    public Dictionary<Index2D, int> IndexInts { get; init; } = [];

    public Areas()
    {

    }
}

public class Area : HashSet<Index2D>
{
    public Area() { }
}