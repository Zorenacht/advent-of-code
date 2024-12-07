namespace AoC_2024;

public sealed class Day03 : Day
{
    [Puzzle(answer: 178886550)]
    public long Part1()
    {
        long result = 0;
        var splitted = InputAsText.Split("mul(");
        foreach (var split in splitted)
        {
            var s2 = split.Split([',', ')']).ToArray();
            result += int.TryParse(s2[0], out var p1) && int.TryParse(s2[1], out var p2) && p1 < 1000 && p2 < 1000
                ? p1 * p2
                : 0;
        }
        return result;
    }

    [Puzzle(answer: 87163705)]
    public long Part2()
    {
        long result = 0;
        var doSplitted = InputAsText.Split("do()");
        foreach (string doSplit in doSplitted)
        {
            var dontSplitted = doSplit.Split("don't()");
            var beforeDont = dontSplitted[0];
            var splitted = beforeDont.Split("mul(");
            foreach (var split in splitted)
            {
                var s2 = split.Split([',', ')']).ToArray();
                result += int.TryParse(s2[0], out var p1) && int.TryParse(s2[1], out var p2) && p1 < 1000 && p2 < 1000
                    ? p1 * p2
                    : 0;
            }
        }
        return result;
    }
};