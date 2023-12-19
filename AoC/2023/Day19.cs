using FluentAssertions.Equivalency.Steps;
using MathNet.Numerics;
using System.Net.Http.Headers;
using System.Numerics;
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
        public Day19Class()
        {
        }


        private record State
        {
            public string Name { get; init; }
            public List<(string Variable, Func<int, string?> Condition)> Maps = new();
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
                    if (cmds[i].Contains('<')) Maps.Add((cond, (int v) => v < val ? dest : null));
                    else Maps.Add((cond, (int v) => v > val ? dest : null));
                }
                Last = cmds[^1];

            }
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
            for (i = i; i < input.Length; i++)
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
                    name = Loop(maps, values, name);
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

        internal long Part2(string[] inputExample)
        {
            long result = 0;
            return result;
        }
    }
}