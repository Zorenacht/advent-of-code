using MathNet.Numerics;

namespace AoC._2023;

public sealed class Day16 : Day
{
    [Puzzle(answer: 46)]
    public int Part1Example() => P1(InputExample);

    [Puzzle(answer: 7728)]
    public int Part1() => P1(Input);

    private IEnumerable<Complex32> Next(char ch, Complex32 dir)
    {
        if (ch == '.') yield return dir;
        else if (ch == '|' && dir.IsReal()) { yield return new Complex32(0, 1); yield return new Complex32(0, -1); }
        else if (ch == '|' && !dir.IsReal()) yield return dir;
        else if (ch == '-' && dir.IsReal()) yield return dir;
        else if (ch == '-' && !dir.IsReal()) { yield return new Complex32(1, 0); yield return new Complex32(-1, 0); }
        else if (ch == '/' && dir.IsReal()) yield return dir * new Complex32(0, 1);
        else if (ch == '/' && !dir.IsReal()) yield return dir * new Complex32(0, -1);
        else if (ch == '\\' && dir.IsReal()) yield return dir * new Complex32(0, -1);
        else if (ch == '\\' && !dir.IsReal()) yield return dir * new Complex32(0, 1);
    }

    public int P1(string[] input)
    {
        var board = input.AddBorder('*').Reverse().ToArray();
        var copy = board.Select(x => x.ToArray()).ToArray();
        var dir = new Complex32(1, 0);
        var current = new Complex32(1, board.Length - 2);
        var nexts = new Queue<(Complex32 Current, Complex32 Dir)>();
        nexts.Enqueue((current, dir));
        var visited = new HashSet<(Complex32 Current, Complex32 Dir)>();
        while (nexts.TryPeek(out var _))
        {
            var next = nexts.Dequeue();
            current = next.Current;
            dir = next.Dir;
            var value = board[(int)current.Imaginary][(int)current.Real];
            if ("*".Contains(value) || visited.Contains((current, dir))) continue;
            var newPoints = Next(value, dir).Select(d => (current + d, d));
            foreach (var p in newPoints)
            {
                nexts.Enqueue(p);
            }
            copy[(int)current.Imaginary][(int)current.Real] = '#';
            visited.Add((current, dir));
        }
        return copy.Sum(r => r.Count(ch => ch == '#'));
    }

    [Puzzle(answer: 51)]
    public int Part2Example() => Part2(InputExample);

    [Puzzle(answer: 8061)]
    public int Part2() => Part2(Input);

    public int Part2(string[] input)
    {
        var board = input.AddBorder('*').Reverse().ToArray();
        var allVisited = new HashSet<(Complex32 Current, Complex32 Dir)>();
        var bottom = board[1][1..^1].Select((x, index) => new Complex32(index + 1, 1));
        var top = board[board.Length - 2][1..^1].Select((x, index) => new Complex32(index + 1, board.Length - 2));
        var left = board.Select((x, index) => new Complex32(1, index)).ToArray()[1..^1];
        var right = board.Select((x, index) => new Complex32(board.Length - 2, index)).ToArray()[1..^1];
        var dirs = new[] { new Complex32(0, 1), new Complex32(0, -1), new Complex32(1, 0), new Complex32(-1, 0) };
        var borders = top.Union(left).Union(right).Union(bottom)
            .SelectMany(x => dirs
                .Select(y => (x, y)))
            .ToHashSet();
        int max = 0;
        while (borders.Any())
        {
            var copy = board.Select(x => x.ToArray()).ToArray();
            var border = borders.First();
            var current = border.Item1;
            var dir = border.Item2;
            var nexts = new Queue<(Complex32 Current, Complex32 Dir)>();
            nexts.Enqueue((current, dir));
            var visited = new HashSet<(Complex32 Current, Complex32 Dir)>();
            while (nexts.TryPeek(out var _))
            {
                var next = nexts.Dequeue();
                current = next.Current;
                dir = next.Dir;
                var value = board[(int)current.Imaginary][(int)current.Real];
                if (allVisited.Contains((current, dir))) { break; }
                if ("*".Contains(value) || visited.Contains((current, dir)))
                {
                    visited.Add((current, dir));
                    continue;
                }
                var newPoints = Next(value, dir).ToArray();
                foreach (var p in newPoints.Select(d => (current + d, d)))
                {
                    nexts.Enqueue(p);
                }
                copy[(int)current.Imaginary][(int)current.Real] = '#';
                visited.Add((current, dir));
            }
            var count = copy.Sum(r => r.Count(ch => ch == '#'));
            if (max < count) max = count;

            allVisited.ExceptWith(visited);
            borders.ExceptWith(visited);
        }
        return max;
    }

}