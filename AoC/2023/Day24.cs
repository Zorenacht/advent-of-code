
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Tools.Geometry;

namespace AoC_2023;

public sealed class Day24 : Day
{
    [Puzzle(answer: null)]
    public long Part1Example() => new Collision().Part1(InputExample, 7, 27);

    [Puzzle(answer: null)]
    public long Part1() => new Collision().Part1(Input, 200_000_000_000_000L, 400_000_000_000_000L);

    [Puzzle(answer: null)]
    public long Part2Example() => new Collision().Part1(InputExample, 7, 27);

    [Puzzle(answer: null)]
    public long Part2() => new Collision().Part1(Input, 7, 27);

    private class Collision
    {
        internal long Part1(string[] input, long min, long max)
        {
            var parsed = input.Select(x =>
            {
                var split = x.Split("@,".ToCharArray(), StringSplitOptions.TrimEntries);
                return new Line2D(
                    new Point2D(long.Parse(split[0]), long.Parse(split[1])),
                    new Point2D(long.Parse(split[3]), long.Parse(split[4])));
            }).ToList();

            int count = 0;
            for (int i = 0; i < parsed.Count; i++)
            {
                for (int j = i + 1; j < parsed.Count; j++)
                {
                    var intersection = parsed[i].Intersect(parsed[j]);
                    if (intersection is null)
                    {
                        continue; //parallel
                    }
                    var time1 = parsed[i].IntersectionTime(intersection.Value);
                    var time2 = parsed[j].IntersectionTime(intersection.Value);
                    if (time1 is null || time2 is null)
                    {
                        continue; //should not happen
                    }
                    if (time1!.Value < 0 || time2!.Value < 0)
                    {
                        continue; //in past
                    }
                    if (min <= intersection.Value.X && intersection.Value.X <= max 
                        && min <= intersection.Value.Y && intersection.Value.Y <= max)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private struct Point2D
        {
            internal double X;
            internal double Y;

            public Point2D(double x, double y)
            {
                X = x;
                Y = y;
            }

            public override string ToString() => $"({X},{Y})";

            public static Point2D operator +(Point2D left, Point2D right) => new Point2D(left.X + right.X, left.Y + right.Y);
            public static Point2D operator -(Point2D left, Point2D right) => new Point2D(left.X - right.X, left.Y - right.Y);
            public static Point2D operator *(double left, Point2D right) => new Point2D(left * right.X, left * right.Y);
            public static Point2D operator *(Point2D left, double right) => right * left;
            public static Point2D operator /(Point2D left, double right) => new Point2D(left.X / right, left.Y / right);
            public static double? operator /(Point2D left, Point2D right)
            {
                var x = left.X / right.X;
                var y = left.Y / right.Y;
                return Math.Abs(y - x) < 0.00000000001 ? x : null;
            }
        }

        private record Line2D(Point2D Point, Point2D Dir)
        {
            public Point2D UDir = Dir / Dir.X;

            public Point2D? Intersect(Line2D line)
            {
                var p1 = Point;
                var p2 = Point + Dir;
                var p3 = line.Point;
                var p4 = line.Point + line.Dir;
                var xNumerator = (p1.X * p2.Y - p1.Y * p2.X) * (p3.X - p4.X) - (p1.X - p2.X) * (p3.X * p4.Y - p3.Y * p4.X);
                var yNumerator = (p1.X * p2.Y - p1.Y * p2.X) * (p3.Y - p4.Y) - (p1.Y - p2.Y) * (p3.X * p4.Y - p3.Y * p4.X);
                var denom = (p1.X - p2.X) * (p3.Y - p4.Y) - (p1.Y - p2.Y) * (p3.X - p4.X);

                if (denom == 0) return null;
                return new Point2D(xNumerator / denom, yNumerator / denom);
            }

            public double? IntersectionTime(Point2D point) => (point - Point) / Dir;
        }
    }
}