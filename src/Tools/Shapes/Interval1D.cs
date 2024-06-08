using System.Collections;

namespace Tools.Shapes;

public record Interval1D(long Start, long End) : IEnumerable<long>
{
    public bool Overlap(Interval1D other)
    {
        return Math.Max(Start, other.Start) <= Math.Min(End, other.End);
    }

    public override string ToString() => $"({Start},{End})";

    public long Length => End - Start + 1;


    public IEnumerable<Interval1D> Remove(Interval1D other)
    {
        if (Start < other.Start) yield return new Interval1D(Start, Math.Min(other.Start - 1, End));
        if (other.End < End) yield return new Interval1D(Math.Max(other.End + 1, Start), End);
    }

    public Interval1D Intersection(Interval1D other)
    {
        return Overlap(other)
            ? new Interval1D(Math.Max(Start, other.Start), Math.Min(End, other.End))
            : throw new Exception();
    }

    public IEnumerator<long> GetEnumerator()
    {
        for (long start = Start; start <= End; start++)
        {
            yield return start;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static Interval1D operator +(Interval1D left, long right) => new(left.Start + right, left.End + right);
    public static Interval1D operator +(long left, Interval1D right) => right + left;
    public static Interval1D operator -(Interval1D left, long right) => left + (-right);
    public static Interval1D operator -(long left, Interval1D right) => (-left) + right;
}