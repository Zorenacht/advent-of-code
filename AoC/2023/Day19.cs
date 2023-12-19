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

    [Puzzle(answer: 167409079868000)]
    public long Part2Example() => new Day19Class().Part2(InputExample);

    [Puzzle(answer: 131796824371749)]
    public long Part2() => new Day19Class().Part2(Input);

    [Test]
    public void IntervalTests()
    {
        Interval4D interval = new(new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10));
        var result = interval.Split("x", ">", 3);

        Interval4D valid = new(new Interval1D(4, 10), new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10));
        Interval4D invalid = new(new Interval1D(1, 3), new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10));
        var expected = new ValidInterval(valid, invalid);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void IntervalTestsMin()
    {
        Interval4D interval = new(new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10));
        var result = interval.Split("x", ">", 10);

        Interval4D? valid = null;// new Interval4D(new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10));
        Interval4D? invalid = new(new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10));
        var expected = new ValidInterval(valid, invalid);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void IntervalTestsMax()
    {
        Interval4D interval = new(new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10));
        var result = interval.Split("x", ">", 0);

        Interval4D? valid = new(new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10));// new Interval4D(new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10), new Interval1D(1, 10));
        Interval4D? invalid = null;
        var expected = new ValidInterval(valid, invalid);
        result.Should().BeEquivalentTo(expected);
    }


    private record ValidInterval(Interval4D? Valid, Interval4D? Invalid);

    private class Interval4D : Dictionary<string, Interval1D>
    {

        public Interval4D(Interval1D x, Interval1D m, Interval1D a, Interval1D s)
        {
            this["x"] = x;
            this["m"] = m;
            this["a"] = a;
            this["s"] = s;
        }

        public Interval4D(IEnumerable<(string, Interval1D)> list)
        {
            foreach (var entry in list)
            {
                this[entry.Item1] = entry.Item2;
            }
        }


        public ValidInterval Split(string coordinate, string symbol, long value)
        {
            var interval = this[coordinate];
            //(0, 200) and <100  -> (0,99) valid, (100,200) invalid
            if (symbol == "<")
            {
                if (value <= interval.Start) return new ValidInterval(null, this);
                if (interval.End < value) return new ValidInterval(this, null);
                else
                {
                    var lower = new Interval1D(interval.Start, value - 1);
                    var lower4d = this.Select(x => (x.Key, x.Key == coordinate ? lower : x.Value));
                    var upper = new Interval1D(value, interval.End);
                    var upper4d = this.Select(x => (x.Key, x.Key == coordinate ? upper : x.Value));
                    return new ValidInterval(new Interval4D(lower4d), new Interval4D(upper4d));
                }
            }
            //(0, 200) and >100  -> (0,100) invalid, (101,200) valid
            if (symbol == ">")
            {
                if (value < interval.Start) return new ValidInterval(this, null);
                if (interval.End <= value) return new ValidInterval(null, this);
                else
                {
                    var lower = new Interval1D(interval.Start, value);
                    var lower4d = this.Select(x => (x.Key, x.Key == coordinate ? lower : x.Value));
                    var upper = new Interval1D(value + 1, interval.End);
                    var upper4d = this.Select(x => (x.Key, x.Key == coordinate ? upper : x.Value));
                    return new ValidInterval(new Interval4D(upper4d), new Interval4D(lower4d));
                }
            }
            throw new Exception();
        }

        public long Size => x.Length * m.Length * a.Length * s.Length;

        public Interval1D x => this["x"];
        public Interval1D m => this["m"];
        public Interval1D a => this["a"];
        public Interval1D s => this["s"];
    };



    private class Day19Class
    {

        private record State
        {
            public string Name { get; init; }
            public List<(string Variable, string Symbol, int Value, string Destination, Func<int, string?> Condition)> Maps = new();
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
                    if (cmds[i].Contains('<')) Maps.Add((cond, "<", val, dest, (int v) => v < val ? dest : null));
                    else Maps.Add((cond, ">", val, dest, (int v) => v > val ? dest : null));
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
            var maps = new Dictionary<string, State>();
            int i = 0;
            for (; i < input.Length; i++)
            {
                var line = input[i];
                if (line == string.Empty) break;
                var state = new State(line);
                maps.Add(state.Name, state);
            }
            Interval4D interval = new Interval4D(new Interval1D(1, 4000), new Interval1D(1, 4000), new Interval1D(1, 4000), new Interval1D(1, 4000));
            return Accepted("in", interval, maps);
        }

        private long Accepted(string name, Interval4D interval, Dictionary<string, State> maps)
        {
            if (name == "A") return interval.Size;
            if (name == "R") return 0;
            long sum = 0;
            foreach (var map in maps[name].Maps)
            {
                var intervals = interval.Split(map.Variable, map.Symbol, map.Value);
                if (intervals.Valid is { } valid) sum += Accepted(map.Destination, valid, maps);
                if (intervals.Invalid is null) break;
                interval = intervals.Invalid;
            }
            if (interval is { }) sum += Accepted(maps[name].Last, interval, maps);

            return sum;
        }
    }
}