using FluentAssertions.Equivalency.Steps;
using MathNet.Numerics;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
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
    public long Part1Example() => new AClass(false).Part1(InputExample);

    //2274 too low
    [Puzzle(answer: 2442)]
    public long Part1() => new AClass(false).Part1(Input);

    [Puzzle(answer: 154)]
    public long Part2Example() => new AClass(true).Part1(InputExample);

    [Puzzle(answer: 6898)]
    public long Part2() => new AClass(true).Part1(Input);


    private class AClass
    {
        private int[] XDir = [0, -1, 0, 1];
        private int[] YDir = [1, 0, -1, 0];
        private readonly bool Part2;
        private List<(int, int)> Vertices = new();
        private Dictionary<(int, int), Dictionary<(int, int), int>> Edges = new();

        public AClass(bool part2)
        {
            Part2 = part2;
        }

        internal long Part1(string[] input)
        {
            input = input.Reverse().ToArray();
            var start = (1, input.Length - 1);
            var end = (input[0].Length - 2, 0);

            //reduce graph
            Vertices = FindVertices(input);
            Vertices.Insert(0, start);
            Vertices.Add(end);

            //find all edges in graph
            foreach (var vertex in Vertices)
            {
                var dict = new Dictionary<(int, int), int>();
                Nexts(vertex, (0, 0), 0, input, dict);
                Edges[vertex] = dict;
            }

            //find all paths
            var distances = new List<int>();
            Paths(start, 0, 1L, distances, end);
            return distances.Count;
        }

        void Paths(
            (int, int) current,
            int distance,
            long visited,
            List<int> distances,
            (int, int) to)
        {
            if (current == to) distances.Add(distance);
            foreach (var next in Edges[current])
            {
                var index = Vertices.IndexOf(next.Key);
                if (((visited >> index) & 1) == 0)
                {
                    Paths(next.Key, distance + next.Value, visited + (1L << index), distances, to);
                }
            }
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