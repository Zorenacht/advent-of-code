using MathNet.Numerics.Distributions;
using MathNet.Numerics.RootFinding;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
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

    private class Loop : IEnumerable<Point>
    {
        public readonly string[] _board;
        public readonly HashSet<Point> _cycle;
        public PointDirection Start { get; private set; }

        public Loop(string[] board)
        {
            _board = board.AddBorder('*');
            var start = _board
                .SelectMany((line, row) => line
                    .Select((ch, col) => (col, ch))
                    .Where(comb => comb.ch == 'S')
                    .Select(comb => new Point(comb.col, row)))
                .First();
            var dir = StartDirection(_board, start);
            _cycle = CyclePoints(_board, start, dir);
            Start = new PointDirection(start, dir);
        }

        public int Length => _cycle.Count;

        public int Inner()
        {
            return 0;
        }

        public record PointDirection(Point Point, Direction Direction);

        private Direction StartDirection(string[] board, Point start)
        {
            PointDirection[] nb = [
                new(start.NeighborV(Direction.W), Direction.E),
                new(start.NeighborV(Direction.N), Direction.S),
                new(start.NeighborV(Direction.E), Direction.W),
                new(start.NeighborV(Direction.S), Direction.N)];
            var incomingDirection = nb.First(pd => Mapping(board[pd.Point.Y][pd.Point.X], pd.Point, pd.Direction).Any())
                .Direction;
            return (Direction)(((int)incomingDirection + 4) % 8);

        }

        public IEnumerator<Point> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private void PrintColor(string[] board, params (HashSet<Point> Set, ConsoleColor Color)[] highlight)
    {
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                if (highlight.Any(x => x.Set.Contains(new Point(j, i))))
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

    public int P1(string[] input) => new Loop(input).Length / 2;

    private static IEnumerable<(Point, Direction)> Mapping(char ch, Point p, Direction dir)
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
        var loop = new Loop(input);
        var start = loop.Start.Point;
        var board = loop._board;
        var cycle = loop._cycle;

        var left = new HashSet<Point>();
        var right = new HashSet<Point>();
        var iterator = (
            start.NeighborV(loop.Start.Direction),
            (Direction)(((int)loop.Start.Direction + 4) % 8));
        do
        {
            var p = iterator.Item1;
            var dir = iterator.Item2;
            var lef = Left(p, dir, board[p.Y][p.X]);
            var rig = Right(p, dir, board[p.Y][p.X]);
            foreach (var l in lef)
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

    public static HashSet<Point> CyclePoints(string[] board, Point start, Direction direction)
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