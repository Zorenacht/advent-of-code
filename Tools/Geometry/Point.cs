using System.Diagnostics.CodeAnalysis;

namespace Tools.Geometry;

public readonly struct Point : IEquatable<Point>
{
    public static readonly Point O = new(0, 0);

    public int X { get; }
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }


    //Neighbor visually correct
    public Point NeighborV(Direction direction)
        => direction switch
        {
            Direction.N => new Point(X, Y - 1),
            Direction.S => new Point(X, Y + 1),
            Direction.W => new Point(X - 1, Y),
            Direction.E => new Point(X + 1, Y),
            Direction.NW => new Point(X - 1, Y - 1),
            Direction.SW => new Point(X - 1, Y + 1),
            Direction.SE => new Point(X + 1, Y + 1),
            Direction.NE => new Point(X + 1, Y - 1),
            _ => throw new NotSupportedException()
        };

    public Point Neighbor(Direction direction)
    {
        int x = Convert.ToInt32(Math.Round(Math.Cos(Convert.ToDouble(direction) * Math.PI / 4)));
        int y = Convert.ToInt32(Math.Round(Math.Sin(Convert.ToDouble(direction) * Math.PI / 4)));
        return new Point(X + x, Y + y);
    }

    public Point Neighbor2(Direction direction)
    {
        int x = Convert.ToInt32(Math.Round(-Math.Sin(Convert.ToDouble(direction) * Math.PI / 4)));
        int y = Convert.ToInt32(Math.Round(Math.Cos(Convert.ToDouble(direction) * Math.PI / 4)));
        return new Point(X + x, Y + y);
    }


    public int Norm => X * X + Y * Y;

    public Direction GeneralDirection
    {
        get
        {
            if (X == 0 && Y > 0) return Direction.N;
            if (X < 0 && Y > 0) return Direction.NW;
            if (X < 0 && Y == 0) return Direction.W;
            if (X < 0 && Y < 0) return Direction.SW;
            if (X == 0 && Y < 0) return Direction.S;
            if (X > 0 && Y < 0) return Direction.SE;
            if (X > 0 && Y == 0) return Direction.E;
            if (X > 0 && Y > 0) return Direction.NE;
            return Direction.None;
        }
    }


    public override string ToString()
    {
        return $"({X},{Y})";
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Point other && Equals(other);
    public bool Equals(Point other) => other.X == X && other.Y == Y;
    public override int GetHashCode() => X ^ (Y << 16);

    public static bool operator ==(Point left, Point right) => left.Equals(right);
    public static bool operator !=(Point left, Point right) => !(left == right);

    public static Point operator -(Point left, Point right) => new(left.X - right.X, left.Y - right.Y);
    public static Point operator +(Point left, Point right) => new(left.X + right.X, left.Y + right.Y);
    public static Point operator *(int left, Point right) => new(left * right.X, left * right.Y);

    public Point Difference(Point other) => new Point(X - other.X, Y - other.Y);
}
