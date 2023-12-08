using System.Numerics;

namespace AoC_2023;

public sealed class Day08 : Day
{
    [Puzzle(answer: 16043)]
    public long Part1() => MinSteps(str => str == "AAA", str => str == "ZZZ");

    private long MinSteps(Func<string,bool> start, Func<string,bool> end)
    {
        var input = Input[0];
        var parsed = Input[2..]
            .Select(x =>
            {
                var split = x.Split([" = (", ", ", ")"], StringSplitOptions.RemoveEmptyEntries);
                return new KeyValuePair<string, (string Left, string Right)>(split[0], (split[1], split[2]));
            })
            .ToDictionary();
        int steps = 0;
        var curr = "AAA";
        while (curr != "ZZZ")
        {
            var lr = input[steps % input.Length];
            var first = parsed[curr];
            if (lr == 'L') curr = first.Left;
            if (lr == 'R') curr = first.Right;
            steps++;
        }
        return steps;
    }

    //11309 not
    [Puzzle(answer: 15726453850399)]
    public long Part2()
    {
        var input = Input[0];
        var parsed = Input[2..]
            .Select(x =>
            {
                var split = x.Split([" = (", ", ", ")"], StringSplitOptions.RemoveEmptyEntries);
                return new KeyValuePair<string,(string Left,string Right)>(split[0], (split[1], split[2]));
            })
            .ToDictionary();
        var curr = parsed.Where(x => x.Key.EndsWith("A")).Select(x => x.Key).ToArray();
        var period = new int[curr.Length];
        for (var i = 0; i < period.Length; i++)
        {
            int count = 0;
            var c = curr[i];
            while (!c.EndsWith("Z"))
            {
                var next = new List<string>();
                var lr = input[count % input.Length];
                var first = parsed[c];
                if (lr == 'L') c = first.Left;
                if (lr == 'R') c = first.Right;
                count++;
            }
            period[i] = count;
        }
        var minPeriod = period.Select(x => new BigInteger(x)).Aggregate((x, y) => x * y / BigInteger.GreatestCommonDivisor(x, y));
        return (long)minPeriod;
    }
}