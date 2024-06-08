using Tools.Shapes;

namespace AoC_2023;

public sealed class Day19 : Day
{
    [Puzzle(answer: 19114)]
    public long Part1Example() => new MachineParts().Part1(InputExample);

    [Puzzle(answer: 425811)]
    public long Part1() => new MachineParts().Part1(Input);

    [Puzzle(answer: 167409079868000)]
    public long Part2Example() => new MachineParts().Part2(InputExample);

    [Puzzle(answer: 131796824371749)]
    public long Part2() => new MachineParts().Part2(Input);

    private class MachineParts
    {
        internal long Part1(string[] input)
        {
            long result = 0;
            var maps = new Dictionary<string, Workflow>();
            int i = 0;
            for (; i < input.Length; i++)
            {
                if (input[i] == string.Empty) break;
                var state = new Workflow(input[i]);
                maps.Add(state.Name, state);
            }
            for (++i; i < input.Length; i++)
            {
                var parsed = input[i].Split("{,=}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var values = new Rating(int.Parse(parsed[1]), int.Parse(parsed[3]), int.Parse(parsed[5]), int.Parse(parsed[7]));
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

        internal long Part2(string[] input)
        {
            var maps = new Dictionary<string, Workflow>();
            for (int i = 0; i < input.Length; i++)
            {
                var line = input[i];
                if (line == string.Empty) break;
                var state = new Workflow(line);
                maps.Add(state.Name, state);
            }
            var interval = new Interval4D(new Interval1D(1, 4000), new Interval1D(1, 4000), new Interval1D(1, 4000), new Interval1D(1, 4000));
            return Accepted("in", interval, maps);
        }


        private static string? Loop(Dictionary<string, Workflow> maps, Rating values, string name)
        {
            var map = maps[name];
            foreach (var m in map.Maps)
            {
                if (m.Next(values[m.PartCategory]) is { } res) return res;
            }
            throw new InvalidProgramException("Wrong input: this should not be reachable as the workflow must end with some default state.");
        }

        private long Accepted(string name, Interval4D interval, Dictionary<string, Workflow> maps)
        {
            if (name == "A") return interval.Size;
            if (name == "R") return 0;
            long sum = 0;
            foreach (var map in maps[name].Maps)
            {
                var intervals = interval.Split(map.PartCategory, map.Symbol, map.Value);
                if (intervals.Valid is { } valid) sum += Accepted(map.Destination, valid, maps);
                if (intervals.Invalid is null) break;
                interval = intervals.Invalid;
            }
            return sum;
        }

        private record Workflow
        {
            public string Name;
            public List<Rule> Maps = new();

            public record Rule(string PartCategory, string Symbol, int Value, string Destination)
            {
                public string? Next(int value) => Symbol switch
                {
                    "<" => value < Value ? Destination : null,
                    ">" => value > Value ? Destination : null,
                    "f" => Destination,
                    _ => throw new InvalidOperationException($"Symbol must be < or >, but is {Symbol}")
                };
            }

            public Workflow(string line)
            {
                var split1 = line.Split('{');
                Name = split1[0];
                var cmds = split1[1][..^1].Split(',');
                for (int i = 0; i < cmds.Length - 1; i++)
                {
                    var split2 = cmds[i].Split("<>:".ToCharArray());
                    var category = split2[0];
                    var symbol = cmds[i].Contains('<') ? "<" : ">";
                    var value = int.Parse(split2[1]);
                    var destination = split2[2];
                    Maps.Add(new Rule(category, symbol, value, destination));
                }
                Maps.Add(new Rule("x", "f", 0, cmds[^1]));
            }
        }

        private class Rating : Dictionary<string, int>
        {
            public long Sum => this.Sum(x => x.Value);

            public Rating(int x, int m, int a, int s)
            {
                Add("x", x);
                Add("m", m);
                Add("a", a);
                Add("s", s);
            }
        }

        private class Interval4D : Dictionary<string, Interval1D>
        {
            public record ValidInterval(Interval4D? Valid, Interval4D? Invalid);

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
                if (symbol == "f")
                {
                    return new ValidInterval(this, null);
                }
                throw new Exception();
            }

            public long Size => this["x"].Length * this["m"].Length * this["a"].Length * this["s"].Length;
        };
    }
}