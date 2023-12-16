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

public sealed class Day16 : Day
{
    [Puzzle(answer: null)]
    public int Part1Example() => P1(InputExample);

    [Puzzle(answer: null)]
    public int Part1() => P1(Input);

    Direction[] dirs = [
        Direction.N,
        Direction.S,
        Direction.W,
        Direction.E,
        Direction.NW,
        Direction.SW,
        Direction.SE,
        Direction.NE];

    public int P1(string[] input)
    {
        int result = 0;
        var parse = input.Select(x =>
        {
            var split1 = x.Split(" ", StringSplitOptions.None);
            var split2 = x.Split(" ", StringSplitOptions.None);
            var split3 = x.Split(" ", StringSplitOptions.None);
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
        var current = new List<Point>();
        var visited = new HashSet<Point>();
        while (current.Count > 0)
        {
            var next = new List<Point>();
            foreach (var curr in current)
            {
                if (visited.Contains(curr)) continue;
                next.AddRange(dirs.Select(x => curr.NeighborV(x)));
            }
        }
        return result;
    }

    [Puzzle(answer: null)]
    public int Part2Example() => Part2(InputExample);

    [Puzzle(answer: null)]
    public int Part2() => Part2(Input);

    public int Part2(string[] input)
    {
        int result = 0;
        var parse = input.Select(x =>
        {
            var split1 = x.Split(" ", StringSplitOptions.None);
            var split2 = x.Split(" ", StringSplitOptions.None);
            var split3 = x.Split(" ", StringSplitOptions.None);
            var kv = new KeyValuePair<string, string>(split1[0], split2[0]);
            return x;
        });
        return result;
    }

}