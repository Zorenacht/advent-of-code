using FluentAssertions.Equivalency.Steps;
using MathNet.Numerics;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Constraints;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Tools.Shapes;

namespace AoC_2023;

public sealed class Day23 : Day
{
    [Puzzle(answer: null)]
    public long Part1Example() => new AClass().Part1(InputExample);

    //2274 too low
    [Puzzle(answer: null)]
    public long Part1() => new AClass().Part1(Input);

    [Puzzle(answer: null)]
    public long Part2Example() => new AClass().Part1(InputExample);

    [Puzzle(answer: null)]
    public long Part2() => new AClass().Part1(Input);


    private class AClass
    {
        private int[] XDir = [0, -1, 0, 1];
        private int[] YDir = [1, 0, -1, 0];

        internal long Part1(string[] input)
        {
            input = input.Reverse().ToArray();
            var start = (1, input.Length - 1);
            var end = (input[0].Length - 2, 0);

            var crosspoints = FindCrosspoints(input);
            crosspoints.Insert(0, start);
            crosspoints.Add(end);
            var connections = new Dictionary<(int, int), Dictionary<(int, int), int>>();

            var queue = new Queue<(int, int)>();
            queue.Enqueue(start);
            while (queue.TryDequeue(out var from))
            {
                var toDistances = new Dictionary<(int, int), int>();
                Nexts(from, (0, -1), 0, crosspoints, input, toDistances);
                connections[from] = toDistances;
                foreach (var pair in toDistances.Keys.Except(connections.Keys))
                {
                    queue.Enqueue(pair);
                }
            }

            var distances = new List<int>();
            Paths(start, 0, 0, end, connections, distances);
            distances.Sort();
            return distances.Max();
        }

        void Paths(
            (int, int) current,
            int distance,
            long visited,
            (int, int) to,
            Dictionary<(int, int), Dictionary<(int, int), int>> connections,
            List<int> distances)
        {
            if (current == to) distances.Add(distance);
            var list = connections.Select(x => x.Key).ToList();
            foreach (var next in connections[current])
            {
                var index = list.IndexOf(current);
                if(((visited >> index) & 1) == 0)
                {
                    Paths(next.Key, distance + next.Value, visited + (1 << index), to, connections, distances);
                }
            }
        }

        void Nexts((int, int) current, (int, int) lastDir, int distance,
            List<(int, int)> crosspoints, string[] input, Dictionary<(int, int), int> toDistances)
        {
            if (distance > 0 && crosspoints.Contains(current))
            {
                toDistances.Add(current, distance);
                return;
            }

            for (int k = 0; k < 4; k++)
            {
                var next = (current.Item1 + XDir[k], current.Item2 + YDir[k]);
                if (next.Item1 >= 0 && next.Item1 <= input[0].Length - 1
                 && next.Item2 >= 0 && next.Item2 <= input.Length - 1
                 && lastDir != (-XDir[k], -YDir[k])
                 && input[next.Item2][next.Item1] != '#' && Allowed((XDir[k], YDir[k]), input[next.Item2][next.Item1]))
                {
                    Nexts(next, (XDir[k], YDir[k]), distance + 1, crosspoints, input, toDistances);
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

        private List<(int, int)> FindCrosspoints(string[] input)
        {
            var kruispunten = new List<(int, int)>();
            for (int i = 1; i < input.Length - 1; i++)
            {
                for (int j = 1; j < input[0].Length - 1; j++)
                {
                    if (input[i][j] == '.')
                    {
                        bool cross = true;
                        int sum = Enumerable.Range(0, 4).Count(k => input[i + YDir[k]][j + XDir[k]] != '#');
                        for (int k = 0; k < 4; k++)
                        {
                            if (".".Contains(input[i + YDir[k]][j + XDir[k]]))
                            {
                                cross = false;
                            }
                        }
                        if (sum > 2) kruispunten.Add((j, i));
                        //if (cross) kruispunten.Add((j, i));
                    }
                }
            }
            return kruispunten;
        }

        internal long Part2(string[] input)
        {
            var parsed = input.Select(x =>
            {
                return x;
            });
            return 0;
        }
    }
}