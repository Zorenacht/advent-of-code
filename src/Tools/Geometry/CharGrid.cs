namespace Tools.Geometry;

public class CharGrid : Grid<char>
{
    public Areas FloodFill(string chars, bool diagonals = false)
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
        return new Areas
        {
            KeyedAreas = keyedAreas,
            IndexAreas = indexAreas,
        };
    }
    
    
    public Areas FloodFillRegions(bool diagonals = false)
    {
        Dictionary<int, Area> keyedAreas = [];
        Dictionary<Index2D, Area> indexAreaMapping = [];
        
        foreach (var (index, value) in EnumerableWithIndex())
        {
            if (indexAreaMapping.ContainsKey(index)) continue;
            var flooded = new Area();
            var toBeFlooded = new Queue<Index2D>();
            toBeFlooded.Enqueue(index);
            while (toBeFlooded.TryDequeue(out var next))
            {
                if (indexAreaMapping.ContainsKey(next)) throw new NotSupportedException("should not be reachable");
                if (!flooded.Add(next)) continue;
                var directions = diagonals ? Directions.AllIndex : Directions.CardinalIndex;
                var neighbors = directions.Select(dir => new BorderIndex(next + dir, dir));
                foreach (var nb in neighbors)
                {
                    if (ValueOrDefault(nb.Index) is char nbVal && nbVal == value)
                        toBeFlooded.Enqueue(nb.Index);
                    else
                        flooded.Border.Add(nb);
                }
            }
            int areaIdentifier = keyedAreas.Count;
            keyedAreas[areaIdentifier] = flooded;
            foreach (var ind in flooded) indexAreaMapping.Add(ind, flooded);
        }
        return new Areas
        {
            KeyedAreas = keyedAreas,
            IndexAreas = indexAreaMapping,
        };
    }
    
    private bool IsBorder(string chars, Index2D index) => ValueOrDefault(index) is char ch && chars.Contains(ch);
    
    public CharGrid(char[][] lines) : base(lines)
    {
    }
    
    public CharGrid(int row, int col) : base(row, col)
    {
    }
}

public class Areas
{
    public Dictionary<int, Area> KeyedAreas { get; init; } = [];
    public Dictionary<Index2D, Area> IndexAreas { get; init; } = [];
    
    public Areas()
    {
    }
}

public class Area : HashSet<Index2D>
{
    public HashSet<BorderIndex> Border { get; } = [];
    
    public int NumberOfSides()
    {
        var horizontalSides = Border
            .Where(x => x.Direction == Index2D.N || x.Direction == Index2D.S)
            .GroupBy(x => (x.Index.Row, x.Direction))
            .Select(x => x.OrderBy(border => border.Index.Col).ToArray())
            .Sum(group => group.Zip(group.Skip(1))
                .Count(x => x.Second.Index.Col - x.First.Index.Col > 1 || x.Second.Direction != x.First.Direction)
                .Plus(1));
        var temp = Border
            .Where(x => x.Direction == Index2D.E || x.Direction == Index2D.W)
            .GroupBy(x => (x.Index.Col, x.Direction))
            .Select(x => x.OrderBy(border => border.Index.Row).ToArray());
        var verticalSides = temp
            .Sum(group => group.Zip(group.Skip(1))
                .Count(x => x.Second.Index.Row - x.First.Index.Row > 1 || x.Second.Direction != x.First.Direction)
                .Plus(1));
        return horizontalSides + verticalSides;
    }
}