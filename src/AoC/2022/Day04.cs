namespace AoC._2022;

public class Day04 : Day
{
    [Test]
    public void Example() => InputExample.Select(Parse).Count(OneContained).Should().Be(2);

    [Test]
    public void Part1() => InputPart1.Select(Parse).Count(OneContained).Should().Be(538);

    [Test]
    public void Part2() => InputPart2.Select(Parse).Count(AnyOverlap).Should().Be(792);

    public Interval[] Parse(string line)
    {
        var split = line.Split('-', ',');
        int a1 = int.Parse(split[0]);
        int b1 = int.Parse(split[1]);
        int a2 = int.Parse(split[2]);
        int b2 = int.Parse(split[3]);
        return new[]
        {
            new Interval(a1, b1),
            new Interval(a2, b2)
        };
    }

    public record Interval(int Min, int Max)
    {
        public bool IsValid = Max - Min >= 0;

        public Interval Intersection(Interval other) =>
            new Interval(
                Math.Max(Min, other.Min),
                Math.Min(Max, other.Max));
    };

    public bool OneContained(Interval[] intervals)
    {
        var intersection = intervals[0].Intersection(intervals[1]);
        return intersection == intervals[0] || intersection == intervals[1];
    }

    public bool AnyOverlap(Interval[] intervals) => intervals[0].Intersection(intervals[1]).IsValid;
}