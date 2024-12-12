namespace Tools.Geometry;

public readonly struct DoubleIndex2D(double row, double col)
{
    public double Row { get; } = row;
    public double Col { get; } = col;
    
    public static readonly DoubleIndex2D N = new(-0.5, +0.0);
    public static readonly DoubleIndex2D S = new(+0.5, +0.0);
    public static readonly DoubleIndex2D E = new(+0.0, +0.5);
    public static readonly DoubleIndex2D W = new(+0.0, -0.5);
    public static readonly DoubleIndex2D NE = N + E;
    public static readonly DoubleIndex2D NW = N + W;
    public static readonly DoubleIndex2D SW = S + W;
    public static readonly DoubleIndex2D SE = S + E;
    
    public static DoubleIndex2D operator +(DoubleIndex2D left, DoubleIndex2D right) => new(left.Row + right.Row, left.Col + right.Col);
    
    public bool Equals(DoubleIndex2D other) => Math.Abs(other.Row - Row) < 0.00001 && Math.Abs(other.Col - Col) < 0.00001;
    public static bool operator ==(DoubleIndex2D left, DoubleIndex2D right) => left.Equals(right);
    public static bool operator !=(DoubleIndex2D left, DoubleIndex2D right) => !(left == right);
    
    public static readonly DoubleIndex2D[] Cardinals = [E, N, W, S];
};