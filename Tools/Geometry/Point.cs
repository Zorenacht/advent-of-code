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


    public Point Neighbor(Direction direction)
    {
        int x = Convert.ToInt32(Math.Round(Math.Cos(Convert.ToDouble(direction) * Math.PI / 4)));
        int y = Convert.ToInt32(Math.Round(Math.Sin(Convert.ToDouble(direction) * Math.PI / 4)));
        return new Point(X + x, Y + y);
    }

    public Point Difference(Point other)
    {
        return new Point(X - other.X, Y - other.Y);
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

    public static bool operator ==(Point left, Point right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Point left, Point right)
    {
        return !(left == right);
    }
}
