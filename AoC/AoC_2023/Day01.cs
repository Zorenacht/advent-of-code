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
            var first = line.First(x => x >= '0' && x <= '9');
            var last = line.Last(x => x >= '0' && x <= '9');
            result += (first - '0') * 10 + (last - '0');
        }
        result.Should().Be(54697);
    }

    public string[] Digits = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];

    [Test]
    public void Part2()
    {
        int result = 0;
        foreach (var line in Input)
        {
            int first = 0;
            int last = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] >= '0' && line[i] <= '9')
                {
                    first = line[i] - '0';
                    break;
                }
                for (int j = 0; j < Digits.Length; j++)
                {
                    if (line[0..(i + 1)].Contains(Digits[j]))
                    {
                        first = j + 1;
                        i = line.Length;
                        break;
                    }
                }
            }
            for (int i = line.Length - 1; i >= 0; i--)
            {
                if (line[i] >= '0' && line[i] <= '9')
                {
                    last = line[i] - '0';
                    break;
                }
                for (int j = 0; j < Digits.Length; j++)
                {
                    if (line[i..].Contains(Digits[j]))
                    {
                        last = j + 1;
                        i = 0;
                        break;
                    }
                }
            }
            var calibration = first * 10 + last;
            result += calibration;
        }
        result.Should().Be(54885);
    }
}