using FluentAssertions;
using Tools.Geometry;

namespace AoC._2024;

public sealed class Day15 : Day
{
    [Puzzle(answer: 1495147)]
    public long Part1()
    {
        int result = 0;
        var groups = Input.GroupBy(string.Empty);
        var grid = groups[0].ToCharGrid();
        var sequence = string.Join("", groups[1]);
        var current = grid.FindIndexes('@').First();
        grid.Print();
        foreach (var ch in sequence)
        {
            var next = current + Map(ch);
            if (grid.ValueOrDefault(next) == '#') continue;
            var prop = next;
            var os = new List<Index2D>();
            while (grid.ValueAt(prop) == 'O')
            {
                prop = prop + Map(ch);
            }
            if (grid.ValueAt(prop) == '#') next = current;
            else
            {
                grid.UpdateAt(prop, 'O');
                grid.UpdateAt(current, '.');
                grid.UpdateAt(next, '@');
                current = next;
            }
        }
        grid.Print();
        long sum = 0;
        foreach (var (ind, val) in grid.EnumerableWithIndex())
        {
            if (val == 'O') sum += ind.Row * 100 + ind.Col;
        }
        return sum;
    }

    public Index2D Map(char ch) => ch switch
    {
        '^' => Index2D.N,
        'v' => Index2D.S,
        '>' => Index2D.E,
        '<' => Index2D.W,
    };

    [Puzzle(answer: 1524905)]
    public long Part2()
    {
        var groups = Input.GroupBy(string.Empty);
        var grid = groups[0]
            .Select(line => line
                .Replace("#", "##")
                .Replace("O", "[]")
                .Replace(".", "..")
                .Replace("@", "@.")).ToArray().ToCharGrid();
        var sequence = string.Join("", groups[1]);
        var current = grid.FindIndexes('@').First();
        grid.Print();
        foreach (var ch in sequence)
        {
            var dir = Map(ch);
            var next = current + Map(ch);
            if (grid.ValueOrDefault(next) == '#') continue;
            var prop = next;
            var os = new List<Index2D>();

            Console.WriteLine($"Move {ch}");

            // horizontal movement
            if (dir == Index2D.E || dir == Index2D.W)
            {
                var propagated = new List<Index2D>();
                while (grid.ValueAt(prop) == '[' || grid.ValueAt(prop) == ']')
                {
                    propagated.Add(prop);
                    prop = prop + Map(ch);
                }
                if (grid.ValueAt(prop) == '#') next = current;
                else
                {
                    for(int j = propagated.Count-1; j >= 0; j--)
                    {
                        grid.UpdateAt(propagated[j] + dir, grid.ValueAt(propagated[j]));
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
                bool move = true;
                while (move && currentRow.Count > 0)
                {
                    var nexts = new HashSet<Index2D>();
                    foreach (var cur in currentRow)
                    {
                        if (grid.ValueAt(cur + dir) == '[') nexts.UnionWith([cur + dir, cur + dir + Index2D.E]);
                        if (grid.ValueAt(cur + dir) == ']') nexts.UnionWith([cur + dir, cur + dir + Index2D.W]);
                        if (grid.ValueAt(cur + dir) == '#')
                        {
                            nexts = [];
                            move = false;
                            break;
                        }
                    }
                    currentRow = nexts;
                    toBeMoved.AddRange(nexts);
                }
                if (move)
                {
                    toBeMoved.Reverse();
                    foreach (var box in toBeMoved)
                    {
                        grid.UpdateAt(box + dir, grid.ValueAt(box));
                        grid.UpdateAt(box, '.');
                    }
                    current = next;
                }
            }
            grid.Print();
        }
        long sum = 0;
        foreach (var (ind, val) in grid.EnumerableWithIndex())
        {
            if (val == '[') sum += ind.Row * 100 + ind.Col;
        }
        return sum;
    }
};