namespace AoC;

public abstract class Day
{
    protected readonly string[] InputExample;
    protected readonly string[] Input;
    protected readonly string InputExampleAsText;
    protected readonly string InputAsText;

    protected Day()
    {
        var year = GetType().Namespace![^4..];
        var className = GetType().Name;
        Reader.TryReadLines(@$"{year}\Input\{className}-Example.txt", out InputExample);
        Reader.TryReadLines(@$"{year}\Input\{className}.txt", out Input);
        InputAsText = Reader.ReadAsText(@$"{year}\Input\{className}-Example.txt");
        InputAsText = Reader.ReadAsText(@$"{year}\Input\{className}.txt");
    }
}