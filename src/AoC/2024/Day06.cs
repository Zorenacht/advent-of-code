using Tools.Geometry;

namespace AoC._2024;

[PuzzleType(PuzzleType.Grid, PuzzleType.Cycle)]
public sealed class Day06 : Day
{
    [Puzzle(answer: 4559)]
    public int Part1() => Path(Input).GroupBy(x => x.Index).Count();

    [Puzzle(answer: 1604)]
    public int Part2() => Loop(Input);

    [Puzzle(answer: 6)]
    public int Part2Example() => Loop(InputExample);

    private static HashSet<IndexDirectionV2> Path(string[] lines)
    {
        var grid = lines.ToCharGrid();
        var current = grid.FindIndexes('^').First();
        var dir = Index2D.N;
        var set = new HashSet<IndexDirectionV2>();
        while (true)
        {
            if (grid.ValueOrDefault(current) == null) break;
            set.Add(new IndexDirectionV2(current, dir));

            var next = current + dir;
            while (grid.ValueOrDefault(next) == '#')
            {
                dir = dir.TurnRight();
                next = current + dir;
            }
            current = next;
        }
        return set;
    }

    private static int Loop(string[] lines)
    {
        var grid = lines.ToCharGrid();
        var path = Path(lines);
        return path.Zip(path.Skip(1))
            .Where(pair => grid.ValueOrDefault(pair.Second.Index) is '.')
            .GroupBy(pair => pair.Second.Index)
            .Select(x => x.First())
            .Count(pair => HasCycle(pair.First, pair.Second.Index, grid));
    }

    private static bool HasCycle(IndexDirectionV2 start, Index2D blockCandidate, CharGrid grid)
    {
        var simulated = new HashSet<IndexDirectionV2>();
        var indexDir = start;
        var blockades = grid.FindIndexes('#').Append(blockCandidate);
        grid.UpdateAt(blockCandidate, '#');
        var sparseGrid = new SparseGrid(grid, '#');
        while (true)
        {
            if (grid.ValueOrDefault(indexDir.Index) == null) break;

            if (simulated.Contains(indexDir))
            {
                grid.UpdateAt(blockCandidate, '.');
                return true;
            }

            var next = indexDir with { Index = indexDir.Index + indexDir.Direction };
            if (grid.ValueOrDefault(next.Index) == '#')
                simulated.Add(indexDir);
            while (grid.ValueOrDefault(next.Index) == '#')
            {
                indexDir = indexDir.TurnRight();
                next = indexDir.Forward();
            }
            indexDir = sparseGrid.NextBlockade(next);
            indexDir = indexDir with { Index = indexDir.Index - indexDir.Direction };
        }
        grid.UpdateAt(blockCandidate, '.');
        return false;
    }
}