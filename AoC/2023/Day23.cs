namespace AoC_2023;

public sealed class Day23 : Day
{
    private Algorithm Algo = Algorithm.DFS;

    [Puzzle(answer: 94)]
    public long Part1Example() => new AClass(Algo).Part1(InputExample, false);

    [Puzzle(answer: 2442)]
    public long Part1() => new AClass(Algo).Part1(Input, false);

    [Puzzle(answer: 154)]
    public long Part2Example() => new AClass(Algo).Part1(InputExample, true);

    [Puzzle(answer: 6898)]
    public long Part2() => new AClass(Algo).Part1(Input, true);

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
        private bool Part2;
        private HashSet<int> VerticesInt = new();
        private Dictionary<(int, int), int> Vertices = new();
        private Dictionary<int, Dictionary<int, int>> Edges = new();
        private (int, int) End = new();

        public AClass(Algorithm algorithm)
        {
            Algorithm = algorithm;
        }

        internal long Part1(string[] input, bool part2)
        {
            Part2 = part2;
            input = input.Reverse().ToArray();
            var start = (1, input.Length - 1);
            End = (input[0].Length - 2, 0);

            //reduce graph
            Vertices = FindVertices(input);
            Vertices.Add(start,0);
            Vertices.Add(End, Vertices.Count);
            VerticesInt = Vertices.Select(x => x.Value).ToHashSet();

            //find all edges in graph
            foreach (var vertex in Vertices.Keys)
            {
                var dict = new Dictionary<int, int>();
                Nexts(vertex, (0, 0), 0, input, dict);
                Edges[Vertices[vertex]] = dict;
            }
            return Algorithm == Algorithm.DFS
                ? DfsPaths(current: 0, distance: 0, visited: 1L)
                : BfsPaths(current: 0);
        }

        int BfsPaths(int current)
        {
            int max = 0;
            var dict = new Dictionary<(int, long), int>(3_000_000);
            var currentIteration = new Dictionary<(int, long), int>()
            {
                { (current, 1L), 0 }
            };
            int count = 0;
            while (currentIteration.Count > 0)
            {
                var nextIteration = new Dictionary<(int, long), int>(3_000_000);
                foreach (var state in currentIteration.Keys)
                {
                    //all next vertices for current state
                    foreach (var next in Edges[state.Item1])
                    {
                        var index = next.Key;
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
                var endpoints = currentIteration.Where(x => x.Key.Item1 == VerticesInt.Count-1);
                if (endpoints.Any()) max = Math.Max(max, endpoints.Max(x => x.Value));
                currentIteration = nextIteration;
            }
            return max;
        }

        int DfsPaths(
            int current,
            int distance,
            long visited)
        {
            if (current == VerticesInt.Count - 1) return distance;
            int max = 0;
            foreach (var next in Edges[current])
            {
                var index = next.Key;
                if (((visited >> index) & 1) == 0)
                {
                    max = Math.Max(max, DfsPaths(next.Key, distance + next.Value, visited + (1L << index)));
                }
            }
            return max;
        }

        void Nexts((int, int) current, (int, int) lastDir, int distance, string[] input, Dictionary<int, int> toDistances)
        {
            if (distance > 0 && Vertices.ContainsKey(current))
            {
                toDistances.Add(Vertices[current], distance);
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

        private Dictionary<(int, int), int> FindVertices(string[] input)
        {
            var kruispunten = new Dictionary<(int, int), int>();
            int index = 1;
            for (int i = 1; i < input.Length - 1; i++)
            {
                for (int j = 1; j < input[0].Length - 1; j++)
                {
                    if (input[i][j] == '.')
                    {
                        int sum = Enumerable.Range(0, 4).Count(k => input[i + YDir[k]][j + XDir[k]] != '#');
                        if (sum > 2) kruispunten.Add((j, i), index++);
                    }
                }
            }
            return kruispunten;
        }
    }
}