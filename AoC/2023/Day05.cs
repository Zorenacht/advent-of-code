using System.Reflection.Metadata.Ecma335;
using Tools.Shapes;

namespace AoC_2023;

public sealed class Day05 : Day
{
    [Puzzle(answer: 31599214)]
    public long Part1()
    {
        var set = new HashSet<int>();
        var dictionary = new Dictionary<string, int>();
        var seeds = Input[0].Split(": ")[1].Split(" ").Select(x => long.Parse(x)).ToArray();
        var groups = new List<List<long[]>>();
        int group = 0;
        foreach (var line in Input.Skip(1))
        {
            if (line == string.Empty)
            {
                group++;
                groups.Add(new List<long[]>());
                continue;
            }
            var nums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToArray();
            groups[^1].Add(nums);
        }

        var locations = new List<long>();
        foreach (long seed in seeds)
        {
            long iter = seed;
            foreach (var g in groups)
            {
                foreach (var arr in g)
                {
                    if (arr[1] <= iter && iter < arr[1] + arr[2])
                    {
                        iter = arr[0] + iter - arr[1];
                        break;
                    }
                }
            }
            locations.Add(iter);
        }
        return locations.Min();
    }

    public record Interval(long Start, long End)
    {
        public bool Overlap(Interval other)
        {
            return Math.Max(Start, other.Start) <= Math.Min(End, other.End);
        }

        public Interval Intersection(Interval other)
        {
            return Overlap(other)
                ? new Interval(Math.Max(Start, other.Start), Math.Min(End, other.End))
                : throw new Exception();
        }
    }

    [Puzzle(answer: null)]
    public long Part2()
    {
        var set = new HashSet<int>();
        var dictionary = new Dictionary<string, int>();
        var seedsInput = Input[0].Split(": ")[1].Split(" ").Select(x => long.Parse(x)).ToArray();
        var seeds = new List<Interval>();
        for (int i = 0; i < seedsInput.Length / 2; i++)
        {
            seeds.Add(new Interval(seedsInput[i], seedsInput[i] + seedsInput[i + 1] - 1));
        }
        var groups = new List<List<long[]>>();
        int group = 0;
        foreach (var line in Input.Skip(1))
        {
            if (line == string.Empty)
            {
                group++;
                groups.Add(new List<long[]>());
                continue;
            }
            var nums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToArray();
            groups[^1].Add(nums);
        }

        var locations = new List<Interval>();
        foreach (var seed in seeds)
        {
            var iter = new List<Interval>() { seed };
            var next = new List<Interval>() { };
            foreach (Interval it in iter)
            {
                foreach (var g in groups)
                {
                    foreach (var arr in g)
                    {
                        var range = new Interval(arr[1], arr[1] + arr[2] - 1);
                        if (it.Overlap(range))
                        {
                            var inter = it.Intersection(range);
                            long st = arr[0] + inter.Start - arr[1];
                            var newRange = new Interval(st, st + inter.End - inter.Start);
                            next.Add(it.Intersection(newRange));
                            break;
                        }
                    }
                }
            }
            iter = next;
            locations.AddRange(next);
        }
        return locations.Min(x => x.Start);
    }
}