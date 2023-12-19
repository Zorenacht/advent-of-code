using FluentAssertions.Equivalency.Steps;
using MathNet.Numerics;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using Tools.Shapes;
using static AoC_2023.Day17;

namespace AoC_2023;

public sealed class Day19 : Day
{
    [Puzzle(answer: 19114)]
    public long Part1Example() => new Day19Class().Part1(InputExample);

    [Puzzle(answer: 425811)]
    public long Part1() => new Day19Class().Part1(Input);

    [Puzzle(answer: null)]
    public long Part2Example() => new Day19Class().Part2(InputExample);

    [Puzzle(answer: null)]
    public long Part2() => new Day19Class().Part2(Input);


    private class Day19Class
    {
        private class Interval4D : Dictionary<string, Interval1D>
        {

            public Interval4D(Interval1D x, Interval1D m, Interval1D a, Interval1D s)
            {
                this["x"] = this.x;
                this["m"] = this.m;
                this["a"] = this.a;
                this["s"] = this.s;
            }

            public Interval4D(IEnumerable<(string, Interval1D)> list)
            {
                foreach(var entry in list)
                {
                    this[entry.Item1] = entry.Item2;
                }
            }

            public class ValidInterval(Interval4D Valid, Interval4D Invalid);

            //using <
            public IEnumerable<Interval4D> Split(string coordinate, string symbol, long value)
            {
                var interval = this[symbol];
                //(0, 200) and <100  -> (0,99), (100,200)
                if (symbol == "<")
                {
                    if (value < interval.Start || interval.End <= value) yield return this;
                    else
                    {
                        var lower = new Interval1D(interval.Start, value);
                        var lower4d = this.Select(x => (x.Key, x.Key == coordinate ? lower : x.Value));
                        var upper = new Interval1D(value + 1, interval.End);
                        var upper4d = this.Select(x => (x.Key, x.Key == coordinate ? lower : x.Value));
                        yield return new Interval4D(lower4d);
                        yield return new Interval4D(upper4d);
                    }
                }
                //(0, 200) and >100  -> (0,100), (101,200)
                /*if (symbol == ">")
                {
                    if (value < interval.Start || interval.End <= value) yield return this;
                    else
                    {
                        var left = new Interval1D(interval.Start, value);
                        var left4d = this.Select(x => (x.Key, x.Key == coordinate ? left : x.Value));
                        var right = new Interval1D(value + 1, interval.End);
                        var right4d = this.Select(x => (x.Key, x.Key == coordinate ? left : x.Value));
                        yield return new Interval4D(left4d);
                        yield return new Interval4D(right4d);

                    }
                }*/
            }

            public long Size => x.Length * m.Length * a.Length * s.Length;

            public Interval1D x => this["x"];
            public Interval1D m => this["m"];
            public Interval1D a => this["a"];
            public Interval1D s => this["s"];
        };

        private record State
        {
            public string Name { get; init; }
            public List<(string Variable, string Symbol, int Value, Func<int, string?> Condition)> Maps = new();
            public string Last { get; init; }

            public State(string line)
            {
                var split1 = line.Split('{');
                Name = split1[0];
                var cmds = split1[1][..^1].Split(',');
                for (int i = 0; i < cmds.Length - 1; i++)
                {
                    var split2 = cmds[i].Split("<>:".ToCharArray());
                    var cond = split2[0];
                    var val = int.Parse(split2[1]);
                    var dest = split2[2];
                    if (cmds[i].Contains('<')) Maps.Add((cond, "<", val, (int v) => v < val ? dest : null));
                    else Maps.Add((cond, ">", val, (int v) => v > val ? dest : null));
                }
                Last = cmds[^1];

            }

            /*public IEnumerable<(string? Next, Interval1D)> Map(Interval4D interval)
            {
                foreach (var map in Maps)
                {
                    // if {} then its <
                    var splitted = interval.Split(map.Variable, map.Symbol, map.Value).ToArray();
                    if(splitted.Length == 1) 
                }
            }*/
        }

        private class Values : Dictionary<string, int>
        {
            public long Sum => this.Sum(x => x.Value);

            public Values(int x, int m, int a, int s)
            {
                Add("x", x);
                Add("m", m);
                Add("a", a);
                Add("s", s);
            }
        }

        internal long Part1(string[] input)
        {
            long result = 0;
            var maps = new Dictionary<string, State>();
            int i = 0;
            for (; i < input.Length; i++)
            {
                var line = input[i];
                if (line == string.Empty) break;
                var state = new State(line);
                maps.Add(state.Name, state);
            }
            for (++i; i < input.Length; i++)
            {
                var line = input[i];
                var split = line.Split("{,=}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var values = new Values(int.Parse(split[1]), int.Parse(split[3]), int.Parse(split[5]), int.Parse(split[7]));
                var name = "in";
                while (true)
                {
                    if (name == "A")
                    {
                        result += values.Sum;
                        break;
                    }
                    if (name == "R") break;
                    name = Loop(maps, values, name!);
                }
            }
            return result;
        }

        private static string? Loop(Dictionary<string, State> maps, Values values, string name)
        {
            var map = maps[name];
            foreach (var m in map.Maps)
            {
                if (m.Condition(values[m.Variable]) is { } res) return res;
            }
            return map.Last;
        }

        internal long Part2(string[] input)
        {
            long result = 0;
            var maps = new Dictionary<string, State>();
            int i = 0;
            for (; i < input.Length; i++)
            {
                var line = input[i];
                if (line == string.Empty) break;
                var state = new State(line);
                maps.Add(state.Name, state);
            }
            for (++i; i < input.Length; i++)
            {
                var line = input[i];
                var split = line.Split("{,=}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var values = new Values(int.Parse(split[1]), int.Parse(split[3]), int.Parse(split[5]), int.Parse(split[7]));
                var name = "in";
                var interval = new Interval4D(new Interval1D(1, 4000), new Interval1D(1, 4000), new Interval1D(1, 4000), new Interval1D(1, 4000));
                while (true)
                {
                    if (name == "A")
                    {
                        result += values.Sum;
                        break;
                    }
                    if (name == "R") break;
                    name = Loop(maps, values, name!);
                }
            }
            return result;
        }

        internal long Accepted(string name, Interval4D interval, Dictionary<string, State> maps)
        {
            if (name == "A") return interval.Size;
            if (name == "R") return 0;
            long sum = 0;
            foreach(var map in maps[name].Maps)
            {
                var intervals = interval.Split(map.Variable, map.Symbol, map.Value).ToArray();
                if (intervals.Length == 1)
                {

                }
            }

            return Accepted(name, interval, maps);
        }
    }
}