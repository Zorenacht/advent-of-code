using System.Diagnostics.CodeAnalysis;

namespace Tools.Shapes;

public enum LineType
{
    Horizontal = 0,
    Vertical = 1
}

public record StraightLine : Line
{
    private LineType Type { get; }
    private int ConstantValue 
        => Type == LineType.Horizontal
            ? Start.Row 
            : Start.Col;

    public StraightLine(Point start, Point end) : base(start, end)
    {
        if (Start.Col != End.Col && Start.Row != End.Row) throw new NotSupportedException("Line must be straight.");
        Type = Start.Col == End.Col
            ? LineType.Vertical 
            : LineType.Horizontal;
    }

    public bool Contains(Point point)
    {
        return Type == LineType.Horizontal
            ? point.Row == ConstantValue && (Start.Col <= point.Col && point.Col <= End.Col || End.Col <= point.Col && point.Col <= Start.Col)
            : point.Col == ConstantValue && (Start.Row <= point.Row && point.Row <= End.Row || End.Row <= point.Row && point.Row <= Start.Row);
    }

    public Point? Intersection(StraightLine line)
    {
        if(Type == LineType.Horizontal && line.Type == LineType.Vertical)
        {
            var point = new Point(ConstantValue, line.ConstantValue);
            return Contains(point) && line.Contains(point) ? point : null;
        }
        if (Type == LineType.Vertical && line.Type == LineType.Horizontal)
        {
            var point = new Point(line.ConstantValue, ConstantValue);
            return Contains(point) && line.Contains(point) ? point : null;
        }
        return null;
    }
}

public record Line : IEquatable<Line>
{
    public Point Start { get; }
    public Point End { get; }

    public Line(Point start, Point end)
    {
        Start = start;
        End = end;
    }

    public override string ToString()
    {
        return $"{Start}->{End}";
    }

    public virtual bool Equals(Line? other) => Start == other?.Start && End == other?.End;
    public override int GetHashCode() => Start.GetHashCode() ^ End.GetHashCode();

    /*public override bool Equals([NotNullWhen(true)] object? obj) => obj is Point other && Equals(other);
    public bool Equals(Line other) => other.Row == Row && other.Col == Col;
    public override int GetHashCode() => Row ^ (Col << 16);*/

    /*public static bool operator ==(Point left, Point right) => left.Equals(right);
    public static bool operator !=(Point left, Point right) => !(left == right);

    public static Point operator -(Point left, Point right) => new(left.Row - right.Row, left.Col - right.Col);
    public static Point operator +(Point left, Point right) => new(left.Row + right.Row, left.Col + right.Col);
    public static Point operator *(int left, Point right) => new(left * right.Row, left * right.Col);

    public Point Difference(Point other) => new Point(Row - other.Row, Col - other.Col);*/
}
