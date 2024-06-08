using System.Numerics;

namespace AoC_2023;

public sealed class Day08 : Day
{
    [Puzzle(answer: 16043)]
    public long Part1() => MinSteps(str => str == "AAA", str => str == "ZZZ");

    [Puzzle(answer: 15726453850399)]
    public long Part2() => MinSteps(str => str.EndsWith("A"), str => str.EndsWith("Z"));

    private long MinSteps(Func<string, bool> startCondition, Func<string, bool> endCondition)
    {
        var sequence = Input[0];
        var mapping = Input[2..]
            .Select(x =>
            {
                var split = x.Split([" = (", ", ", ")"], StringSplitOptions.RemoveEmptyEntries);
                return new KeyValuePair<string, (string Left, string Right)>(split[0], (split[1], split[2]));
            })
            .ToDictionary();

        var currentNodes = mapping
            .Where(x => startCondition(x.Key))
            .Select(x => x.Key)
            .ToArray();
        var periods = new int[currentNodes.Length];
        for (var i = 0; i < periods.Length; i++)
        {
            int steps = 0;
            var node = currentNodes[i];
            while (!endCondition(node))
            {
                var lr = sequence[steps % sequence.Length];
                if (lr == 'L') node = mapping[node].Left;
                if (lr == 'R') node = mapping[node].Right;
                steps++;
            }
            periods[i] = steps;
        }
        var minPeriod = periods
            .Select(x => new BigInteger(x))
            .Aggregate((x, y) => x * y / BigInteger.GreatestCommonDivisor(x, y));
        return (long)minPeriod;
    }
}