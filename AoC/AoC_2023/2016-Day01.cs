using FluentAssertions;
using System.Security.AccessControl;
using Tools.Geometry;

namespace AoC_2023;

public sealed class Day01 : Day
{
    [Test]
    public void Part1()
    {
        int result = 0;
        var parse = InputAsText
            .Split(", ")
            .Select(x => (x[0], int.Parse(x[1..].ToString())))
            .ToList();
        int up = 0;
        int right = 0;
        int dir = 0;
        var lines = new List<Line>();
        foreach(var ele in parse)
        {
            dir = ele.Item1 == 'R' ? dir + 1 : dir + 3;
            var start = new Point(up, right);
            if (dir % 4 == 0) up += ele.Item2;
            if (dir % 4 == 1) right += ele.Item2;
            if (dir % 4 == 2) up -= ele.Item2;
            if (dir % 4 == 3) right -= ele.Item2;
            var end = new Point(up, right);
            if (lines.Any(l => l.Contains(end)))
            {
                result = Math.Abs(end.X) + Math.Abs(end.Y);
                break;
            }
            lines.Add(new Line(start,end));
        }
        //result = Math.Abs(up) + Math.Abs(right);
        result.Should().Be(0);
    }

    public record Line(Point Start, Point End)
    {
        public bool Contains(Point point) =>
            point.X == Start.X &&
                (Start.Y <= point.Y && point.Y <= End.Y || End.Y <= point.Y && point.Y <= Start.Y)
            || point.Y == Start.Y &&
                (Start.X <= point.X && point.X <= End.X || End.X <= point.X && point.X <= Start.X);
    }

    [Test]
    public void Part2()
    {
        int result = 0;
        var parse = Input.Select(Int32.Parse).ToArray();
        var set = new HashSet<int>(parse);
        result.Should().Be(0);
    }
}