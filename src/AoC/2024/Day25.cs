using Tools.Geometry;

namespace AoC._2024;

[PuzzleType(PuzzleType.Compute)]
public sealed class Day25 : Day
{
    [Puzzle(answer: 2933)]
    public long Part1()
    {
        int rows = 7;
        int cols = 5;
        var groups = Input.GroupBy(string.Empty);

        var top = new List<int[]>();
        var bot = new List<int[]>();
        var grids = new List<SparseGrid>();
        foreach (var group in groups)
        {
            var maze = group.ToMaze();
            var grid = new SparseGrid(group.ToMaze(), '#');
            grids.Add(grid);
            if (grid.WallRows.TryGetValue(0, out var row))
                top.Add(Enumerable.Range(0, cols)
                    .Select(i => grid.WallCols.TryGetValue(i, out var w1)
                        ? w1.Count
                        : 0)
                    .ToArray());
            if (grid.WallRows.TryGetValue(maze.RowLength - 1, out var col))
                bot.Add(Enumerable.Range(0, cols)
                    .Select(i => grid.WallCols.TryGetValue(i, out var w1)
                        ? w1.Count
                        : 0)
                    .ToArray());
        }

        long result = 0;
        foreach (var t in top)
        {
            foreach (var b in bot)
            {
                result += IsValid(t, b) ? 1 : 0;
            }
        }
        return result;

        bool IsValid(int[] t, int[] b)
        {
            for (int i = 0; i < t.Length; ++i)
            {
                if (t[i] + b[i] > rows) return false;
            }
            return true;
        }
    }
};