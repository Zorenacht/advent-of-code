namespace Tools;

public static class BasicMath
{
    public static int Modulo(this int a, int b)
    {
        int remainder = a % b;
        return remainder >= 0 ? remainder : remainder + b;
    }
}
