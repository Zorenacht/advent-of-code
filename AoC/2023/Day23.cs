using FluentAssertions.Equivalency.Steps;
using MathNet.Numerics;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Constraints;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using Tools.Shapes;

namespace AoC_2023;

public sealed class Day23 : Day
{
    [Puzzle(answer: null)]
    public long Part1Example() => new AClass().Part1(InputExample);

    [Puzzle(answer: null)]
    public long Part1() => new AClass().Part1(Input);

    [Puzzle(answer: null)]
    public long Part2Example() => new AClass().Part1(InputExample);

    [Puzzle(answer: null)]
    public long Part2() => new AClass().Part1(Input);


    private class AClass
    {
        internal long Part1(string[] input)
        {
            var parsed = input.Select(x =>
            {
                return x;
            });
            return 0;
        }

        internal long Part2(string[] input)
        {
            var parsed = input.Select(x =>
            {
                return x;
            });
            return 0;
        }
    }
}