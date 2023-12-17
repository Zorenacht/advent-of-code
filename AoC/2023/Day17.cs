using MathNet.Numerics;
using ShortestPath;
using System.Diagnostics.Metrics;
using System.Drawing;
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

    public record State(Complex32 Point, Complex32 Dir, int Count);

    public int Part2(string[] input, int min, int max)
    {
        var board = input.AddBorder('*').Reverse().ToArray();
        var pq = new PriorityQueue<State, int>();
        var start = new Complex32(1, board.Length - 2);
        pq.Enqueue(new State(start + new Complex32(1, 0), new Complex32(1, 0), 1), 0);
        pq.Enqueue(new State(start + new Complex32(0, -1), new Complex32(0, -1), 1), 0);
        var dict = new Dictionary<State, int>();
        while (pq.Count > 0)
        {
            pq.TryDequeue(out var state, out var priority);
            //Console.WriteLine(string.Join("\n", board.Select(x => x)));
            //Console.WriteLine();
            //Print(board, state);
            var boardValue = board[(int)state!.Point.Imaginary][(int)state.Point.Real];
            var cost = boardValue - '0';
            if (boardValue == '*' || dict.ContainsKey(state)) continue;
            //if (state.Point == new Complex32(board[0].Length - 2, 1)) return state.Distance + cost;
            if (state.Count >= min) pq.Enqueue(new State(state.Point + state.Dir * new Complex32(0, 1), state.Dir * new Complex32(0, 1), 1),
                priority + cost);
            if (state.Count >= min) pq.Enqueue(new State(state.Point + state.Dir * new Complex32(0, -1), state.Dir * new Complex32(0, -1), 1),
                priority + cost);
            if (state.Count < max) pq.Enqueue(new State(state.Point + state.Dir, state.Dir, state.Count + 1),
                priority + cost);
            dict.Add(state, priority + cost);
        }

        return dict.Where(x => x.Key.Point == new Complex32(board[0].Length - 2, 1) && x.Key.Count >= min).Min(x => x.Value);
    }


    private static void Print(string[] board, State state)
    {
        if (board[(int)state!.Point.Imaginary][(int)state.Point.Real] == '*') return;
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
}