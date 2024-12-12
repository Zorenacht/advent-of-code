namespace AoC._2023;

public sealed class Day12 : Day
{
    [Puzzle(answer: 21)]
    public long Part1Example()
        => InputExample.Select(line => new HotSprings(line, 1).Arrangements()).Sum();

    [Puzzle(answer: 7251)]
    public long Part1()
        => Input.Select(line => new HotSprings(line, 1).Arrangements()).Sum();


    [Puzzle(answer: 525152)]
    public long Part2Example()
        => InputExample.Select(line => new HotSprings(line, 5).Arrangements()).Sum();

    [Puzzle(answer: 2128386729962)]
    public long Part2()
        => Input.Select(line => new HotSprings(line, 5).Arrangements()).Sum();

    public class HotSprings
    {
        private string template;
        private int[] sequence;

        public HotSprings(string line, int repeat)
        {
            var singleTemplate = line.Split()[0];
            var singleSequence = line.Split()[1].Split(",").Select(x => int.Parse(x)).ToArray();
            template = string.Join("?", Enumerable.Repeat(singleTemplate, repeat));
            sequence = Enumerable.Repeat(singleSequence, repeat).SelectMany(x => x).ToArray();
        }

        private record State(int Length)
        {
            private long[] States = new long[Length];

            public long this[int i]
            {
                get { return States[i]; }
                set { States[i] = value; }
            }

            public override string ToString()
            {
                return string.Join(",", States);
            }
        }

        private State[] States(int length)
        {
            var states = new State[length + 1];
            for (int i = 0; i < length; i++)
            {
                states[i] = new State(sequence[i] + 1);
            }
            states[^1] = new State(1);
            return states;
        }

        // Time complexity: O(template.Length * sequence.Max * sequence.Length)
        // Space complexity: O(sequence.Max * sequence.Length)
        public long Arrangements()
        {
            Console.WriteLine($"Line length: {template.Length}");
            var states = States(sequence.Length);
            states[0][0] = 1;
            Print(states);
            for (int templateIndex = 0; templateIndex < template.Length; templateIndex++)
            {
                var next = States(sequence.Length);
                var ch = template[templateIndex];
                if (ch == '.' || ch == '?')
                {
                    for (int i = 0; i < states.Length; i++)
                    {
                        next[i][0] += states[i][0];
                        if (i < states.Length - 1) next[i + 1][0] += states[i][^1];
                    }
                }
                if (ch == '#' || ch == '?')
                {
                    for (int i = 0; i < states.Length; i++)
                    {
                        for (int j = 0; j < states[i].Length - 1; j++)
                        {
                            next[i][j + 1] += states[i][j];
                        }
                    }
                }
                states = next;
                Print(states, $"After applying:{ch}");
            }
            return states[^2][^1] + states[^1][^1];
        }

        private void Print(State[] states, string title = "---------")
        {
            Console.WriteLine($"-------{title}-------");
            foreach (var state in states)
            {
                Console.WriteLine(state.ToString());
            }
        }
    }
}