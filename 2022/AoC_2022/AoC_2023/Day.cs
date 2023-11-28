using Tools;

namespace AoC_2023;

public abstract class Day
{
    protected readonly string[] InputExample = Array.Empty<string>();
    protected readonly string[] Input = Array.Empty<string>();

    protected Day()
    {
        var className = GetType().Name;
        Reader.TryReadLines(@$"InputFiles\{className}-Example.txt", out InputExample);
        Reader.TryReadLines(@$"InputFiles\{className}.txt", out Input);
    }
}