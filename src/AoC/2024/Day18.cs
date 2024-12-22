using System.Diagnostics.CodeAnalysis;
using Tools.Geometry;

namespace AoC._2024;

[SuppressMessage("ReSharper", "InlineTemporaryVariable")]
[PuzzleType(PuzzleType.Grid, PuzzleType.ShortestPath)]
public sealed class Day18 : Day
{
    [Puzzle(answer: 22)]
    public long Part1_Example()
    {
        var maze = InitMaze(7, 7, 12, InputExample);
        return maze.ShortestPathV2(Index2D.O, new Index2D(maze.RowLength - 1, maze.ColLength - 1));
    }

    [Puzzle(answer: 336)]
    public long Part1()
    {
        var maze = InitMaze(71, 71, 1024, Input);
        maze.Print();
        return maze.ShortestPathV2(Index2D.O, new Index2D(maze.RowLength - 1, maze.ColLength - 1));
    }

    [Puzzle(answer: "24,30")]
    public string Part2()
    {
        const int size = 71;
        var lines = Input;
        var start = Index2D.O;
        var end = new Index2D(size - 1, size - 1);
        var left = 0;
        var right = lines.Length - 1;
        while (left < right)
        {
            var mid = (left + right + 1) / 2;
            var midMaze = InitMaze(size, size, mid + 1, lines);
            var midValue = midMaze.ShortestPathV2(start, end);
            if (midValue == -1) right = mid;
            else left = mid + 1;
        }
        return lines[left];
    }

    private static Maze InitMaze(int rows, int cols, int take, string[] blockades)
    {
        var maze = new Maze(rows, cols);
        foreach (var line in blockades.Take(take))
        {
            var ints = line.Ints();
            maze.UpdateAt(ints[0], ints[1], '#');
        }
        return maze;
    }
};