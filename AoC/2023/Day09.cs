using MathNet.Numerics.Distributions;
using System.Numerics;

namespace AoC_2023;

public sealed class Day09 : Day
{
    [Puzzle(answer: null)]
    public int Part1Example() => P1(InputExample);

    [Puzzle(answer: null)]
    public int Part1() => P1(Input);

    public int P1(string[] input)
    {
        int result = 0;
        var parse = input.Select(x =>
        {
            var split1 = x.Split("", StringSplitOptions.None);
            var split2 = x.Split("", StringSplitOptions.None);
            var split3 = x.Split("", StringSplitOptions.None);
            var kv = new KeyValuePair<string, string>(split1[0], split2[0]);
            return x;
        });
        var board = input.AddBorder('*');
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[0].Length; j++)
            {

            }
        }
        return result;
    }

    [Puzzle(answer: null)]
    public int Part2Example() => P1(InputExample);

    [Puzzle(answer: null)]
    public int Part2() => P1(Input);

    [Puzzle(answer: 15726453850399)]
    public int Part2(string[] input)
    {
        int result = 0;
        var parse = input.Select(x =>
        {
            var split1 = x.Split("", StringSplitOptions.None);
            var split2 = x.Split("", StringSplitOptions.None);
            var split3 = x.Split("", StringSplitOptions.None);
            var kv = new KeyValuePair<string, string>(split1[0], split2[0]);
            return x;
        });
        return result;
    }

}