using MathNet.Numerics;
using System.Runtime.InteropServices;
using Tools.Geometry;

namespace AoC_2022;

public sealed partial class Day09 : Day
{
    [Test]
    public void Example()
    {
        var visited = new List<Point>();
        var head = new Point(49, 49);
        var tail = new Point(49, 49);
        visited.Add(tail);
        foreach (string line in InputExample)
        {
            var split = line.Split(' ');
            var dir = split[0];
            var amount = int.Parse(split[1]);
            var direction = dir switch
            {
                "R" => Direction.E,
                "L" => Direction.W,
                "U" => Direction.N,
                "D" => Direction.S,
                _ => throw new IndexOutOfRangeException(),
            };
            for (int i = 0; i < amount; i++)
            {
                head = head.Neighbor(direction);
                var diff = head.Difference(tail);
                if (diff.Norm > 2)
                {
                    tail = tail.Neighbor(diff.GeneralDirection);
                    if (!visited.Contains(tail))
                    {
                        visited.Add(tail);
                    }
                }
            }/*
            Console.WriteLine("---------");
            Console.WriteLine(head);
            Console.WriteLine(tail);*/
        }
        visited.Count().Should().Be(13);
    }

    [Test]
    public void Part1()
    {
        var visited = new List<Point>();
        var head = new Point(49, 49);
        var tail = new Point(49, 49);
        visited.Add(tail);
        foreach (string line in InputPart1)
        {
            var split = line.Split(' ');
            var dir = split[0];
            var amount = int.Parse(split[1]);
            var direction = dir switch
            {
                "R" => Direction.E,
                "L" => Direction.W,
                "U" => Direction.N,
                "D" => Direction.S,
                _ => throw new IndexOutOfRangeException(),
            };
            for (int i = 0; i < amount; i++)
            {
                head = head.Neighbor(direction);
                var diff = head.Difference(tail);
                if (diff.Norm > 2)
                {
                    tail = tail.Neighbor(diff.GeneralDirection);
                    if (!visited.Contains(tail))
                    {
                        visited.Add(tail);
                    }
                }
            }/*
            Console.WriteLine("---------");
            Console.WriteLine(head);
            Console.WriteLine(tail);*/
        }
        visited.Count().Should().Be(6044);
    }

    [Test]
    public void Part2()
    {

        var visited = new List<Point>();
        var rope = Enumerable.Repeat(new Point(0,0), 10).ToArray();
        visited.Add(rope[^1]);
        foreach (string line in InputPart1)
        {
            var split = line.Split(' ');
            var dir = split[0];
            var amount = int.Parse(split[1]);
            var direction = dir switch
            {
                "R" => Direction.E,
                "L" => Direction.W,
                "U" => Direction.N,
                "D" => Direction.S,
                _ => throw new IndexOutOfRangeException(),
            };
            for (int i = 0; i < amount; i++)
            {
                rope[0] = rope[0].Neighbor(direction);
                UpdateTail(rope);
                if (!visited.Contains(rope[^1]))
                {
                    visited.Add(rope[^1]);
                }
            }
            Console.WriteLine("---------");
            Console.WriteLine(rope[0]);
            Console.WriteLine(rope[^1]);
        }
        visited.Count().Should().Be(2384);
    }

    private void UpdateTail(Point[] rope)
    {
        for (int i = 1; i < rope.Length; i++)
        {
            var diff = rope[i - 1].Difference(rope[i]);
            if (diff.Norm > 2)
            {
                rope[i] = rope[i].Neighbor(diff.GeneralDirection);
            }
        }
    }
}