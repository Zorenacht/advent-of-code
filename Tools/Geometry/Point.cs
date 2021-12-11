namespace Tools.Geometry;

public struct Point
{
    public int X { get; init; }
    public int Y { get; init; }

    public Point(int x, int y)
    {
        X = x; 
        Y = y; 
    }

    public Point Neighbor(Direction direction) 
    { 
        int x = Convert.ToInt32(-Math.Round(Math.Sin(Convert.ToDouble(direction) * Math.PI / 4)));
        int y = Convert.ToInt32(Math.Round(Math.Cos(Convert.ToDouble(direction) * Math.PI / 4)));
        return new Point(X+x, Y+y);
    }

    public override string ToString()
    {
        return $"({X},{Y})";
    }
}
