using FluentAssertions;

namespace AoC._YearPlaceholder;

public sealed class DayDayPlaceholder : Day
{
    [Puzzle(answer: null)]
    public long Part1()
    {
        long result = 0;
        var lines = Input;
        foreach (var line in lines)
        {
            var splitted = line.Split(',');
            result += int.Parse(splitted[0]);
        }
        return result;
    }

    [Puzzle(answer: null)]
    public long Part2()
    {
        return 0;
    }
};