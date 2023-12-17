using MathNet.Numerics;
using ShortestPath;
using System.Diagnostics.Metrics;
using System.Drawing;
using static AoC_2023.Day17;

namespace AoC_2023;

public sealed class Day17 : Day
{
    [Puzzle(answer: null)]
    public int Part1Example() => P1(InputExample);

    [Puzzle(answer: null)]
    public int Part1() => P1(Input);

    public record State(Complex32 Point, Complex32 Dir, int Count, int Distance);

    public int P1(string[] input)
    {
        var board = input.AddBorder('*').Reverse().ToArray();
        var pq = new PriorityQueue<State, int>();
        pq.Enqueue(new State(new Complex32(1, board.Length - 2), new Complex32(1, 0), 1, 0), 0);
        pq.Enqueue(new State(new Complex32(1, board.Length - 2), new Complex32(0, -1), 1, 0), 0);
        var dict = new Dictionary<State, int>();
        while (pq.Count > 0)
        {
            pq.TryDequeue(out var state, out var priority);
            //Console.WriteLine(string.Join("\n", board.Select(x => x)));
            //Console.WriteLine();
            //Print(board, state);
            var boardValue = board[(int)state.Point.Imaginary][(int)state.Point.Real];
            var cost = boardValue - '0';
            if (boardValue == '*' || dict.ContainsKey(state)) continue;
            //if (state.Point == new Complex32(board[0].Length - 2, 1)) return state.Distance + cost;
            pq.Enqueue(new State(state.Point + state.Dir * new Complex32(0, 1), state.Dir * new Complex32(0, 1), 1, state.Distance + cost),
                state.Distance + cost);
            pq.Enqueue(new State(state.Point + state.Dir * new Complex32(0, -1), state.Dir * new Complex32(0, -1), 1, state.Distance + cost),
                state.Distance + cost);
            if (state.Count < 2) pq.Enqueue(new State(state.Point + state.Dir, state.Dir, state.Count+1, state.Distance + cost),
                state.Distance + cost);
            //if (Count < 3) yield return new Node<St>(new St(Point + Dir, Dir, Count + 1), initialDistance + );
            dict.Add(state, state.Distance);
        }

        return dict.Where(x => x.Key.Point == new Complex32(board[0].Length - 2, 1)).Min(x => x.Value);
    }

    private static void Print(string[] board, State state)
    {
        for (int i = board.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                if (new Complex32(j, i) == state.Point - state.Dir)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(board[i][j]);
                    Console.ResetColor();
                }
                else if (new Complex32(j, i) == state.Point)
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

    [Puzzle(answer: null)]
    public int Part2Example() => Part2(InputExample);

    [Puzzle(answer: null)]
    public int Part2() => Part2(Input);

    public int Part2(string[] input)
    {
        return 0;
    }

}