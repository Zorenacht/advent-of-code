namespace AoC._2022;

public abstract class Day
{
    protected readonly string[] InputExample = Array.Empty<string>();
    protected readonly string[] InputPart1;
    protected readonly string[] InputPart2 = Array.Empty<string>();

    protected Day()
    {
        var year = GetType().Namespace![^4..];
        var className = GetType().Name;
        Reader.TryReadLines(@$"{year}/Input/{className}-Example.txt", out InputExample);
        Reader.TryReadLines(@$"{year}/Input/{className}-1.txt", out InputPart1);
        InputPart2 = InputPart1;
    }
}