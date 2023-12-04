using System.Text;

namespace Tools;

public static class StringExtensions
{
    public static string[] Lines(this string str)
        => str.Split(
            Environment.NewLine,
            StringSplitOptions.None);

    public static string[] AddBorder(this string[] strings, char symbol)
    {
        var modified = new List<string>();
        var first = new string(symbol, strings[0].Length + 2);
        var last = new string(symbol, strings[^1].Length + 2);

        modified.Add(first);
        foreach (var str in strings)
        {
            var sb = new StringBuilder(str);
            sb.Insert(0, symbol);
            sb.Append(symbol);
            modified.Add(sb.ToString());
        }
        modified.Add(last);
        return [.. modified];
    }
}
