using System.Data;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Xml;
using Tools.Geometry;

namespace AoC_2023;

public sealed class Day25 : Day
{
    [Puzzle(answer: 54)]
    public long Part1Example() => new Snowverload().Part1(InputExample);

    [Puzzle(answer: null)]
    public long Part1() => new Snowverload().Part1(Input);

    private class Snowverload
    {
        internal long Part1(string[] input)
        {
            var set = new DataSet();
            return 0;
        }
    }
}