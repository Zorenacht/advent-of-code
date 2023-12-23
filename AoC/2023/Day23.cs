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
    [Puzzle(answer: 94)]
    public long Part1Example() => new AClass().Part1(InputExample);

    //2274 too low
    [Puzzle(answer: 2442)]
    public long Part1() => new AClass().Part1(Input);

    [Puzzle(answer: 154)]
    public long Part2Example() => new AClass().Part1(InputExample, true);

    [Puzzle(answer: 6898)]
    public long Part2() => new AClass().Part1(Input, true);


    private class AClass
    {
        private int[] XDir = [0, -1, 0, 1];
        private int[] YDir = [1, 0, -1, 0];
        private bool _part2 = false;

        internal long Part1(string[] input, bool p2 = false)
        {
            _part2 = p2;
            input = input.Reverse().ToArray();
            var start = (1, input.Length - 1);
            var end = (input[0].Length - 2, 0);

            //reduce graph
            var vertices = FindCrosspoints(input);
            vertices.Insert(0, start);
            vertices.Add(end);

            //find all edges in graph
            var connections = new Dictionary<(int, int), Dictionary<(int, int), int>>();
            var enqueued = new HashSet<(int, int)>();
            var queue = new Queue<(int, int)>();
            queue.Enqueue(start);
            foreach(var vertex in vertices)
            {
                var dict = new Dictionary<(int, int), int>();
                Nexts(vertex, (0, 0), 0, vertices, input, dict);
                connections[vertex] = dict;
            }

            //find all paths
            var distances = new List<int>();
            Paths(start, 0, 1L, end, connections, distances);
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
                var index = list.IndexOf(next.Key);
                if (((visited >> index) & 1) == 0)
                {
                    Paths(next.Key, distance + next.Value, visited + (1L << index), to, connections, distances);
                }
            }
        }
/*
        Dictionary<(int, int), int> Nexts2(
            (int, int) current,
            (int, int) lastDir,
            List<(int, int)> crosspoints,
            string[] input)
        {
            var dict = new Dictionary<(int, int), int>();
            var queue = new Queue<((int, int), (int, int), int)>();
            queue.Enqueue((current, lastDir, 0));
            while (queue.TryDequeue(out var result))
            {
                var point = result.Item1;
                lastDir = result.Item2;
                var distance = result.Item3;
                if (crosspoints.Contains(point) && distance > 0) { 
                    dict.Add(point, distance);
                    continue;
                }
                for (int k = 0; k < 4; k++)
                {
                    var dir = (XDir[k], YDir[k]);
                    var next = (point.Item1 + dir.Item1, point.Item2 + dir.Item2);
                    if (next.Item1 >= 0 && next.Item1 <= input[0].Length - 1
                        && next.Item2 >= 0 && next.Item2 <= input.Length - 1
                        && lastDir != (-dir.Item1, -dir.Item2)
                        && input[next.Item2][next.Item1] != '#' && (Allowed(dir, input[next.Item2][next.Item1]) || _part2))
                    {
                        queue.Enqueue((next, dir, distance + 1));
                    }
                }
            }
            return dict;
        }*/

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
                var dir = (XDir[k], YDir[k]);
                var next = (current.Item1 + dir.Item1, current.Item2 + dir.Item2);
                if (next.Item1 >= 0 && next.Item1 <= input[0].Length - 1
                 && next.Item2 >= 0 && next.Item2 <= input.Length - 1
                 && lastDir != (-dir.Item1, -dir.Item2)
                 && input[next.Item2][next.Item1] != '#' && (Allowed(dir, input[next.Item2][next.Item1]) || _part2))
                {
                    Nexts(next, dir, distance + 1, crosspoints, input, toDistances);
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
                        int sum = Enumerable.Range(0, 4).Count(k => input[i + YDir[k]][j + XDir[k]] != '#');
                        if (sum > 2) kruispunten.Add((j, i));
                    }
                }
            }
            return kruispunten;
        }
    }
}