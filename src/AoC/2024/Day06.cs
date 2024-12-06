using Tools.Geometry;

namespace AoC_2024;

public sealed class Day06 : Day
{
    [Puzzle(answer: 4559)]
    public int Part1() => Path(Input).GroupBy(x => x.Index).Count();
    
    [Puzzle(answer: 1604)]
    public int Part2() => Loop(Input);
    
    [Puzzle(answer: 6)]
    public int Part2Example() => Loop(InputExample);
    
    private record IndexDir(Index2D Index, Direction Direction);
    
    private HashSet<IndexDir> Path(string[] lines)
    {
        var grid = lines.ToGrid();
        var current = grid.FindIndexes('^').First();
        var dir = Direction.N;
        var set = new HashSet<IndexDir>();
        while (true)
        {
            if (grid.ValueOrDefault(current) == null) break;
            set.Add(new IndexDir(current, dir));
            
            var next = current + dir;
            while (grid.ValueOrDefault(next) == '#')
            {
                dir = dir.Right();
                next = current + dir;
            }
            current = next;
        }
        return set;
    }
    
    private int Loop(string[] lines)
    {
        var grid = lines.ToGrid();
        var path = Path(lines);
        return path.Zip(path.Skip(1))
            .Where(pair => grid.ValueOrDefault(pair.Second.Index) is '.')
            .GroupBy(pair => pair.Second.Index)
            .Select(x => x.First())
            .Count(pair => HasCycle(pair.First, pair.Second.Index, grid));
    }
    
    private static bool HasCycle(IndexDir indexDir, Index2D blockCandidate, Grid<char> grid)
    {
        var simulated = new HashSet<IndexDir>();
        var start = indexDir.Index;
        var dir = indexDir.Direction;
        while (true)
        {
            if (grid.ValueOrDefault(start) == null) break;
            
            if (simulated.Contains(new IndexDir(start, dir)))
                return true;
            simulated.Add(new IndexDir(start, dir));
            
            var next = start + dir;
            while (grid.ValueOrDefault(next) == '#' || next == blockCandidate)
            {
                dir = dir.Right();
                next = start + dir;
            }
            start = next;
        }
        return false;
    }
}