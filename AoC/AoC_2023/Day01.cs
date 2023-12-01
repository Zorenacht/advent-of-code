using FluentAssertions;

namespace AoC_2023;

public sealed class Day01 : Day
{
    [Puzzle(54697)]
    public int Part1(string input)
    {
        int result = 0;
        foreach (var line in input.Lines())
        {
            var digits = line
                .Where(Digits.Contains)
                .Select(ch => Digits.IndexOf(ch))
                .ToArray();
            result += digits[0] * 10 + digits[^1];
        }
        return result;
    }

    public string Digits = "0123456789";
    public List<string> DigitWords = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];

    [Puzzle(54885)]
    public int Part2(string input)
    {
        int result = 0;
        foreach (var line in input.Lines())
        {
            var digits = new List<int>();
            for (int i = 0; i < line.Length; i++)
            {
                if (Digits.Contains(line[i])) digits.Add(line[i] - '0');
                else
                {
                    var index = DigitWords.FindIndex(line[0..(i + 1)].EndsWith);
                    if (index != -1) digits.Add(index + 1);
                }
            }
            result += digits[0] * 10 + digits[^1];
        }
        return result;
    }
}