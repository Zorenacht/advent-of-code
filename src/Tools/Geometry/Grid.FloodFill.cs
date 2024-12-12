using Microsoft.VisualBasic;
using System.Collections;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Tools.Geometry;

public partial class Grid<T> : IEnumerable, IEnumerable<T> where T : struct
{
    public Areas<T> FloodFill(string chars, bool diagonals = false)
    {
        Dictionary<int, Area> keyedAreas = [];
        Dictionary<Index2D, Area> indexAreas = [];
        
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
                    .Where(ind => IsValid(ind) && !IsBorder(chars, ind));
                foreach (var toEnqueue in toBeEnqueued)
                {
                    toBeFlooded.Enqueue(toEnqueue);
                }
            }
            int areaIdentifier = keyedAreas.Count;
            keyedAreas[areaIdentifier] = flooded;
            foreach (var ind in flooded) indexAreas.Add(ind, flooded);
        }
        return new Areas<T>
        {
            KeyedAreas = keyedAreas,
            IndexAreas = indexAreas,
        };
    }
    
    
    public Areas<T> FloodFillInclude(string include, bool diagonals = false)
    {
        Dictionary<int, Area> keyedAreas = [];
        Dictionary<Index2D, Area> indexAreaMapping = [];
        
        foreach (var (index, value) in EnumerableWithIndex())
        {
            if (value is not char ch) continue;
            if (!include.Contains(ch) || indexAreaMapping.ContainsKey(index)) continue;
            var flooded = new Area();
            var toBeFlooded = new Queue<Index2D>();
            toBeFlooded.Enqueue(index);
            while (toBeFlooded.TryDequeue(out var next))
            {
                if (indexAreaMapping.ContainsKey(next)) throw new NotSupportedException("should not be reachable");
                if (!flooded.Add(next)) continue;
                var directions = diagonals ? Directions.All : Directions.Cardinal;
                var toBeEnqueued = directions
                    .Select(dir => next + dir)
                    .Where(ind => ValueOrDefault(ind) is char nbVal && nbVal == ch);
                foreach (var toEnqueue in toBeEnqueued)
                {
                    toBeFlooded.Enqueue(toEnqueue);
                }
            }
            int areaIdentifier = keyedAreas.Count;
            keyedAreas[areaIdentifier] = flooded;
            foreach (var ind in flooded) indexAreaMapping.Add(ind, flooded);
        }
        return new Areas<T>
        {
            KeyedAreas = keyedAreas,
            IndexAreas = indexAreaMapping,
        };
    }
    
    private bool IsBorder(string chars, Index2D index) => ValueOrDefault(index) is char ch && chars.Contains(ch);
}

public class Areas<T> where T : struct
{
    public Dictionary<int, Area> KeyedAreas { get; init; } = [];
    public Dictionary<Index2D, Area> IndexAreas { get; init; } = [];
    
    public Areas()
    {
    }
}

public class Area : HashSet<Index2D>
{
    public Area()
    {
    }
}