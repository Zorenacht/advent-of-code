using FluentAssertions.Equivalency.Steps;
using MathNet.Numerics;
using System.Net.Http.Headers;
using System.Numerics;
using static AoC_2023.Day17;

namespace AoC_2023;

public sealed class Day18 : Day
{
    [Puzzle(answer: 62)]
    public int Part1Example() => Part1(InputExample);

    [Puzzle(answer: 52055)]
    public int Part1() => Part1(Input);

    public int Part1(string[] input)
    {
        var parse = input.Select(x =>
        {
            var split = x.Split();
            return (split[0], int.Parse(split[1]), split[2][1..^1]);
        }).ToList();

        var grid = Enumerable.Repeat(false, 1000).Select(x => Enumerable.Repeat(false, 1000).ToArray()).ToArray();
        var pos = new Complex(500, 500);
        grid[500][500] = true;
        var border = new HashSet<Complex>() { pos };
        foreach (var cmd in parse)
        {
            var dir = Dir(cmd.Item1);
            for (int i = 0; i < cmd.Item2; i++)
            {
                pos += dir;
                grid[(int)pos.Imaginary][(int)pos.Real] = true;
                border.Add(pos);
            }
        }
        var outside = new HashSet<Complex>() { Complex.Zero };
        var inside = new HashSet<Complex>() { };
        Print(grid);
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[0].Length; j++)
            {
                var loc = new Complex(j, i);
                if (border.Contains(loc) || outside.Contains(loc) || inside.Contains(loc)) continue;
                var que = new Queue<Complex>(); que.Enqueue(loc);
                bool foundOutside = false;
                var flood = new HashSet<Complex>();
                while (que.Count > 0)
                {
                    var ele = que.Dequeue();
                    if (flood.Contains(ele)) continue;
                    if (outside.Contains(ele)) foundOutside = true;
                    foreach (var ch in "URDL")
                    {
                        var temp = ele + Dir(new string(ch, 1));
                        if (!border.Contains(temp) && !flood.Contains(temp) && temp.Real >= 0 && temp.Real < grid[0].Length && temp.Imaginary >= 0 && temp.Imaginary < grid.Length)
                        {
                            que.Enqueue(temp);
                        }
                    }
                    flood.Add(ele);
                }
                if (foundOutside) outside.UnionWith(flood);
                else inside.UnionWith(flood);
            }
        }

        Print(grid);
        return border.Count + inside.Count;
    }

    [Puzzle(answer: 952408144115)]
    public long Part2Example() => Part2(InputExample);

    [Puzzle(answer: 67622758357096)]
    public long Part2() => Part2(Input);


    private Complex Dir(string c) => c switch
    {
        "U" => Complex.ImaginaryOne,
        "R" => Complex.One,
        "D" => -Complex.ImaginaryOne,
        "L" => -Complex.One,
        _ => throw new Exception()
    };

    public long Part2(string[] input)
    {
        var parse = input.Select<string, (int Steps, Complex Dir)>(x =>
        {
            var split = x.Split()[2][1..^1];
            Complex dir = Complex.One;
            for (int i = 0; i < split[^1] - '0'; i++)
            {
                dir *= Complex.ImaginaryOne;
            }
            return (Convert.ToInt32(split[1..^1], 16), dir);
        }).ToList();

        //create grid with reduced coordinates
        var location = Complex.Zero;
        var horizontal = new HashSet<int>() { 0 };
        var vertical = new HashSet<int>() { 0 };
        foreach (var cmd in parse)
        {
            location += cmd.Steps * cmd.Dir;
            horizontal.Add((int)location.Real);
            vertical.Add((int)location.Imaginary);
        }
        var hValues = horizontal.ToList().OrderBy(x => x).ToList().SelectMany((x, i) => i < horizontal.Count - 1 ? new[] { x, int.MaxValue } : new[] { x }).ToList();
        var vValues = vertical.ToList().OrderBy(x => x).ToList().SelectMany((x, i) => i < vertical.Count - 1 ? new[] { x, int.MaxValue } : new[] { x }).ToList();

        var grid = Enumerable.Repeat(false, vValues.Count).Select(x => Enumerable.Repeat(false, hValues.Count).ToArray()).ToArray();

        //grid with borders
        var border = new HashSet<Complex>() { };
        int rowIndex = vValues.IndexOf(0);
        int colIndex = hValues.IndexOf(0);
        foreach (var cmd in parse)
        {
            location += cmd.Steps * cmd.Dir;
            if (cmd.Dir.Real != 0)
            {
                while (hValues[colIndex] != location.Real)
                {
                    colIndex += Math.Sign(cmd.Dir.Real);
                    border.Add(new Complex(colIndex, rowIndex));
                    grid[rowIndex][colIndex] = true;
                }
            }
            if (cmd.Dir.Imaginary != 0)
            {
                while (vValues[rowIndex] != location.Imaginary)
                {
                    rowIndex += Math.Sign(cmd.Dir.Imaginary);
                    border.Add(new Complex(colIndex, rowIndex));
                    grid[rowIndex][colIndex] = true;
                }
            }
        }

        //flood fill
        var outside = new HashSet<Complex>() { Complex.Zero };
        var inside = new HashSet<Complex>() { };
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[0].Length; j++)
            {
                var loc = new Complex(j, i);
                if (border.Contains(loc) || outside.Contains(loc) || inside.Contains(loc)) continue;
                var que = new Queue<Complex>(); que.Enqueue(loc);
                bool foundOutside = false;
                var flood = new HashSet<Complex>();
                while (que.Count > 0)
                {
                    var ele = que.Dequeue();
                    if (flood.Contains(ele)) continue;
                    foreach (var ch in "URDL")
                    {
                        var temp = ele + Dir(new string(ch, 1));
                        if (!(temp.Real >= 0 && temp.Real < grid[0].Length && temp.Imaginary >= 0 && temp.Imaginary < grid.Length)) foundOutside = true;
                        else if (!border.Contains(temp) && !flood.Contains(temp))
                        {
                            que.Enqueue(temp);
                        }
                    }
                    flood.Add(ele);
                }
                if (foundOutside) outside.UnionWith(flood);
                else { 
                    foreach(var ele in flood) grid[(int)ele.Imaginary][(int)ele.Real] = true;
                    inside.UnionWith(flood); 
                }
            }
        }

        //count interior
        long interiorCount = 0;
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[0].Length; j++)
            {
                var loc = new Complex(j, i);
                if (!inside.Contains(loc)) continue;
                long verticalLength = vValues[i] == int.MaxValue ? vValues[i + 1] - vValues[i - 1] - 1 : 1;
                long horizontalLength = hValues[j] == int.MaxValue ? hValues[j + 1] - hValues[j - 1] - 1 : 1;
                interiorCount += horizontalLength * verticalLength;
            }
        }
        long borderCount = parse.Sum(x => x.Steps);
        return interiorCount + borderCount;
    }

    private static void Print(bool[][] board)
    {
        for (int i = board.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j < board[0].Length; j++)
            {

                if (board[i][j])
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write('#');
                    Console.ResetColor();
                }
                else Console.Write('.');
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}