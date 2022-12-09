using MathNet.Numerics;
using System.Runtime.InteropServices;
using Tools.Geometry;

namespace AoC_2022;

public sealed partial class Day09 : Day
{
    [Test]
    public void Example()
    {
        var rope = Enumerable.Repeat(new Point(0, 0), 2).ToArray();
        var visited = new HashSet<Point>() { rope[^1] };
        foreach (string line in InputExample)
        {
            var split = line.Split(' ');
            var dir = StringToDirection(split[0]);
            var amount = int.Parse(split[1]);
            for (int i = 0; i < amount; i++)
            {
                rope[0] = rope[0].Neighbor(dir);
                UpdateTail(rope);
                if (!visited.Contains(rope[^1]))
                {
                    visited.Add(rope[^1]);
                }
            }
        }
        visited.Count().Should().Be(13);
    }

    [Test]
    public void Part1()
    {
        var rope = Enumerable.Repeat(new Point(0, 0), 2).ToArray();
        var visited = new HashSet<Point>() { rope[^1] };
        foreach (string line in InputPart1)
        {
            var split = line.Split(' ');
            var dir = StringToDirection(split[0]);
            var amount = int.Parse(split[1]);
            for (int i = 0; i < amount; i++)
            {
                rope[0] = rope[0].Neighbor(dir);
                UpdateTail(rope);
                if (!visited.Contains(rope[^1]))
                {
                    visited.Add(rope[^1]);
                }
            }
        }
        visited.Count().Should().Be(6044);
    }

    [Test]
    public void Part2()
    {

        var rope = Enumerable.Repeat(new Point(0, 0), 10).ToArray();
        var visited = new HashSet<Point>() { rope[^1] };
        foreach (string line in InputPart1)
        {
            var split = line.Split(' ');
            var dir = StringToDirection(split[0]);
            var amount = int.Parse(split[1]);
            for (int i = 0; i < amount; i++)
            {
                rope[0] = rope[0].Neighbor(dir);
                UpdateTail(rope);
                if (!visited.Contains(rope[^1]))
                {
                    visited.Add(rope[^1]);
                }
            }
        }
        visited.Count().Should().Be(2384);
    }

    private Direction StringToDirection(string s) => s switch
    {
        "R" => Direction.E,
        "L" => Direction.W,
        "U" => Direction.N,
        "D" => Direction.S,
        _ => throw new IndexOutOfRangeException(),
    };

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