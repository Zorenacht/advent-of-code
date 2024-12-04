using FluentAssertions;
using System.Text;
using Tools.Geometry;

namespace AoC_2024;

public sealed class Day04 : Day
{
    [Puzzle(answer: 2567)]
    public int Part1()
    {
        int result = 0;
        var lines = Input;
        for (int i = 0; i < lines.Length; ++i)
        {
            for (int j = 0; j < lines[i].Length; ++j)
            {
                if (lines[i][j] == 'X') result += CountXMAS(i, j);
            }
        }
        return result;
    }
    
    private int CountXMAS(int i, int j)
    {
        var p = new Point(j, i);
        var dirs = new Direction[] { Direction.E, Direction.N, Direction.W, Direction.S, Direction.NE, Direction.NW, Direction.SW, Direction.SE };
        var words = dirs
            .Select(dir => XmasWalk(p, dir, Input))
            .ToArray();
        return words.Count(x => x == "XMAS");
    }
    
    private string XmasWalk(Point p, Direction dir, string[] input)
    {
        var sb = new StringBuilder();
        sb.Append("X");
        for (int c = 1; c < 4; ++c)
        {
            p = p.NeighborV(dir);
            if (p.Y >= 0 && p.Y < input.Length && p.X >= 0 && p.X < input[0].Length)
                sb.Append(input[p.Y][p.X]);
        }
        return sb.ToString();
    }
    
    [Puzzle(answer: 2029)]
    public int Part2()
    {
        int result = 0;
        var lines = Input;
        for (int i = 0; i < lines.Length; ++i)
        {
            for (int j = 0; j < lines[i].Length; ++j)
            {
                if (lines[i][j] == 'A') result += CountX_MAS(i, j, lines) ? 1 : 0;
            }
        }
        return result;
    }
    
    private bool CountX_MAS(int i, int j, string[] input)
    {
        var p = new Point(j, i);
        var dirs = new Direction[] { Direction.NE, Direction.SE, Direction.SW, Direction.NW, };
        var values = dirs.Select(dir => Value(p.NeighborV(dir))).ToArray();        
        return values.Count(x => x == 'M') == 2 && values.Count(c => c == 'S') == 2 && values[0] != values[2];
        
        char? Value(Point point) =>
            point is { Y: >= 0, X: >= 0 } p2 && p2.Y < input.Length && p2.X < input[0].Length
                ? input[p2.Y][p2.X]
                : null;
    }
};