namespace Tools.Shapes;

public record Interval(long Start, long End)
{
    public bool Overlap(Interval other)
    {
        return Math.Max(Start, other.Start) <= Math.Min(End, other.End);
    }

    public override string ToString() => $"({Start},{End})";

    public IEnumerable<Interval> Remove(Interval other)
    {
        if (Start < other.Start) yield return new Interval(Start, Math.Min(other.Start - 1, End));
        if (other.End < End) yield return new Interval(Math.Max(other.End + 1, Start), End);
    }

    public Interval Intersection(Interval other)
    {
        return Overlap(other)
            ? new Interval(Math.Max(Start, other.Start), Math.Min(End, other.End))
            : throw new Exception();
    }

    public static Interval operator +(Interval left, long right) => new(left.Start + right, left.End + right);
    public static Interval operator +(long left, Interval right) => right + left;
    public static Interval operator -(Interval left, long right) => left + (-right);
    public static Interval operator -(long left, Interval right) => (-left) + right;
}