namespace Tools.Geometry;

public static class Directions
{
    public  static readonly Direction[] All = [
        Direction.E,
        Direction.NE,
        Direction.N,
        Direction.NW,
        Direction.W,
        Direction.SW,
        Direction.S,
        Direction.SE,
    ];

    public static readonly Direction[] Cardinal = [
        Direction.E,
        Direction.N,
        Direction.W,
        Direction.S,
    ];

    public static readonly Direction[] Ordinal = [
        Direction.NE,
        Direction.NW,
        Direction.SW,
        Direction.SE,
    ];

    public static Direction Left(this Direction dir)
        => (Direction)(((int)dir + 2) % 8);

    public static Direction Right(this Direction dir)
        => (Direction)(((int)dir + 6) % 8);

    public static Direction Backwards(this Direction dir)
        => (Direction)(((int)dir + 4) % 8);

    public static bool RightAngle(Direction dir1, Direction dir2)
    {
        return BasicMath.Modulo((int)dir1 - (int)dir2, 2) == 0
            && BasicMath.Modulo((int)dir1 - (int)dir2, 4) != 0;
    }

    public static bool OppositeAngle(Direction dir1, Direction dir2)
    {
        return dir1 != dir2 && BasicMath.Modulo((int)dir1 - (int)dir2, 4) == 0;
    }
}
