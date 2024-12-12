using Tools.Geometry;

namespace AoC._2024;

public sealed class Day06 : Day
{
    [Puzzle(answer: 4559)]
    public int Part1() => Path(Input).GroupBy(x => x.Index).Count();

    [Puzzle(answer: 1604)]
    public int Part2() => Loop(Input);

    [Puzzle(answer: 6)]
    public int Part2Example() => Loop(InputExample);

    private HashSet<IndexDirection> Path(string[] lines)
    {
        var grid = lines.ToCharGrid();
        var current = grid.FindIndexes('^').First();
        var dir = Direction.N;
        var set = new HashSet<IndexDirection>();
        while (true)
        {
            if (grid.ValueOrDefault(current) == null) break;
            set.Add(new IndexDirection(current, dir));

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
        var grid = lines.ToCharGrid();
        var path = Path(lines);
        return path.Zip(path.Skip(1))
            .Where(pair => grid.ValueOrDefault(pair.Second.Index) is '.')
            .GroupBy(pair => pair.Second.Index)
            .Select(x => x.First())
            .Count(pair => HasCycle(pair.First, pair.Second.Index, grid));
    }

    private static bool HasCycle(IndexDirection indexDir, Index2D blockCandidate, Grid<char> grid)
    {
        var simulated = new HashSet<IndexDirection>();
        var start = indexDir.Index;
        var dir = indexDir.Direction;
        while (true)
        {
            if (grid.ValueOrDefault(start) == null) break;

            if (simulated.Contains(new IndexDirection(start, dir)))
                return true;
            simulated.Add(new IndexDirection(start, dir));

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