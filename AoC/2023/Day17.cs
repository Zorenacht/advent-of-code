using System.Numerics;
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

    public record State(Complex Point, Complex Dir, int Count)
    {
        public IEnumerable<State> Next(int min, int max)
        {
            if (Count < max) yield return new State(Point + Dir, Dir, Count + 1);
            if (min <= Count) yield return new State(Point + Dir * new Complex(0, +1), Dir * new Complex(0, +1), 1);
            if (min <= Count) yield return new State(Point + Dir * new Complex(0, -1), Dir * new Complex(0, -1), 1);
        }
    };

    public int Part2(string[] input, int min, int max)
    {
        var board = input.AddBorder('*').Reverse().ToArray();
        var start = new Complex(1, board.Length - 2);
        var end = new Complex(board[0].Length/2 - 2, 1);
        var pq = new PriorityQueue<State, int>();
        pq.Enqueue(new State(start + new Complex(1, 0), new Complex(1, 0), 1), 0);
        pq.Enqueue(new State(start + new Complex(0, -1), new Complex(0, -1), 1), 0);
        var visited = new HashSet<State>();
        int count = 0;
        while (pq.Count > 0)
        {
            count++;
            pq.TryDequeue(out var state, out var priority);
            var boardValue = board[(int)state!.Point.Imaginary][(int)state.Point.Real];
            var cost = boardValue - '0';
            if (state.Point == end)
            {
                Console.WriteLine(pq.Count);
                Console.WriteLine(count);
                return priority + cost;
            }
            if (boardValue == '*' || visited.Contains(state)) continue;
            foreach (var next in state.Next(min,max)) pq.Enqueue(next, priority + cost);
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