namespace AoC._2024;

[PuzzleType(PuzzleType.Compute, PuzzleType.DP, PuzzleType.Recursion)]
public sealed class Day11 : Day
{
    [Puzzle(answer: 194557)]
    public long Part1() => PebbleCountIterative(25);
    [Puzzle(answer: 194557)]
    public long Part1_Recursive() => PebbleCountRecursive(25);
    
    [Puzzle(answer: 231532558973909)]
    public long Part2() => PebbleCountIterative(75);
    [Puzzle(answer: 231532558973909)]
    public long Part2_Recursive() => PebbleCountRecursive(75);
    
    private long PebbleCountRecursive(int iterations)
    {
        var pebbles = InputAsText.Split(' ', StringSplitOptions.TrimEntries).ToList();
        var dp = new Dictionary<(string Value, int Iterations), long>();
        return pebbles.Sum(x => PebbleCount(x, iterations, dp));
    }
    
    private static long PebbleCount(string value, int iterations, Dictionary<(string Value, int Iterations), long> dict)
    {
        if (iterations == 0) return 1;
        if (dict.TryGetValue((value, iterations), out var count)) return count;
        
        long val = 0;
        if (value == "0")
        {
            val += PebbleCount("1", iterations - 1, dict);
        }
        else if (value.Length % 2 == 0)
        {
            val += PebbleCount(long.Parse(value[..(value.Length / 2)]).ToString(), iterations - 1, dict);
            val += PebbleCount(long.Parse(value[(value.Length / 2)..]).ToString(), iterations - 1, dict);
        }
        else
        {
            val += PebbleCount((long.Parse(value) * 2024).ToString(), iterations - 1, dict);
        }
        
        dict[(value, iterations)] = val;
        return val;
    }
    
    private long PebbleCountIterative(int iterations)
    {
        var split = InputAsText.Split(' ', StringSplitOptions.TrimEntries).ToList();
        var counts = new Dictionary<string, long>();
        foreach (var spl in split) AddOrPlus(counts, 1, spl);
        for (int i = 0; i < iterations; ++i)
        {
            var next = new Dictionary<string, long>();
            foreach (var kv in counts)
            {
                if (kv.Key == "0") AddOrPlus(next, kv.Value, "1");
                else if (kv.Key.Length % 2 == 0)
                {
                    AddOrPlus(next, kv.Value, long.Parse(kv.Key[..(kv.Key.Length / 2)]).ToString());
                    AddOrPlus(next, kv.Value, long.Parse(kv.Key[(kv.Key.Length / 2)..]).ToString());
                }
                else
                {
                    AddOrPlus(next, kv.Value, (long.Parse(kv.Key) * 2024).ToString());
                }
            }
            counts = next;
        }
        return counts.Sum(x => x.Value);
        
        void AddOrPlus(Dictionary<string, long> dict, long plus, string value)
        {
            if (!dict.TryAdd(value, plus)) dict[value] += plus;
        }
    }
};