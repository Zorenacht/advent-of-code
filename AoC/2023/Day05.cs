using MathNet.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Tools.Shapes;

namespace AoC_2023;

public sealed class Day05 : Day
{
    [Puzzle(answer: 31599214)]
    public long Part1()
    {
        var seeds = Fertilizer.ParseSeeds(Input[0]);
        var groups = Fertilizer.ParseMaps(Input[2..]);
        var min = new Fertilizer(groups).Min(seeds);
        return min;
    }

    [Puzzle(answer: 20358599)]
    public long Part2()
    {
        var seeds = Fertilizer.ParseSeedRanges(Input[0]);
        var groups = Fertilizer.ParseMaps(Input[2..]);
        var min = new Fertilizer(groups).Min(seeds);
        return min;
    }

    public record Fertilizer(long[][][] Maps)
    {
        public long Min(IEnumerable<Interval> seeds)
        {
            var locations = new List<Interval>();
            var current = seeds;
            foreach (var maps in Maps)
            {
                var next = new List<Interval>() { };
                foreach (var curr in current)
                {
                    var mappedToItself = new List<Interval>() { curr };
                    foreach (var map in maps)
                    {
                        var range = new Interval(map[1], map[1] + map[2] - 1);
                        if (curr.Overlap(range))
                        {
                            var inter = curr.Intersection(range);
                            long translation = map[0] - map[1];
                            next.Add(inter + translation);
                        }
                        mappedToItself = mappedToItself.SelectMany(x => x.Remove(range)).ToList();
                    }
                    next.AddRange(mappedToItself);
                }
                current = next;
            }
            return current.Min(x => x.Start);
        }

        public static Interval[] ParseSeeds(string line)
            => line.Split(": ")[1]
                .Split(" ")
                .Select(x => new Interval(long.Parse(x), long.Parse(x)))
                .ToArray();

        public static Interval[] ParseSeedRanges(string line)
        {
            var seedsInput = line.Split(": ")[1]
                .Split(" ")
                .Select(x => long.Parse(x));

            return seedsInput
                .Zip(seedsInput.Skip(1), (first, second) => new Interval(first, first + second - 1))
                .Where((_, index) => index % 2 == 0)
                .ToArray();
        }

        public static long[][][] ParseMaps(string[] lines)
            => lines
                .Where(x => !x.Contains("map"))
                .GroupBy("")
                .Select(group => group
                    .Select(line => line
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => long.Parse(x)).ToArray())
                    .ToArray())
                .ToArray();
    }
}