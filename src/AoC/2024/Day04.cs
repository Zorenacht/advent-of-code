using System.Text;
using Tools.Geometry;

namespace AoC._2024;

[PuzzleType(PuzzleType.Grid)]
public sealed class Day04 : Day
{
    [Puzzle(answer: 2567)]
    public int Part1()
    {
        var grid = Input.ToCharGrid();
        return grid.EnumerableWithIndex()
            .Where(ele => ele.Value == 'X')
            .Sum(ele => CountXMAS(ele.Index, grid));

        int CountXMAS(Index2D index, Grid<char> grid)
        {
            var words = Directions.All
                .Select(dir => XmasWalk(index, dir, grid))
                .ToArray();
            return words.Count(x => x == "XMAS");
        }

        string XmasWalk(Index2D index, Direction dir, Grid<char> input)
        {
            var sb = new StringBuilder();
            sb.Append("X");
            for (int c = 1; c < 4; ++c)
            {
                index = index.Neighbor(dir);
                if (index.Row >= 0 && index.Row < input.RowLength && index.Col >= 0 && index.Col < input.ColLength)
                    sb.Append(input[index.Row][index.Col]);
            }
            return sb.ToString();
        }
    }

    [Puzzle(answer: 2029)]
    public int Part2()
    {
        int result = 0;
        var grid = Input.ToCharGrid();
        foreach ((var index, char value) in grid.EnumerableWithIndex())
        {
            if (value == 'A') result += CountXMas(index, grid) ? 1 : 0;
        }
        return result;

        bool CountXMas(Index2D p, Grid<char> grid)
        {
            var values = Directions.Ordinal
                .Select(dir => grid.ValueOrDefault(p.Neighbor(dir)))
                .ToArray();
            return values.Count(x => x == 'M') == 2
                   && values.Count(c => c == 'S') == 2
                    && values[0] != values[2];
        }
    }
};