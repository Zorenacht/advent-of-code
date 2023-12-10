using MathNet.Numerics.Distributions;
using MathNet.Numerics.RootFinding;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Tools.Geometry;

namespace AoC_2023;

public sealed class Day10 : Day
{
    [Puzzle(answer: 80)]
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

    private void PrintColor(string[] board, params (HashSet<Point> Set, ConsoleColor Color)[] highlight)
    {
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                if(highlight.Any(x => x.Set.Contains(new Point(j, i))))
                {
                    foreach (var h in highlight)
                    {
                        if (h.Set.Contains(new Point(j, i)))
                        {
                            Console.BackgroundColor = h.Color;
                            Console.Write(board[i][j]);
                            Console.ResetColor();
                            break;
                        }
                    }
                }
                else if (board[i][j] == '.')
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(board[i][j]);
                    Console.ResetColor();
                }
                else if (board[i][j] == '*')
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(board[i][j]);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(board[i][j]);
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public int P1(string[] input)
    {
        var board = input.AddBorder('*');
        Point start = Point.O;
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                if (board[i][j] == 'S')
                {
                    start = new Point(j, i);
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
            //PrintColor(board, current.Select(x => x.Item1).ToHashSet());
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

    [Puzzle(answer: 10)]
    public int Part2Example() => Part2(InputExample);

    //lower probably than 547]
    //not 479
    //not 382
    [Puzzle(answer: 381)]
    public int Part2() => Part2(Input);

    public int Part2(string[] input)
    {
        var board = input.AddBorder('*');
        Point start = Point.O;
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                if (board[i][j] == 'S')
                {
                    start = new Point(j, i);
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
            //PrintColor(board, current.Select(x => x.Item1).ToHashSet());
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
        visited.Remove((start, Direction.SW));

        var cycleStart = visited.First(x => x.Item1 == start);
        var cycle = CyclePoints(
            board,
            cycleStart.Item1,
            cycleStart.Item2);

        /*var free = new HashSet<Point>();
        var enclosed = new HashSet<Point>();
        var visited1 = new HashSet<Point>();
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                var p = new Point(j, i);
                if (board[p.Y][p.X] != '*' && !cycle.Contains(p) && !free.Contains(p) && !enclosed.Contains(p))
                {
                    var all = new HashSet<Point>() { };
                    var currs = new List<Point>() { p };
                    while (currs.Count > 0)
                    {
                        var next = new List<Point>();
                        foreach (var curr in currs)
                        {
                            if (all.Contains(curr)) continue;
                            if (board[curr.Y][curr.X] == '*')
                            {
                                all.Add(curr);
                                continue;
                            }
                            var nbs = Neighbors(curr);
                            next.AddRange(Neighbors(curr).Where(poi => board[poi.Y][poi.X] == '.' || board[poi.Y][poi.X] == '*'));
                            all.Add(curr);
                        }
                        currs = next;
                    }
                    if (all.Any(poi => board[poi.Y][poi.X] == '*')) free.UnionWith(all);
                    else enclosed.UnionWith(all);
                }
            }
        }*/

        /*var left = new HashSet<Point>();
        var right = new HashSet<Point>();
        var first = visited.First(x => x.Item1 == start);
        var looping = visited.First(x => x.Item1 == start);
        looping = (looping.Item1.NeighborV(looping.Item2), (Direction)(((int)looping.Item2 + 4)  % 8));
        do
        {
            var p = looping.Item1;
            var l = Left(looping.Item1, looping.Item2);
            var r = Right(looping.Item1, looping.Item2);
            if (board[l.Y][l.X] == '.' && !left.Contains(r) && !right.Contains(r))
            {
                left.Add(l);
            }
            if (board[r.Y][r.X] == '.' && !left.Contains(r) && !right.Contains(r))
            {
                right.Add(r);
            }
            looping = Mapping(board[p.Y][p.X], looping.Item1, looping.Item2).First();
        } while (looping.Item1 != start);*/

        var left = new HashSet<Point>();
        var right = new HashSet<Point>();
        var iterator = (
            cycleStart.Item1.NeighborV(cycleStart.Item2),
            (Direction)(((int)cycleStart.Item2 + 4) % 8));
        do
        {
            var p = iterator.Item1;
            var dir = iterator.Item2;
            var lef = Left(p, dir, board[p.Y][p.X]);
            var rig = Right(p, dir, board[p.Y][p.X]);
            foreach(var l in lef)
            {
                if (board[l.Y][l.X] != '*' && !cycle.Contains(l) && !left.Contains(l) && !right.Contains(l))
                {
                    left.Add(l);
                }
            }
            foreach (var r in rig)
            {
                if (board[r.Y][r.X] != '*' && !cycle.Contains(r) && !left.Contains(r) && !right.Contains(r))
                {
                    right.Add(r);
                }
            }
            iterator = Mapping(board[p.Y][p.X], iterator.Item1, iterator.Item2).First();
            cycle.Add(iterator.Item1);
        } while (iterator.Item1 != start);


        //PrintColor(board, [(free, ConsoleColor.Yellow), (enclosed, ConsoleColor.Green), (left, ConsoleColor.Red) , (right, ConsoleColor.Blue)]);
        PrintColor(board, [(cycle, ConsoleColor.Magenta), (left, ConsoleColor.Red), (right, ConsoleColor.Blue)]);


        FloodFill(board, left, right, cycle); 
        PrintColor(board, [(cycle, ConsoleColor.Magenta), (left, ConsoleColor.Red), (right, ConsoleColor.Blue)]);

        //enclosed.ExceptWith(outsideBorder);
        //return enclosed.Count;
        return Math.Min(left.Count, right.Count);
    }

    public void FloodFill(string[] board, HashSet<Point> left, HashSet<Point> right, HashSet<Point> cycle)
    {
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                var p = new Point(j, i);
                bool? foundLeft = null;
                if (board[p.Y][p.X] != '*' && !cycle.Contains(p) && !left.Contains(p) && !right.Contains(p))
                {
                    var all = new HashSet<Point>() { };
                    var currs = new List<Point>() { p };
                    while (currs.Count > 0)
                    {
                        var next = new List<Point>();
                        foreach (var curr in currs)
                        {
                            if (all.Contains(curr) || cycle.Contains(curr) || board[curr.Y][curr.X] == '*') continue;
                            /*if (board[curr.Y][curr.X] == '*')
                            {
                                all.Add(curr);
                                continue;
                            }*/
                            var nbs = Neighbors(curr);
                            next.AddRange(Neighbors(curr).Where(poi => !cycle.Contains(poi) && board[poi.Y][poi.X] != '*'));
                            all.Add(curr);
                        }
                        currs = next;
                    }
                    if (all.Overlaps(left)) foundLeft = true;
                    if (all.Overlaps(right)) foundLeft = false;
                    all.UnionWith(all);
                    if (foundLeft is null) throw new Exception();
                    else if (foundLeft.Value) left.UnionWith(all);
                    else if (!foundLeft.Value) right.UnionWith(all);
                }
            }
        }
    }

    public HashSet<Point> CyclePoints(string[] board, Point start, Direction direction)
    {
        var iterator = (start.NeighborV(direction), (Direction)(((int)direction + 4) % 8));
        var cycle = new HashSet<Point>();
        do
        {
            var p = iterator.Item1;
            cycle.Add(iterator.Item1);
            iterator = Mapping(board[p.Y][p.X], iterator.Item1, iterator.Item2).First();
        } while (iterator.Item1 != start);
        cycle.Add(iterator.Item1);
        return cycle;
    }


    public IEnumerable<Point> Left(Point p, Direction incoming, char ch)
    {
        var left = (Direction)(((int)incoming + 2) % 8);
        yield return p.NeighborV(left);
        var nextIncomingDir = Mapping(ch, p, incoming).First().Item2;
        var diff = ((int)nextIncomingDir - (int)incoming + 8) % 8;
        var straight = (Direction)(((int)incoming + 4) % 8);
        if (diff == 2) yield return p.NeighborV(straight);
    }

    public IEnumerable<Point> Right(Point p, Direction incoming, char ch)
    {
        var right = (Direction)(((int)incoming + 6) % 8);
        yield return p.NeighborV(right);
        var nextIncomingDir = Mapping(ch, p, incoming).First().Item2;
        var diff = ((int)nextIncomingDir - (int)incoming + 8) % 8;
        var straight = (Direction)(((int)incoming + 4) % 8);
        if (diff == 6) yield return p.NeighborV(straight);
    }

    public Point[] Neighbors(Point p)
        => [new Point(p.X, p.Y - 1),
            new Point(p.X, p.Y + 1),
            new Point(p.X - 1, p.Y),
            new Point(p.X + 1, p.Y)
            ];
}