namespace AoC_2022;

public abstract class Day
{
    protected readonly string[] InputExample = Array.Empty<string>();
    protected readonly string[] InputPart1;
    protected readonly string[] InputPart2 = Array.Empty<string>();

    protected Day()
    {
        var className = GetType().Name;
        Reader.TryReadLines(@$"InputFiles\{className}-Example.txt", out InputExample);
        Reader.TryReadLines(@$"InputFiles\{className}-1.txt", out InputPart1);
        if (!Reader.TryReadLines(@$"InputFiles\{className}-2.txt", out InputPart2) || InputPart2.Length == 0)
        {
            InputPart2 = InputPart1;
        };
    }
}