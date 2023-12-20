using FluentAssertions.Equivalency.Steps;
using MathNet.Numerics;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Constraints;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using Tools.Shapes;
using static AoC_2023.Day17;

namespace AoC_2023;

public sealed class Day20 : Day
{
    [Puzzle(answer: 32000000)]
    public long Part1Example() => new Propagation().Part1(InputExample);

    [Puzzle(answer: 670984704)]
    public long Part1() => new Propagation().Part1(Input);

    [Puzzle(answer: 262775362119547)]
    public long Part2() => new Propagation().Part2(Input);

    private class Propagation
    {
        private Dictionary<string, Node> Dictionary(string[] input)
        {
            var parsed = input.Select<string, (int Type, string Name, string[] Nexts)>(x =>
            {
                var split = x.Split(" ->,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var type = "%&".IndexOf(split[0][0]) + 1;
                return (type, type == 0 ? split[0] : split[0][1..], split[1..]);
            }).ToList();
            var dict = new Dictionary<string, Node>(parsed.Select(x => new KeyValuePair<string, Node>(x.Name, new Node() { Name = x.Name, Type = x.Type })));
            foreach (var entry in parsed)
            {
                dict[entry.Name].Nexts = entry.Nexts.Select(str => {
                    if (!dict.ContainsKey(str)) dict[str] = new Node() { Name = str, Type = 3 };
                    return dict[str];
                });
                foreach (var conjunction in dict[entry.Name].Nexts.Where(x => x.Type == 2))
                {
                    conjunction.MostRecent[entry.Name] = false;
                }
            }
            return dict;
        }

        internal long Part1(string[] input)
        {
            var dict = Dictionary(input);
            long[] result = [0, 0];
            var button = new Node() { Name = "button", Nexts = [dict["broadcaster"]] };
            var queue = new Queue<Pulse>();
            for (int i = 0; i < 1000; i++)
            {
                long[] count = [0, 0];
                queue.Enqueue(new Pulse(button, false, dict["broadcaster"]));
                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    foreach (var enq in current.To.Propagate(current.From.Name, current.High))
                    {
                        queue.Enqueue(enq);
                    }
                    count[current.High ? 1 : 0]++;
                }
                result[0] += count[0];
                result[1] += count[1];
            }
            return (long)result[0] * result[1];
        }

        internal long Part2(string[] input)
        {
            var dict = Dictionary(input);

            var button = new Node() { Name = "button", Nexts = [dict["broadcaster"]] };
            var queue = new Queue<Pulse>();
            var periods = new Dictionary<string, int>();
            for (int i = 1; i < 10_000; i++)
            {
                queue.Enqueue(new Pulse(button, false, dict["broadcaster"]));
                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    if (current.To.Name == "zr" && current.High && !periods.ContainsKey(current.From.Name))
                    {
                        periods[current.From.Name] = i;
                    }
                    foreach (var enq in current.To.Propagate(current.From.Name, current.High))
                    {
                        queue.Enqueue(enq);
                    }
                }
            }
            return (long)periods
                .Select(x => new BigInteger(x.Value))
                .Aggregate((x, y) => x * y / BigInteger.GreatestCommonDivisor(x, y));
        }

        private record Pulse(Node From, bool High, Node To)
        {
            public override string ToString() => $"{From.Name} -{(High ? "high" : "low")}-> {To.Name}";
        };


        private class Node
        {
            public string Name { get; init; } = string.Empty;
            //0 = normal, 1 = flipflop, 2 = conjunction, 3 = nothing
            public int Type { get; init; }
            public Dictionary<string, bool> MostRecent { get; set; } = new();
            public bool High { get; set; } = false;
            public IEnumerable<Node> Nexts { get; set; } = Array.Empty<Node>();
            public override string ToString() => $"Name: {Name}, Type: {Type}";

            public IEnumerable<Pulse> Propagate(string from, bool high)
            {
                if (MostRecent.ContainsKey(from))
                {
                    MostRecent[from] = high;
                }
                if (Type == 0)
                {
                    return Nexts.Select(x => new Pulse(this, high, x));
                }
                if (Type == 1)
                {
                    if (high) return Array.Empty<Pulse>();
                    High = !High;
                    return Nexts.Select(x => new Pulse(this, High, x));
                }
                if (Type == 2)
                {
                    if (MostRecent.All(x => x.Value))
                        return Nexts.Select(x => new Pulse(this, false, x));
                    return Nexts.Select(x => new Pulse(this, true, x));
                }
                if (Type == 3)
                {
                    return Array.Empty<Pulse>();
                }
                throw new Exception();
            }
        }
    }
}