using System.Numerics;

namespace AoC_2023;

public sealed class Day08 : Day
{
    [Puzzle(answer: 16043)]
    public long Part1()
    {
        var input = Input[0];
        var parsed = Input[2..]
            .Select(x =>
            {
                var split1 = x.Split(" = ");
                var arg1 = split1[0];
                var split2 = split1[1].Split([", ", "(", ")"], StringSplitOptions.RemoveEmptyEntries);
                var arg2 = split2[0];
                var arg3 = split2[1];

                return (arg1, arg2, arg3);
            })
            .ToArray();
        //var dict = new Dictionary<string, (string, string)>(parsed);
        int count = 0;
        var curr = "AAA";
        while (curr != "ZZZ")
        {
            var lr = input[count % input.Length];
            var first = parsed.First(x => x.Item1 == curr);
            if (lr == 'L') curr = first.arg2;
            if (lr == 'R') curr = first.arg3;
            count++;
        }
        return count;
    }

    //11309 not
    [Puzzle(answer: 15726453850399)]
    public long Part2()
    {
        var input = Input[0];
        var parsedtemp = Input[2..]
            .Select(x =>
            {
                var split1 = x.Split(" = ");
                var arg1 = split1[0];
                var split2 = split1[1].Split([", ", "(", ")"], StringSplitOptions.RemoveEmptyEntries);
                var arg2 = split2[0];
                var arg3 = split2[1];

                return (arg1, arg2, arg3);
            })
            .ToArray();
        var parsed = new Dictionary<string, (string, string)>();
        foreach (var p in parsedtemp)
        {
            parsed.Add(p.arg1, (p.arg2, p.arg3));
        }
        var curr = parsed.Where(x => x.Key.EndsWith("A")).Select(x => x.Key).ToArray();
        var period = new int[curr.Count()];
        for (var i = 0; i < period.Length; i++)
        {
            int count = 0;
            var c = curr[i];
            while (!c.EndsWith("Z"))
            {
                var next = new List<string>();
                var lr = input[count % input.Length];
                var first = parsed[c];
                if (lr == 'L') c = first.Item1;
                if (lr == 'R') c = first.Item2;
                count++;
            }
            period[i] = count;
        }
        var minPeriod = period.Select(x => new BigInteger(x)).Aggregate((x, y) => x * y / BigInteger.GreatestCommonDivisor(x, y));
        return (long)minPeriod;
    }

    private string Cards = "23456789TJQKA";
    private string CardsWithJoker = "J23456789TQKA";
}