using FluentAssertions;
using MathNet.Numerics.Providers.LinearAlgebra;
using System.Linq;
using System.Text;

namespace AoC_2023;

public sealed class Day01 : Day
{
    [Test]
    public void Part1()
    {
        int result = 0;
        foreach (var line in Input)
        {
            var digits = line
                .Where(Digits.Contains)
                .Select(ch => Digits.IndexOf(ch))
                .ToArray();
            result += digits[0] * 10 + digits[^1];
        }
        result.Should().Be(54697);
    }

    public string Digits = "0123456789";
    public List<string> DigitWords = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];

    [Test]
    public void Part2()
    {
        int result = 0;
        foreach (var line in Input)
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
        result.Should().Be(54885);
    }
}