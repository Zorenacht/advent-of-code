using Tools;

namespace AoC_2023;

public abstract class Day
{
    protected readonly string[] InputExample;
    protected readonly string[] Input;

    protected Day()
    {
        var className = GetType().Name;
        Reader.TryReadLines(@$"Input\{className}-Example.txt", out InputExample);
        Reader.TryReadLines(@$"Input\{className}.txt", out Input);
    }
}