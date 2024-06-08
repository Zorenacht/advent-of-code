using System.Text.RegularExpressions;

namespace AoC_2020;

public sealed class Day02 : Day
{
    [Puzzle(460)]
    public int Part1()
    {
        int result = Input
            .Select(x =>
            {
                var regex = new Regex(@"(\d+)-(\d+) (.): (.*)");
                var match = regex.Match(x);
                return (
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    match.Groups[3].Value[0],
                    match.Groups[4].Value);
            })
            .Where(x =>
            {
                var count = x.Item4.Where(ch => ch == x.Item3).Count();
                return count >= x.Item1 && count <= x.Item2;
            })
            .Count();
        return result;
    }

    [Puzzle(251)]
    public int Part2()
    {
        int result = Input
            .Select(x =>
            {
                var regex = new Regex(@"(\d+)-(\d+) (.): (.*)");
                var match = regex.Match(x);
                return (
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    match.Groups[3].Value[0],
                    match.Groups[4].Value);
            })
            .Where(x =>
            {
                return x.Item4[x.Item1 - 1] == x.Item3 ^ x.Item4[x.Item2 - 1] == x.Item3;
            })
            .Count();
        return result;
    }
}