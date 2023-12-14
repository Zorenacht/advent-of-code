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
    public int Part1Example() => P1(InputExample);

    [Puzzle(answer: 111979)]
    public int Part1() => P1(Input);
    public int P1(string[] input)
    {
        int result = 0;
        int count = 0;
        for (int j = 0; j < input[0].Length; j++)
        {
            int lastTag = -1;
            int os = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i][j] == 'O') count += input.Length - (lastTag + ++os);
                if (input[i][j] == '#')
                {
                    lastTag = i;
                    os = 0;
                }
            }
        }
        return count;
    }

    [Puzzle(answer: 64)]
    public long Part2Example() => P2(InputExample);

    [Puzzle(answer: 102055)]
    public long Part2() => P2(Input);

    private bool Equal(string[] arr1, string[] arr2)
    {
        for (int i = 0; i < arr1.Length; i++)
        {
            for (int j = 0; j < arr1[0].Length; j++)
            {
                if (arr1[i][j] != arr2[i][j]) return false;
            }
        }
        return true;
    }

    private int Load(string[] arr)
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

    public long P2(string[] input)
    {
        int result = 0;
        var copy = input.Select(x => Enumerable.Repeat('.', x.Length).ToArray()).ToArray();
        Console.WriteLine(string.Join("\n", input.Select(x => new string(x.ToArray()))));
        Console.WriteLine();
        var cache = new List<(string[], long)>();
        for (long count = 0; count <= 100000; count++)
        {
            for (int turn = 0; turn < 4; turn++)
            {
                for (int j = 0; j < input[0].Length; j++)
                {
                    int lastTag = -1;
                    int os = 0;
                    for (int i = 0; i < input.Length; i++)
                    {
                        if (input[i][j] == '#')
                        {
                            copy[i][j] = '#';
                            lastTag = i;
                            os = 0;
                        }
                        if (input[i][j] == 'O' && lastTag + os + 1 < input.Length)
                        {
                            copy[lastTag + ++os][j] = 'O';
                        }
                    }
                }
                input = RotateClockWise(copy.Select(x => new string(x)).ToArray());
                copy = input.Select(x => Enumerable.Repeat('.', x.Length).ToArray()).ToArray();
            }
            if (cache.Any(x => Equal(x.Item1, input)))
            {
                cache.Add((input, count));
                var test = cache.Select(x => Load(x.Item1)).ToList();
                var ele = cache.First(x => Equal(x.Item1, input));
                var index = ele.Item2 + (1000000000L - ele.Item2) % (count - ele.Item2);
                var target = cache[(int)index - 1].Item1;
                return Load(target);
            }
            else
            {
                cache.Add((input, count));
            }
            Console.WriteLine(string.Join("\n", input.Select(x => new string(x.ToArray()))));
            Console.WriteLine();
        }
        /*for (int i = 0; i < input.Length; i++)
        {
            int lastTag = -1;
            int os = 0;
            for (int j = 0; j < input[0].Length; j++)
            {
                if (input[i][j] == '#')
                {
                    copy[i][j] = '#';
                    lastTag = j;
                    os = 0;
                }
                if (input[i][j] == 'O' && lastTag + os + 1 < input[0].Length)
                {
                    copy[i][lastTag + ++os] = 'O';
                }
            }
        }
        input = copy.Select(x => new string(x.ToArray())).ToArray();
        copy = input.Select(x => Enumerable.Repeat('.', x.Length).ToList()).ToArray();*//*
        Console.WriteLine(string.Join("\n", input.Select(x => new string(x.ToArray()))));
        Console.WriteLine();*/
        return result;
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
}