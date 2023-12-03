using System.Diagnostics.CodeAnalysis;

namespace Tools.Shapes;

public readonly struct Point : IEquatable<Point>
{
    public static readonly Point O = new(0, 0);

    public int Row { get; }
    public int Col { get; }

    public Point(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public override string ToString()
    {
        return $"({Row},{Col})";
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Point other && Equals(other);
    public bool Equals(Point other) => other.Row == Row && other.Col == Col;
    public override int GetHashCode() => Row ^ (Col << 16);

    public static bool operator ==(Point left, Point right) => left.Equals(right);
    public static bool operator !=(Point left, Point right) => !(left == right);

    public static Point operator -(Point left, Point right) => new(left.Row - right.Row, left.Col - right.Col);
    public static Point operator +(Point left, Point right) => new(left.Row + right.Row, left.Col + right.Col);
    public static Point operator *(int left, Point right) => new(left * right.Row, left * right.Col);

    public Point Difference(Point other) => new Point(Row - other.Row, Col - other.Col);
}
