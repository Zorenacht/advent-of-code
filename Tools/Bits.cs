namespace Tools.Geometry;

public static class Bits
{
    public static long Extract(long bits, int start, int length)
    {
        long mask = (1 << length) - 1;
        long translatedMask = mask << start;
        return bits & translatedMask;
    }
}
