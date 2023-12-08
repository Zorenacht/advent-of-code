using MathNet.Numerics.Distributions;
using System.Numerics;

namespace AoC_2023;

public sealed class Day08 : Day
{
    [Puzzle(answer: 16043)]
    public long Part1() => MinSteps(str => str == "AAA", str => str == "ZZZ");

    [Puzzle(answer: 15726453850399)]
    public long Part2() => MinSteps(str => str.EndsWith("A"), str => str.EndsWith("Z"));

    private long MinSteps(Func<string,bool> startCondition, Func<string,bool> endCondition)
    {
        var input = Input[0];
        var parsed = Input[2..]
            .Select(x =>
            {
                var split = x.Split([" = (", ", ", ")"], StringSplitOptions.RemoveEmptyEntries);
                return new KeyValuePair<string, (string Left, string Right)>(split[0], (split[1], split[2]));
            })
            .ToDictionary();

        var curr = parsed.Where(x => startCondition(x.Key)).Select(x => x.Key).ToArray();
        var period = new int[curr.Length];
        for (var i = 0; i < period.Length; i++)
        {
            int steps = 0;
            var c = curr[i];
            while (!endCondition(c))
            {
                var next = new List<string>();
                var lr = input[steps % input.Length];
                var first = parsed[c];
                if (lr == 'L') c = first.Left;
                if (lr == 'R') c = first.Right;
                steps++;
            }
            period[i] = steps;
        }
        var minPeriod = period.Select(x => new BigInteger(x)).Aggregate((x, y) => x * y / BigInteger.GreatestCommonDivisor(x, y));
        return (long)minPeriod;
    }
}