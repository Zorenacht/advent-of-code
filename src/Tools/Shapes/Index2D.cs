using System.Diagnostics.CodeAnalysis;

namespace Tools.Shapes;

public readonly struct Index2D : IEquatable<Index2D>
{
    public static readonly Index2D O = new(0, 0);

    public int Row { get; }
    public int Col { get; }

    public Index2D(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public override string ToString()
    {
        return $"({Row},{Col})";
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Index2D other && Equals(other);
    public bool Equals(Index2D other) => other.Row == Row && other.Col == Col;
    public override int GetHashCode() => Row ^ (Col << 16);

    public static bool operator ==(Index2D left, Index2D right) => left.Equals(right);
    public static bool operator !=(Index2D left, Index2D right) => !(left == right);

    public static Index2D operator -(Index2D left, Index2D right) => new(left.Row - right.Row, left.Col - right.Col);
    public static Index2D operator +(Index2D left, Index2D right) => new(left.Row + right.Row, left.Col + right.Col);
    public static Index2D operator *(int left, Index2D right) => new(left * right.Row, left * right.Col);

    public Index2D Difference(Index2D other) => new Index2D(Row - other.Row, Col - other.Col);
}
