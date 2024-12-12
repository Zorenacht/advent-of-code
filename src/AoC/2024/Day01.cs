namespace AoC._2024;

public sealed class Day01 : Day
{
    [Puzzle(answer: 1941353)]
    public int Part1()
    {
        int result = 0;
        var lines = Input;
        var left = new List<int>();
        var right = new List<int>();
        foreach (var line in lines)
        {
            var splitted = line.Split(' ');
            left.Add(int.Parse(splitted[0]));
            right.Add(int.Parse(splitted[^1]));
        }
        var l = left.OrderByDescending(x => x).ToArray();
        var r = right.OrderByDescending(x => x).ToArray();
        for (int i = 0; i < l.Length; i++)
        {
            result += Math.Abs(l[i] - r[i]);
        }
        return result;
    }

    [Puzzle(answer: 22539317)]
    public int Part2()
    {
        int result = 0;
        var lines = Input;
        var left = new List<int>();
        var right = new Dictionary<int, int>();
        foreach (var line in lines)
        {
            var splitted = line.Split(' ');
            left.Add(int.Parse(splitted[0]));
            var r = int.Parse(splitted[^1]);
            if (!right.TryAdd(r, 1)) ++right[r];
        }
        foreach (var l in left)
        {
            if (right.TryGetValue(l, out var val))
            {
                result += l * val;
            }
        }
        return result;
    }
};