using FluentAssertions.Equivalency.Steps;
using MathNet.Numerics;
using System.Net.Http.Headers;
using System.Numerics;
using static AoC_2023.Day17;

namespace AoC_2023;

public sealed class Day19 : Day
{
    [Puzzle(answer: null)]
    public long Part1Example() => new Day19Class().Part1(InputExample);

    [Puzzle(answer: null)]
    public long Part1() => new Day19Class().Part1(Input);

    [Puzzle(answer: null)]
    public long Part2Example() => new Day19Class().Part2(InputExample);

    [Puzzle(answer: null)]
    public long Part2() => new Day19Class().Part2(Input);


    private class Day19Class
    {   
        public Day19Class()
        {
        }

        internal long Part1(string[] input)
        {
            long result = 0;
            return result;
        }

        internal long Part2(string[] inputExample)
        {
            long result = 0;
            return result;
        }
    }
}