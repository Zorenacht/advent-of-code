using FluentAssertions.Equivalency.Steps;
using MathNet.Numerics;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Constraints;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using Tools.Shapes;

namespace AoC_2023;

public sealed class Day22 : Day
{
    [Puzzle(answer: null)]
    public long Part1Example() => new Airship().Part1(InputExample);

    public int Copies = 1 + 0 * 2;

    [Puzzle(answer: null)]
    public long Part1() => new Airship().Part1(Input);

    [Puzzle(answer: null)]
    public long Part2Example() => new Airship().Part2(InputExample);

    [Puzzle(answer: null)]
    public long Part2() => new Airship().Part2(Input);


    private class Airship
    {
        internal long Part1(string[] input)
        {
            return 0;
        }

        internal long Part2(string[] input)
        {
            return 0;
        }

    }
}