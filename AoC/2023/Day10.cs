using MathNet.Numerics.Distributions;
using System.Collections.Generic;
using Tools.Geometry;

namespace AoC_2023;

public sealed class Day10 : Day
{
    [Puzzle(answer: 8)]
    public int Part1Example() => P1(InputExample);

    [Puzzle(answer: 6717)]
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
        Point start = Point.O;
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                if (board[j][i] == 'S')
                {
                    start = new Point(i, j);
                    break;
                }
            }
        }

        var current = new List<(Point, Direction)>() {
            (start, Direction.SW),
        };
        var visited = new HashSet<(Point, Direction)>();
        var dists = new Dictionary<Point, int>();
        int count = 0;
        while (current.Count > 0 && visited.Count(x => x.Item1 == start) < 3)
        {
            var next = new List<(Point, Direction)>();
            foreach (var curr in current)
            {
                var ch = board[curr.Item1.Y][curr.Item1.X];
                if (visited.Contains(curr) || ch == '*') continue;
                next.AddRange(Mapping(ch, curr.Item1, curr.Item2));
                visited.Add(curr);
            }
            current = next;
            count++;
        }

        /*for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                if (visited.Any(x => x.Item1 == new Point(j,i)))
                {
                    Console.Write(board[i][j]);
                }
                else { 
                    Console.Write("."); 
                }
            }
            Console.WriteLine(".");
        }*/
        return count / 2;
    }

    private IEnumerable<(Point, Direction)> Mapping(char ch, Point p, Direction dir)
    {
        return (ch, dir) switch
        {
            ('|', Direction.S) => [(p.NeighborV(Direction.N), Direction.S)],
            ('|', Direction.N) => [(p.NeighborV(Direction.S), Direction.N)],
            ('-', Direction.W) => [(p.NeighborV(Direction.E), Direction.W)],
            ('-', Direction.E) => [(p.NeighborV(Direction.W), Direction.E)],
            ('L', Direction.N) => [(p.NeighborV(Direction.E), Direction.W)],
            ('L', Direction.E) => [(p.NeighborV(Direction.N), Direction.S)],
            ('J', Direction.N) => [(p.NeighborV(Direction.W), Direction.E)],
            ('J', Direction.W) => [(p.NeighborV(Direction.N), Direction.S)],
            ('7', Direction.S) => [(p.NeighborV(Direction.W), Direction.E)],
            ('7', Direction.W) => [(p.NeighborV(Direction.S), Direction.N)],
            ('F', Direction.S) => [(p.NeighborV(Direction.E), Direction.W)],
            ('F', Direction.E) => [(p.NeighborV(Direction.S), Direction.N)],
            ('S', _) => [(p.NeighborV(Direction.W), Direction.E),
                (p.NeighborV(Direction.N), Direction.S),
                (p.NeighborV(Direction.E), Direction.W),
                (p.NeighborV(Direction.S), Direction.N)
            ],
            /*('.', Direction.N) => (p.NeighborV(Direction.E), Direction.E),
            ('S', _) => (p.NeighborV(Direction.E), Direction.E),*/
            _ => []
        };
    }

    [Puzzle(answer: 4)]
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
        var board = input.AddBorder('*');
        Point start = Point.O;
        for (int i = 0; i < board[0].Length; i++)
        {
            for (int j = 0; j < board.Length; j++)
            {
                if (board[j][i] == 'S')
                {
                    start = new Point(i, j);
                    break;
                }
            }
        }

        var current = new List<(Point, Direction)>() {
            (start, Direction.SW),
        };
        var visited = new HashSet<(Point, Direction)>();
        var visitedPoints = new HashSet<Point>();
        var dists = new Dictionary<Point, int>();
        int count = 0;
        while (current.Count > 0 && visited.Count(x => x.Item1 == start) < 3)
        {
            var next = new List<(Point, Direction)>();
            foreach (var curr in current)
            {
                var ch = board[curr.Item1.Y][curr.Item1.X];
                if (visited.Contains(curr) || ch == '*') continue;
                next.AddRange(Mapping(ch, curr.Item1, curr.Item2));
                visited.Add(curr);
                visitedPoints.Add(curr.Item1);
            }
            current = next;
            count++;
        }

        var free = new HashSet<Point>();
        var enclosed = new HashSet<Point>();
        var visited1 = new HashSet<Point>();
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                var p = new Point(i, j);
                if (board[p.X][p.Y] == '.' && !free.Contains(p) && !enclosed.Contains(p))
                {
                    var all = new HashSet<Point>() { };
                    var currs = new List<Point>() { p };
                    while (currs.Count > 0)
                    {
                        var next = new List<Point>();
                        foreach (var curr in currs)
                        {
                            if (all.Contains(curr)) continue;
                            if (board[curr.X][curr.Y] == '*')
                            {
                                all.Add(curr);
                                continue;
                            }
                            var nbs = Neighbors(curr);
                            next.AddRange(Neighbors(curr).Where(poi => board[poi.X][poi.Y] == '.' || board[poi.X][poi.Y] == '*'));
                            all.Add(curr);
                        }
                        currs = next;
                    }
                    if (all.Any(poi => board[poi.X][poi.Y] == '*')) free.UnionWith(all);
                    else enclosed.UnionWith(all);
                }
            }
        }

        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                if (free.Contains(new Point(i,j)))
                {
                    Console.Write('O');
                }
                else if (enclosed.Contains(new Point(i, j)))
                { 
                    Console.Write("I"); 
                }
                else
                {
                    Console.Write(" ");
                    /*Console.Write(board[i][j]);*/
                }
            }
            Console.WriteLine();
        }
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                Console.Write(board[i][j]);
            }
            Console.WriteLine();
        }
        return enclosed.Count;
    }

    public Point[] Neighbors(Point p)
        => [new Point(p.X, p.Y - 1),
            new Point(p.X, p.Y + 1),
            new Point(p.X - 1, p.Y),
            new Point(p.X + 1, p.Y)
            ];
}