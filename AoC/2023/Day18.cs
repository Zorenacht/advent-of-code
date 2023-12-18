using FluentAssertions.Equivalency.Steps;
using System.Numerics;
using static AoC_2023.Day17;

namespace AoC_2023;

public sealed class Day18 : Day
{
    [Puzzle(answer: 62)]
    public int Part1Example() => Part2(InputExample);

    [Puzzle(answer: null)]
    public int Part1() => Part2(Input);

    [Puzzle(answer: null)]
    public int Part2Example() => Part2(InputExample);

    [Puzzle(answer: null)]
    public int Part2() => Part2(Input);


    private Complex Dir(string c) => c switch
    {
        "U" => Complex.ImaginaryOne,
        "R" => Complex.One,
        "D" => -Complex.ImaginaryOne,
        "L" => -Complex.One,
        _ => throw new Exception()
    };

    //not 142705
    public int Part2(string[] input)
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
        var inside = new HashSet<Complex>() {  };
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
                while(que.Count > 0)
                {
                    var ele = que.Dequeue();
                    if (flood.Contains(ele)) continue;
                    if(outside.Contains(ele)) foundOutside = true;
                    foreach(var ch in "URDL")
                    {
                        var temp = ele + Dir(new string(ch,1));
                        if(!border.Contains(temp) && !flood.Contains(temp) && temp.Real >= 0 && temp.Real < grid[0].Length && temp.Imaginary >= 0 && temp.Imaginary < grid.Length)
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

        /*for (int i=0; i<grid.Length; i++)
        {
            var cnt = 0;
            var inside = false;
            for (int j = 0; j < grid[0].Length; j++)
            {
                if (grid[i][j] == true) {
                    insideCount += cnt;
                    inside = !inside;
                    while (grid[i][j]) j++;
                }
                if (inside)
                {
                    grid[i][j] = true;
                    cnt++;
                }
            }
        }*/
        Print(grid);
        return border.Count + inside.Count;
    }

    private static void Print(bool[][] board)
    {
        for (int i = 700; i >= 300; i--)
        {
            for (int j = 480; j < 900; j++)
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
    }
}