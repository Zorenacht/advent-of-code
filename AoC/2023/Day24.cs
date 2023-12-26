using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Xml;
using Tools.Geometry;

namespace AoC_2023;

public sealed class Day24 : Day
{
    [Puzzle(answer: 2)]
    public long Part1Example() => new Collision().Part1(InputExample, 7, 27);

    [Puzzle(answer: 14046)]
    public long Part1() => new Collision().Part1(Input, 200_000_000_000_000L, 400_000_000_000_000L);

    [Puzzle(answer: 47)]
    public long Part2Example() => new Collision().Part2(InputExample);

    [Puzzle(answer: null)]
    public long Part2() => new Collision().Part2(Input);

    private class Collision
    {
        internal long Part1(string[] input, long min, long max)
        {
            var lines = input.Select(x =>
            {
                var split = x.Split("@,".ToCharArray(), StringSplitOptions.TrimEntries);
                return new Line2D(
                    new Point2D(long.Parse(split[0]), long.Parse(split[1])),
                    new Point2D(long.Parse(split[3]), long.Parse(split[4])));
            }).ToList();

            int count = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = i + 1; j < lines.Count; j++)
                {
                    var intersection = lines[i].IntersectS(lines[j]);
                    var intersectionS = lines[i].IntersectS(lines[j]);
                    if (intersection is null)
                    {
                        if (lines[i].SameLine(lines[j]) && lines[i].LiesInsideBlock(min, max) is { } t1 && t1 >= 0
                            && lines[j].LiesInsideBlock(min, max) is { } t2 && t2 >= 0)
                        {
                            count++;
                            continue;
                        }
                        continue; //parallel and not on same line
                    }
                    var time1 = lines[i].IntersectionTime(lines[j], intersection.Value);
                    var time2 = lines[j].IntersectionTime(lines[i], intersection.Value);
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

        internal long Part2(string[] input)
        {
            var lines = input.Select<string, ((double X, double Y, double Z) P, (double X, double Y, double Z) D)>(x =>
            {
                var split = x.Split("@,".ToCharArray(), StringSplitOptions.TrimEntries);
                return (
                    (double.Parse(split[0]), double.Parse(split[1]), double.Parse(split[2])),
                    (double.Parse(split[3]), double.Parse(split[4]), double.Parse(split[5]))
                    );
            }).ToList();
            var sorted = lines
                .Select(x => new Line2D(new Point2D(x.P.X, x.P.Y), new Point2D(x.D.X, x.D.Y)))
                .Select(x => $"y = {x.A}x + {x.B}")
                .ToList();

            var f = (double t, double t2) =>
            {
                var baseLine = new Line2D(new Point2D(lines[0].P.X, lines[0].P.Y), new Point2D(lines[0].D.X, lines[0].D.Y));
                var targetLine = new Line2D(new Point2D(lines[1].P.X, lines[1].P.Y), new Point2D(lines[1].D.X, lines[1].D.Y));
                var basePoint = baseLine.At(t);
                var targetPoint = targetLine.At(t + t2);
                var guessLine = Line2D.FromPoints(basePoint, targetPoint, t2);

                //assert
                var confirmTime = guessLine.IntersectionTime(targetLine, targetPoint);
                //confirmTime.Should().BeApproximately(t2, 0.001);
                return lines.Skip(2).Sum(line =>
                {
                    var currentLine = new Line2D(new Point2D(line.P.X, line.P.Y), new Point2D(line.D.X, line.D.Y));
                    var intersection = guessLine.IntersectS(currentLine);
                    if (intersection is null) return 10000;
                    var time = currentLine.IntersectionTime(guessLine, intersection.Value)!.Value;
                    var baseToIntersectionTime = guessLine.IntersectionTime(currentLine, intersection.Value)!.Value;
                    var error = Math.Min(
                        Math.Abs((t + baseToIntersectionTime - time)),
                        Math.Abs((t - baseToIntersectionTime - time)));
                    return error;
                });
            };
            /*Console.WriteLine($"4.999: {f(4.9, 1)}");
            Console.WriteLine($"5.000: {f(5.0, 1)}");
            Console.WriteLine($"5.001: {f(5.1, 1)}");*/
            /*for (int i = 0; i < 40; i++)
            {
                var time = 5 - 0.2 + (double)i / 100;
                Console.WriteLine($"{time}: {f(time, -3, 1)}");
            }*/

            for (long i = 0; i <= 5_000_000_000_000; i += 100_000_000_000)
            {
                for (long j = -5_000_000_000_000; j <= 5_000_000_000_000; j += 400_000_000_000)
                {
                    //Console.WriteLine($"{4 + 0.04 * i}, {-3 + 0.04 * j}");
                    //181274863478376
                    var value = Math.Log(f(i, j));
                    if (value < 0.1) Console.BackgroundColor = ConsoleColor.Green;
                    else if (value < 0.2) Console.BackgroundColor = ConsoleColor.DarkGreen;
                    else if (value < 0.5) Console.BackgroundColor = ConsoleColor.Yellow;
                    else if (value < 0.75) Console.BackgroundColor = ConsoleColor.DarkYellow;
                    else if(value < 1) Console.BackgroundColor = ConsoleColor.Red;
                    else if(value < 2) Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write(value.ToString("0.00 "));
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            /*var learnDecay = 0.1d;
            var learnRate = 1d;
            var dp = new double[] { 0.0001, 0.0001 }; //derivative precision
            var guess = (5d, -2d);

            for (int i = 1; i < 100000; i++)
            {
                var currentError = f(guess.Item1, guess.Item2);
                learnRate = Math.Exp(-1+-i/1000d);
                var ft = (f(guess.Item1 + dp[0], guess.Item2) - f(guess.Item1 - dp[0], guess.Item2)) / (2 * dp[0]);
                var ft2 = (f(guess.Item1, guess.Item2 + dp[1]) - f(guess.Item1, guess.Item2 - dp[1])) / (2 * dp[1]);
                guess = (guess.Item1 - learnRate * ft, guess.Item2 - learnRate * ft2);
                //guess = (guess.Item1, guess.Item2, guess.Item3 - learnRate * fdy);
                if (i % 1000 == 0)
                {
                    var q = 1;
                }
            }*/
            var a = 1;
            return 0;
            //return (long)(guess.Item1 + guess.Item2);
        }
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

        public bool Approximate(Point2D other)
        {
            return Math.Abs(other.X - this.X) == 0 && Math.Abs(other.Y - this.Y) == 0;
            //return Math.Abs(other.X - this.X) < 0.00000000001d && Math.Abs(other.Y - this.Y) < 0.00000000001d;
        }

        public static bool operator ==(Point2D left, Point2D right) => left.X == right.X && left.Y == right.Y;
        public static bool operator !=(Point2D left, Point2D right) => !(left == right);
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

        public bool InsideBounds(long min, long max) =>
            min <= X && X <= max && min <= Y && Y <= max;
    }

    private record Line2D(Point2D Point, Point2D Dir)
    {
        public Point2D UDir = Dir / Dir.X;

        public double A => Dir.Y / Dir.X;
        public double B => Point.Y - A * Point.X;

        public static Line2D FromPoints(Point2D from, Point2D to, double time = 1)
        {
            return new Line2D(from, (to - from) / time);
        }

        public Point2D? IntersectS(Line2D line)
        {
            return (A - line.A) != 0
                ? new Point2D((line.B - B) / (A - line.A), A * (line.B - B) / (A - line.A) + B)
                : null;
        }

        public Point2D? Intersect(Line2D line)
        {
            var p1 = Point;
            var p2 = Point + Dir;
            var p3 = line.Point;
            var p4 = line.Point + line.Dir;
            var x1 = (BigInteger)p1.X;
            var y1 = (BigInteger)p1.Y;
            var x2 = (BigInteger)p2.X;
            var y2 = (BigInteger)p2.Y;
            var x3 = (BigInteger)p3.X;
            var y3 = (BigInteger)p3.Y;
            var x4 = (BigInteger)p4.X;
            var y4 = (BigInteger)p4.Y;
            var denom = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (denom == 0) return null;

            var xNumerator = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4));
            var yNumerator = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4));
            return new Point2D((double)xNumerator / (double)denom, (double)yNumerator / (double)denom);
        }

        public Point2D At(double time) => Point + time * Dir;

        public bool SameLine(Line2D line)
        {
            var dir = (line.Point - Point);
            return (dir / dir.X).Approximate(UDir) || (dir / dir.X).Approximate(line.UDir);
        }

        public double? LiesInsideBlock(long min, long max)
        {
            double time = 0;
            if (Dir.X != 0) time = (min - Point.X) / Dir.X;
            if (Dir.Y != 0) time = (min - Point.Y) / Dir.Y;
            return (Point + time * UDir).InsideBounds(min, max)
                ? time
                : null;
        }

        public double? IntersectionTime(Line2D line, Point2D point)
        {
            return !UDir.Approximate(line.UDir)
                ? (point - Point).X / Dir.X
                : null;
        }
    }
}