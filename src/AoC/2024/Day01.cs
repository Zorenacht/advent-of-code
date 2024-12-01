using FluentAssertions;

namespace AoC_2024;

public sealed class Day01 : Day
{
    [Puzzle(answer: null)]
    public int Part1()
    {
        int result = 0;
        var lines = Input;
        foreach (var line in lines)
        {
            var splitted = line.Split(',');
            result += int.Parse(splitted[0]);
        }
        return result;
    }

    [Puzzle(answer: null)]
    public int Part2()
    {
        return 0;
    }
};