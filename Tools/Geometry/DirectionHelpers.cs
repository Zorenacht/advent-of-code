namespace Tools.Geometry;

public static class DirHelpers
{
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
