using Tools;

namespace AoC_2023;

public abstract class Day
{
    protected readonly string[] InputExample;
    protected readonly string[] Input;

    protected Day()
    {
        var className = GetType().Name;
        Reader.TryReadLines(@$"{className}-Example.txt", out InputExample);
        Reader.TryReadLines(@$"{className}.txt", out Input);
    }
}