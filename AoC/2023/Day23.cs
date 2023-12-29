namespace AoC_2023;

public sealed class Day23 : Day
{
    private Algorithm Algo = Algorithm.DFS;

    [Puzzle(answer: 94)]
    public long Part1Example() => new AClass(false, Algo).Part1(InputExample);

    [Puzzle(answer: 2442)]
    public long Part1() => new AClass(false, Algo).Part1(Input);

    [Puzzle(answer: 154)]
    public long Part2Example() => new AClass(true, Algo).Part1(InputExample);

    [Puzzle(answer: 6898)]
    public long Part2() => new AClass(true, Algo).Part1(Input);

    private enum Algorithm
    {
        DFS,
        BFS
    }

    private class AClass
    {
        private int[] XDir = [0, -1, 0, 1];
        private int[] YDir = [1, 0, -1, 0];
        private readonly Algorithm Algorithm;
        private readonly bool Part2;
        private List<(int, int)> Vertices = new();
        private Dictionary<(int, int), Dictionary<(int, int), int>> Edges = new();
        private (int, int) End = new();

        public AClass(bool part2, Algorithm algorithm)
        {
            Part2 = part2;
            Algorithm = algorithm;
        }

        internal long Part1(string[] input)
        {
            input = input.Reverse().ToArray();
            var start = (1, input.Length - 1);
            End = (input[0].Length - 2, 0);

            //reduce graph
            Vertices = FindVertices(input);
            Vertices.Insert(0, start);
            Vertices.Add(End);

            //find all edges in graph
            foreach (var vertex in Vertices)
            {
                var dict = new Dictionary<(int, int), int>();
                Nexts(vertex, (0, 0), 0, input, dict);
                Edges[vertex] = dict;
            }

            //find all paths
            return Algorithm == Algorithm.DFS
                ? DfsPaths(start, 0, 1L)
                : BfsPaths(start);
        }

        int BfsPaths(
            (int, int) current)
        {
            int max = 0;
            var dict = new Dictionary<((int, int), long), int>();
            var currentIteration = new Dictionary<((int, int), long), int>()
            {
                { (current, 1L), 0 }
            };
            int count = 0;
            while (currentIteration.Count > 0)
            {
                var nextIteration = new Dictionary<((int, int), long), int>();
                foreach (var state in currentIteration.Keys)
                {
                    //all next vertices for current state
                    foreach (var next in Edges[state.Item1])
                    {
                        var index = Vertices.IndexOf(next.Key);
                        if (((state.Item2 >> index) & 1) == 0)
                        {
                            var nextState = (next.Key, state.Item2 + (1L << index));
                            if (!nextIteration.ContainsKey(nextState))
                            {
                                nextIteration[nextState] = currentIteration[state] + next.Value;
                            }
                            else
                            {
                                count++;
                                nextIteration[nextState] = Math.Max(nextIteration[nextState], currentIteration[state] + next.Value);
                            }
                        }
                    }

                }
                var endpoints = currentIteration.Where(x => x.Key.Item1 == End);
                if (endpoints.Any()) max = Math.Max(max, endpoints.Max(x => x.Value));
                currentIteration = nextIteration;
            }
            return max;
        }

        int DfsPaths(
            (int, int) current,
            int distance,
            long visited)
        {
            if (current == End) return distance;
            int max = 0;
            foreach (var next in Edges[current])
            {
                var index = Vertices.IndexOf(next.Key);
                if (((visited >> index) & 1) == 0)
                {
                    max = Math.Max(max, DfsPaths(next.Key, distance + next.Value, visited + (1L << index)));
                }
            }
            return max;
        }

        void Nexts((int, int) current, (int, int) lastDir, int distance, string[] input, Dictionary<(int, int), int> toDistances)
        {
            if (distance > 0 && Vertices.Contains(current))
            {
                toDistances.Add(current, distance);
                return;
            }

            for (int k = 0; k < 4; k++)
            {
                var dir = (XDir[k], YDir[k]);
                var next = (current.Item1 + dir.Item1, current.Item2 + dir.Item2);
                if (next.Item1 >= 0 && next.Item1 <= input[0].Length - 1
                 && next.Item2 >= 0 && next.Item2 <= input.Length - 1
                 && lastDir != (-dir.Item1, -dir.Item2)
                 && input[next.Item2][next.Item1] != '#' && (Allowed(dir, input[next.Item2][next.Item1]) || Part2))
                {
                    Nexts(next, dir, distance + 1, input, toDistances);
                }
            }
        }

        private bool Allowed((int, int) dir, char ch)
        {
            return ch switch
            {
                '^' => dir != (0, -1),
                '<' => dir != (1, 0),
                'v' => dir != (0, 1),
                '>' => dir != (-1, 0),
                '.' => true,
                _ => throw new Exception()
            };
        }

        private List<(int, int)> FindVertices(string[] input)
        {
            var kruispunten = new List<(int, int)>();
            for (int i = 1; i < input.Length - 1; i++)
            {
                for (int j = 1; j < input[0].Length - 1; j++)
                {
                    if (input[i][j] == '.')
                    {
                        int sum = Enumerable.Range(0, 4).Count(k => input[i + YDir[k]][j + XDir[k]] != '#');
                        if (sum > 2) kruispunten.Add((j, i));
                    }
                }
            }
            return kruispunten;
        }
    }
}