using FluentAssertions;

namespace AoC._2024;

public sealed class Day22 : Day
{
    [Puzzle(answer: 14273043166)]
    public long Part1()
    {
        long result = 0;
        var lines = Input;
        foreach (var line in lines)
        {
            var num = line.Ints().First();
            result += SecretNumber(num, num, 2000, [], [], []);
        }
        return result;
    }

    [Puzzle(answer: 1667)]
    public long Part2()
    {
        var lines = Input;
        var cache = new Dictionary<string, long>();
        foreach (var line in lines)
        {
            var num = line.Ints().First();
            SecretNumber(num, num, 2_000, [], [], cache);
        }
        return cache.Max(x => x.Value);
    }

    public long SecretNumber(long current, long start, int times,
        List<long> diffs,
        HashSet<string> added,
        Dictionary<string, long> bananas)
    {
        if (times == 0) return current;
        var lastDigit = current % 10;
        current = ((current * 0064) ^ current) % 16777216;
        current = ((current / 0032) ^ current) % 16777216;
        current = ((current * 2048) ^ current) % 16777216;
        var diff = (current % 10) - lastDigit;
        if (diffs.Count == 4) diffs.RemoveAt(0);
        diffs.Add(diff);
        var key = string.Join(",", diffs);
        if (diffs.Count == 4 && added.Add(key))
        {
            if (!bananas.TryAdd(key, current % 10))
                bananas[key] += (current % 10);
        };
        return SecretNumber(current, start, times - 1, diffs, added, bananas);
    }

};