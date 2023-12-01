namespace Tools;

public static class StringExtensions
{
    public static string[] Lines(this string str) 
        => str.Split(
            Environment.NewLine,
            StringSplitOptions.None);
}
