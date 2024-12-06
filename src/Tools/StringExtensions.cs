using System.Text;
using Tools.Geometry;

namespace Tools;

public static class StringExtensions
{
    public static string[] Linebreaks = ["\r\n", "\r", "\n"];
    
    public static Grid<char> ToGrid(this string[] lines) => new(lines.Select(x => x.ToCharArray()).ToArray());

    public static string[] Lines(this string str)
        => str.Split(
            Environment.NewLine,
            StringSplitOptions.None);

    public static string[][] GroupBy(this IEnumerable<string> lines, string separator)
    {
        var groups = new List<List<string>> { new() };
        foreach (var l in lines)
        {
            if (l == separator)
            {
                groups.Add(new List<string>());
                continue;
            }
            groups[^1].Add(l);
        }
        if (groups[^1].Count == 0) groups.RemoveAt(groups.Count - 1);
        return groups.Select(group => group.ToArray()).ToArray();
    }

    public static string[] AddBorder(this string[] lines, char symbol)
    {
        var modified = new List<string>();
        var first = new string(symbol, lines[0].Length + 2);
        var last = new string(symbol, lines[^1].Length + 2);

        modified.Add(first);
        foreach (var str in lines)
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
