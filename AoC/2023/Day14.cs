using AoC;
using MathNet.Numerics;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using Tools.Geometry;
using Tools.Shapes;

namespace AoC_2023;

public sealed class Day14 : Day
{
    [Puzzle(answer: 136)]
    public int Part1Example() => NorthLoad(MoveAllNorth(InputExample));
    [Puzzle(answer: 111979)]
    public int Part1() => NorthLoad(MoveAllNorth(Input));

    [Puzzle(answer: 64)]
    public long Part2Example() => P2(InputExample);
    [Puzzle(answer: 102055)]
    public long Part2() => P2(Input);

    public long P2(string[] input)
    {
        int result = 0;
        var copy = input.Select(x => Enumerable.Repeat('.', x.Length).ToArray()).ToArray();
        var cache = new List<string[]>();
        int iteration = 0;
        while(true)
        {
            for (int turn = 0; turn < 4; turn++)
            {
                input = RotateClockWise(MoveAllNorth(input));
            }
            if (cache.Any(cached => Equal(cached, input)))
            {
                var cycleStart = cache.Select((cached, index) => (cached, index)).First(x => Equal(x.Item1, input)).Item2;
                var cycleLength = (iteration - cycleStart);
                var index = cycleStart + (1000000000L - cycleStart) % cycleLength;
                var target = cache[(int)index - 1];
                return NorthLoad(target);
            }
            cache.Add(input);
            iteration++;
        }
        return result;
    }

    public string[] MoveAllNorth(string[] input)
    {
        var copy = input.Select(x => Enumerable.Repeat('.', x.Length).ToArray()).ToArray();
        for (int j = 0; j < input[0].Length; j++)
        {
            int wall = -1;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i][j] == '#')
                {
                    copy[i][j] = '#';
                    wall = i;
                }
                if (input[i][j] == 'O' && wall + 1 < input.Length)
                {
                    copy[++wall][j] = 'O';
                }
            }
        }
        return copy.Select(x => new string(x)).ToArray();
    }

    public string[] RotateClockWise(string[] input)
    {
        var diagional = Enumerable.Repeat(0, input[0].Length).Select(x => new char[input.Length]).ToArray();
        var vertical = Enumerable.Repeat(0, input[0].Length).Select(x => new char[input.Length]).ToArray();

        for (int i = 0; i < diagional.Length; i++)
        {
            for (int j = 0; j < diagional[0].Length; j++)
            {
                diagional[i][j] = input[j][i];
            }
        }
        for (int i = 0; i < vertical.Length; i++)
        {
            for (int j = 0; j < vertical[0].Length; j++)
            {
                vertical[i][j] = diagional[i][vertical[0].Length - j - 1];
            }
        }
        return vertical.Select(x => new string(x)).ToArray();
    }

    private bool Equal(string[] arr1, string[] arr2)
    {
        for (int i = 0; i < arr1.Length; i++)
        {
            if (arr1[i] != arr2[i]) return false;
        }
        return true;
    }

    private int NorthLoad(string[] arr)
    {
        int count = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            for (int j = 0; j < arr[0].Length; j++)
            {
                if (arr[i][j] == 'O') count += arr.Length - i;
            }
        }
        return count;
    }
}