using FluentAssertions;

namespace AoC_2024;

public sealed class Day03 : Day
{
    [Puzzle(answer: 178886550)]
    public long Part1()
    {
        long result = 0;
        foreach (var line in Input)
        {
            var splitted = line.Split("mul(");
            foreach (var split in splitted)
            {
                var s2 = split.Split([',',')']).ToArray();
                result += int.TryParse(s2[0], out var p1) && int.TryParse(s2[1], out var p2) && p1 < 1000 && p2 < 1000
                    ? p1 * p2
                    : 0;
            }
        }
        return result;
    }

    [Puzzle(answer: 87163705)]
    public long Part2()
    {
        long result = 0;
        bool first = true;
        foreach (var line in Input)
        {
            var doSplitted = line.Split("do()");
            for (int i = 0; i < doSplitted.Length; ++i)
            {
                if (i == 0 && !first) continue;
                
                var dontSplitted = doSplitted[i].Split("don't()");
                var beforeDont = dontSplitted[0];
                var splitted = beforeDont.Split("mul(");
                foreach (var split in splitted)
                {
                    var s2 = split.Split([',',')']).ToArray();
                    result += int.TryParse(s2[0], out var p1) && int.TryParse(s2[1], out var p2) && p1 < 1000 && p2 < 1000
                        ? p1 * p2
                        : 0;
                }
                
                if (i == doSplitted.Length - 1) first = dontSplitted.Length <= 1;
            }
        }
        return result;
    }
};