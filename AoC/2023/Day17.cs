using MathNet.Numerics;
using System.Numerics;
using System.Runtime.CompilerServices;
using static AoC_2023.Day17;

namespace AoC_2023;

public sealed class Day17 : Day
{
    [Puzzle(answer: 102)]
    public int Part1Example() => Part2(InputExample, 0, 3);

    [Puzzle(answer: 1128)]
    public int Part1() => Part2(Input, 0, 3);

    [Puzzle(answer: 94)]
    public int Part2Example() => Part2(InputExample, 4, 10);

    [Puzzle(answer: 1268)]
    public int Part2() => Part2(Input, 4, 10);

    public record State(Complex Point, Complex Dir, int Count);
    public record Priority(int Distance, int Heuristic) : IComparer<Priority>
    {
        public int Compare(Priority? x, Priority? y)
        {
            return (x?.Distance + x?.Heuristic ?? -1) - (y?.Distance + y?.Heuristic ?? -1);
        }

        public int CompareTo(Priority? other) => Distance + Heuristic - (other?.Distance + other?.Heuristic ?? -1);
    }
    private int MDiff(Complex first, Complex second)
    {
        var diff = first - second;
        return ((int)(Math.Abs(diff.Real) + Math.Abs(diff.Imaginary)))*100;
    }


    public int Part2(string[] input, int min, int max)
    {
        var board = input.AddBorder('*').Reverse().ToArray();
        var start = new Complex(1, board.Length - 2);
        var end = new Complex(board[0].Length - 2, 1);
        var pq = new PriorityQueue<State, Priority>((IComparer<Priority>)new Priority(0, 0));
        var dists = board.Select((row, ri) => row.Select((col, ci) => MDiff(new Complex(ci, ri), end)).ToArray()).ToArray();
        var heur = (Complex comp) => dists[(int)comp.Imaginary][(int)comp.Real];
        pq.Enqueue(new State(start + new Complex(1, 0), new Complex(1, 0), 1), new Priority(0, MDiff(start + new Complex(1, 0), end)));
        pq.Enqueue(new State(start + new Complex(0, -1), new Complex(0, -1), 1), new Priority(0, MDiff(start + new Complex(0, -1), end)));
        var visited = new HashSet<State>();
        while (pq.Count > 0)
        {
            pq.TryDequeue(out var state, out var priority);
            var boardValue = board[(int)state!.Point.Imaginary][(int)state.Point.Real];
            var cost = boardValue - '0';
            if (state.Point == end)
            {
                return priority!.Distance + cost;
            }
            if (boardValue == '*' || visited.Contains(state)) continue;

            if (state.Count < max)
            {
                var newDir = state.Dir;
                var newPoint = state.Point + newDir;
                pq.Enqueue(new State(newPoint, newDir, state.Count + 1),
                new(priority!.Distance + cost, heur(newPoint)));
            }
            if (state.Count >= min)
            {
                var newDir = state.Dir * new Complex(0, +1);
                var newPoint = state.Point + newDir;
                pq.Enqueue(new State(state.Point + state.Dir * new Complex(0, +1), state.Dir * new Complex(0, +1), 1),
                    new(priority!.Distance + cost, heur(newPoint)));
                newDir = state.Dir * new Complex(0, -1);
                newPoint = state.Point + newDir;
                pq.Enqueue(new State(state.Point + state.Dir * new Complex(0, -1), state.Dir * new Complex(0, -1), 1),
                    new(priority!.Distance + cost, heur(newPoint)));
            }

            visited.Add(state);
        }
        throw new Exception("Endpoint was not reached!");
    }

    private static void Print(string[] board, State state)
    {
        if (board[(int)state!.Point.Imaginary][(int)state.Point.Real] == '*') return;
        for (int i = board.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                if (new Complex(j, i) == state.Point - state.Dir)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(board[i][j]);
                    Console.ResetColor();
                }
                else if (new Complex(j, i) == state.Point)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
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
    }
}