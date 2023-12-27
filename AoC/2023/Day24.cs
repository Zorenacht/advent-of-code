namespace AoC_2023;

public sealed class Day24 : Day
{
    [Puzzle(answer: 2)]
    public long Part1Example() => new Collision().Part1(InputExample, 7, 27);

    [Puzzle(answer: 14046)]
    public long Part1() => new Collision().Part1(Input, 200_000_000_000_000L, 400_000_000_000_000L);

    [Puzzle(answer: 47)]
    public long Part2Example() => new Collision().Part2(InputExample);

    [Puzzle(answer: 808107741406756)]
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
                    var intersection = lines[i].Intersection(lines[j]);
                    if (intersection is null || intersection.Result != IntersectionResult.Found)
                    {
                        continue; //parallel and not on same line
                    }
                    if (intersection.ToTime < 0 || intersection.FromTime < 0)
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
            var parsed = input.Select<string, ((double X, double Y, double Z) P, (double X, double Y, double Z) D)>(x =>
            {
                var split = x.Split("@,".ToCharArray(), StringSplitOptions.TrimEntries);
                return (
                    (double.Parse(split[0]), double.Parse(split[1]), double.Parse(split[2])),
                    (double.Parse(split[3]), double.Parse(split[4]), double.Parse(split[5]))
                    );
            }).ToList();
            var lines = parsed.Select(x => new Line2D(new Point2D(x.P.X, x.P.Y), new Point2D(x.D.X, x.D.Y))).ToList();

            var dirs = new List<Point2D>();
            int blockRange = 1;
            for (int i = -blockRange; i <= blockRange; i++)
            {
                for (int j = -blockRange; j <= blockRange; j++)
                {
                    dirs.Add(new Point2D(i, j));
                }
            }
            var error = 1000000d;
            var set = new HashSet<Point2D>();
            var pq = new PriorityQueue<Point2D, double>(1_000_000);
            pq.Enqueue(new Point2D(300, -200), ReturnIfIntersectionNotUnique(lines, new Point2D(300, -200), out var inter));
            var results = new PriorityQueue<(Point2D I, Point2D D), double>(1_000_000);
            while (error > 1)
            {
                var dir = pq.Dequeue();
                error = ReturnIfIntersectionNotUnique(lines, dir, out inter);
                var lst = new List<double>();
                for (int i = 0; i < dirs.Count; i++)
                {
                    var d = dir + dirs[i];
                    if (!set.Contains(d))
                    {
                        var err = ReturnIfIntersectionNotUnique(lines, d, out _);
                        pq.Enqueue(d, err);
                        set.Add(d);
                    }
                }
                results.Enqueue((inter, dir), error);
            }
            var result = results.Peek();

            for (int i = -1000; i < 1000; i++)
            {
                var grouped = parsed.Select(parse => parse.P.Z + (parse.D.Z - i) / (parse.D.X - result.D.X) * (result.I.X - parse.P.X) )
                    .GroupBy(z => z)
                    .Select(x => x.Key)
                    .Where(x => x is not double.NegativeInfinity or double.PositiveInfinity)
                    .OrderBy(x => x)
                    .ToList();
                if(Math.Abs(grouped[0] - grouped[^1]) < 5d)
                {
                    return (long)(result.I.X + result.I.Y + grouped[grouped.Count/2]);
                }
            }
            return -1;

            static double ReturnIfIntersectionNotUnique(List<Line2D> lines, Point2D dir, out Point2D inter)
            {
                inter = new();
                var set = new Dictionary<Point2D, int>();
                var times = new List<double>();
                var baseLine = new Line2D(lines[0].Point, lines[0].Dir - dir);
                for (int i = 1; i < lines.Count; i++)
                {
                    var line = new Line2D(lines[i].Point, lines[i].Dir - dir);
                    var intersection = baseLine.Intersection(line);
                    if (intersection is null || intersection.Result != IntersectionResult.Found)
                    {
                        times.Add(int.MaxValue);
                        continue; //parallel and not on same line
                    }
                    if (intersection.ToTime < 0 || intersection.FromTime < 0)
                    {
                        times.Add(int.MaxValue);
                        continue; //in past
                    }
                    if (!set.ContainsKey(intersection!.Value)) set[intersection.Value] = 1;
                    else set[intersection.Value]++;
                    times.Add(intersection.FromTime);
                    //if (set.Count > 1 && !set.First().AboutEquals(intersection.Value)) return false;

                }
                var median = times[times.Count / 2];
                var err = times.Sum(x => Math.Abs(median - x));
                inter = set.First().Key;
                if (err == double.NaN) return int.MaxValue;
                return err;
            }
            //return (long)(guess.Item1 + guess.Item2);
        }
    }

    public struct Point2D
    {
        internal double X;
        internal double Y;

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X},{Y})";

        public bool AboutEquals(Point2D other, double acceptableError = 1e-5) => Math.Abs(other.X - this.X) < acceptableError && Math.Abs(other.Y - this.Y) < acceptableError;

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

    public record Intersection
    {
        public Point2D Value { get; init; }
        public double FromTime { get; init; }
        public double ToTime { get; init; }


        public IntersectionResult Result { get; init; }

    }

    public enum IntersectionResult
    {
        Found,
        Parallel,
    }

    private record Line2D(Point2D Point, Point2D Dir)
    {
        public Point2D UDir => Dir / Dir.X;
        public double A => Type == LineType.Basic ? Dir.Y / Dir.X : 0;
        public double B => Type == LineType.Basic ? Point.Y - A * Point.X : Point.X;


        private LineType Type => Dir.X != 0 ? LineType.Basic : LineType.Vertical;

        private enum LineType
        {
            Basic,
            Vertical,
            Point
        }

        public Intersection Intersection(Line2D line)
        {
            if (Type == LineType.Vertical && line.Type == LineType.Vertical)
                return new Intersection() { Result = IntersectionResult.Parallel };
            if (Type == LineType.Vertical && line.Type == LineType.Basic)
                return VerticalBasicIntersection(this, line);
            if (Type == LineType.Basic && line.Type == LineType.Vertical)
            {
                var intersection = VerticalBasicIntersection(line, this);
                return intersection with { ToTime = intersection.FromTime, FromTime = intersection.ToTime };
            }
            if (Type == LineType.Basic && line.Type == LineType.Basic)
                return BasicBasicIntersection(this, line);
            throw new Exception();
        }

        private static Intersection VerticalBasicIntersection(Line2D vertical, Line2D basic)
        {
            var toTime = (vertical.Point.X - basic.Point.X) / basic.Dir.X;
            var intersection = basic.Point + toTime * basic.Dir;
            var fromTime = (intersection.Y - vertical.Point.Y) / vertical.Dir.Y;
            return new Intersection() { Result = IntersectionResult.Found, Value = intersection, FromTime = fromTime, ToTime = toTime };
        }

        private static Intersection BasicBasicIntersection(Line2D line1, Line2D line2)
        {
            if (line1.A == line2.A)
                return new Intersection() { Result = IntersectionResult.Parallel };
            var intersection = new Point2D(
                (line2.B - line1.B) / (line1.A - line2.A),
                line1.A * (line2.B - line1.B) / (line1.A - line2.A) + line1.B);
            var fromTime = (intersection - line1.Point).X / line1.Dir.X;
            var toTime = (intersection - line2.Point).X / line2.Dir.X;
            return new Intersection() { Result = IntersectionResult.Found, Value = intersection, FromTime = fromTime, ToTime = toTime };
        }

        public Point2D? IntersectS(Line2D line)
        {
            return (A - line.A) != 0
                ? new Point2D((line.B - B) / (A - line.A), A * (line.B - B) / (A - line.A) + B)
                : null;
        }

        public static Line2D FromPoints(Point2D from, Point2D to, double time = 1d)
        {
            return new Line2D(from, (to - from) / time);
        }

        public Point2D At(double time) => Point + time * Dir;

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
            return !UDir.AboutEquals(line.UDir)
                ? (point - Point).X / Dir.X
                : null;
        }

        /*        public Point2D? Intersect(Line2D line)
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
        }*/
    }

    /*var f = (double t, double t2) =>
    {
        var baseLine = new Line2D(new Point2D(lines[0].P.X, lines[0].P.Y), new Point2D(lines[0].D.X, lines[0].D.Y));
        var targetLine = new Line2D(new Point2D(lines[1].P.X, lines[1].P.Y), new Point2D(lines[1].D.X, lines[1].D.Y));
        var basePoint = baseLine.At(t);
        var targetPoint = targetLine.At(t + t2);
        var guessLine = Line2D.FromPoints(basePoint, targetPoint, t2);

        //assert
        var confirmTime = guessLine.IntersectionTime(targetLine, targetPoint);
        //confirmTime.Should().BeApproximately(t2, 0.001);
        return lines.Skip(2).Take(1).Sum(line =>
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
    };*/
    /*Console.WriteLine($"4.999: {f(4.9, 1)}");
    Console.WriteLine($"5.000: {f(5.0, 1)}");
    Console.WriteLine($"5.001: {f(5.1, 1)}");*/
    /*for (int i = 0; i < 40; i++)
    {
        var time = 5 - 0.2 + (double)i / 100;
        Console.WriteLine($"{time}: {f(time, -3, 1)}");
    }*/

    /*for (long i = 0; i <= 5_000_000_000_000; i += 100_000_000_000)
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
            else if (value < 1) Console.BackgroundColor = ConsoleColor.Red;
            else if (value < 2) Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write(value.ToString("0.00 "));
            Console.ResetColor();
        }
        Console.WriteLine();
    }
    Console.WriteLine();*/

    /*var learnDecay = 0.1d;
    var learnRate = 1d;
    var dp = new double[] { 0.0001, 0.0001 }; //derivative precision
    var guess = (5d, -1d);

    for (int i = 1; i < 100000; i++)
    {
        var currentError = f(guess.Item1, guess.Item2);
        learnRate = Math.Exp(-2 + -i / 1000d);
        var ft = (f(guess.Item1 + dp[0], guess.Item2) - f(guess.Item1 - dp[0], guess.Item2)) / (2 * dp[0]);
        var ft2 = (f(guess.Item1, guess.Item2 + dp[1]) - f(guess.Item1, guess.Item2 - dp[1])) / (2 * dp[1]);
        guess = (guess.Item1 - learnRate * ft, guess.Item2 - learnRate * ft2);
        //guess = (guess.Item1, guess.Item2, guess.Item3 - learnRate * fdy);
        if (i % 1000 == 0)
        {
            var q = 1;
        }
    }
    var a = 1;*/
}