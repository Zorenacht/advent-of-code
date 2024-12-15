using FluentAssertions;
using Tools.Geometry;

namespace AoC._2024;

public sealed class Day15 : Day
{
    [Puzzle(answer: 1495147)]
    public long Part1()
    {
        var groups = Input.GroupBy(string.Empty);
        var grid = groups[0].ToCharGrid();
        var moves = string.Join("", groups[1]);
        var current = grid.FindIndexes('@').First();

        grid.Print();
        foreach (var move in moves)
        {
            var dir = Map(move);
            var next = current + dir;
            var propagated = next;

            while (grid.ValueAt(propagated) == 'O')
            {
                propagated = propagated + Map(move);
            }
            if (grid.ValueAt(propagated) == '.')
            {
                grid.UpdateAt(propagated, 'O');
                grid.UpdateAt(current, '.');
                grid.UpdateAt(next, '@');
                current = next;
            }
        }
        grid.Print();

        return grid
            .EnumerableWithIndex()
            .Where(iv => iv.Value == 'O')
            .Sum(iv => iv.Index.Row * 100 + iv.Index.Col);
    }

    [Puzzle(answer: 1524905)]
    public long Part2()
    {
        var groups = Input.GroupBy(string.Empty);
        var grid = groups[0]
            .Select(line => line
                .Replace("#", "##")
                .Replace("O", "[]")
                .Replace(".", "..")
                .Replace("@", "@."))
            .ToArray()
            .ToCharGrid();
        var moves = string.Join("", groups[1]);
        var current = grid.FindIndexes('@').First();

        grid.Print();
        foreach (var move in moves)
        {
            var dir = Map(move);
            var next = current + dir;

            // horizontal movement
            if (dir == Index2D.E || dir == Index2D.W)
            {
                var boxes = new List<Index2D>();
                var propagated = next;
                while (grid.ValueAt(propagated) == '[' || grid.ValueAt(propagated) == ']')
                {
                    boxes.Add(propagated);
                    propagated = propagated + Map(move);
                }
                if (grid.ValueAt(propagated) == '.')
                {
                    for (int j = boxes.Count - 1; j >= 0; j--)
                    {
                        grid.UpdateAt(boxes[j] + dir, grid.ValueAt(boxes[j]));
                    }
                    grid.UpdateAt(next, '@');
                    grid.UpdateAt(current, '.');
                    current = next;
                }
            }

            // vertical movement
            if (dir == Index2D.N || dir == Index2D.S)
            {
                var toBeMoved = new List<Index2D>() { current };
                var currentRow = new HashSet<Index2D>() { current };
                bool canMove = true;
                while (canMove && currentRow.Count > 0)
                {
                    var nexts = new HashSet<Index2D>();
                    foreach (var cur in currentRow)
                    {
                        var nxt = cur + dir;
                        if (grid.ValueAt(nxt) == '[') nexts.UnionWith([nxt, nxt + Index2D.E]);
                        if (grid.ValueAt(nxt) == ']') nexts.UnionWith([nxt, nxt + Index2D.W]);
                        if (grid.ValueAt(nxt) == '#')
                        {
                            nexts = [];
                            canMove = false;
                            break;
                        }
                    }
                    currentRow = nexts;
                    toBeMoved.AddRange(nexts);
                }
                if (canMove)
                {
                    // iterating in reverse prevents overwriting boxes yet to be moved
                    toBeMoved.Reverse();
                    foreach (var box in toBeMoved)
                    {
                        grid.UpdateAt(box + dir, grid.ValueAt(box));
                        grid.UpdateAt(box, '.');
                    }
                    current = next;
                }
            }
        }
        grid.Print();

        return grid
            .EnumerableWithIndex()
            .Where(iv => iv.Value == '[')
            .Sum(iv => iv.Index.Row * 100 + iv.Index.Col);
    }

    public Index2D Map(char ch) => ch switch
    {
        '^' => Index2D.N,
        'v' => Index2D.S,
        '>' => Index2D.E,
        '<' => Index2D.W,
        _ => throw new ArgumentOutOfRangeException()
    };
};