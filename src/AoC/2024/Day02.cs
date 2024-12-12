namespace AoC._2024;

public sealed class Day02 : Day
{
    [Puzzle(answer: 407)]
    public int Part1()
    {
        int result = 0;
        var lines = Input;
        foreach (var line in lines)
        {
            var vals = line.Split(' ').Select(x => int.Parse(x)).ToList();
            result += IsSafe(vals) ? 1 : 0;
        }
        return result;
    }

    [Puzzle(answer: 2)]
    public int Part1Example()
    {
        int result = 0;
        var lines = InputExample;
        foreach (var line in lines)
        {
            var vals = line.Split(' ').Select(x => int.Parse(x)).ToList();
            result += IsSafe(vals) ? 1 : 0;
        }
        return result;
    }

    [Puzzle(answer: 459)]
    public int Part2()
    {
        int result = 0;
        var lines = Input;
        foreach (var line in lines)
        {
            var splitted = line.Split(' ');
            var all = splitted.Select(x => int.Parse(x)).ToList();
            for (int i = 0; i < all.Count; i++)
            {
                var vals = all.Where((x, ind) => ind != i).ToList();
                if (IsSafe(vals))
                {
                    result++;
                    break;
                }
            }
        }
        return result;
    }

    private static bool IsSafe(List<int> vals)
    {
        bool increasing = true;
        bool decreasing = true;
        for (int i = 0; i < vals.Count - 1; ++i)
        {
            var dif = vals[i + 1] - vals[i];
            if (!(1 <= dif && dif <= 3)) increasing = false;
            if (!(-3 <= dif && dif <= -1)) decreasing = false;
        }
        return increasing || decreasing;
    }
};