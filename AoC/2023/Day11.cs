using Tools.Geometry;

namespace AoC_2023;

public sealed class Day11 : Day
{
    [Puzzle(answer: 374)]
    public long Part1Example() => Part1(InputExample, 2);

    [Puzzle(answer: 10231178)]
    public long Part1() => Part1(Input, 2);

    Direction[] dirs = [
        Direction.N,
        Direction.S,
        Direction.W,
        Direction.E];

    public long Part1(string[] board, int expansion)
    {
        var galaxies = new List<Point>();
        var rowHasGalaxy = new bool[board.Length];
        var colHasGalaxy = new bool[board[0].Length];
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[0].Length; j++)
            {
                if (board[i][j] == '#')
                {
                    galaxies.Add(new Point(i, j));
                    rowHasGalaxy[i] = true;
                    colHasGalaxy[j] = true;
                }
            }
        }

        long sum = 0;
        for(int fromIndex = 0; fromIndex < galaxies.Count; fromIndex++)
        {
            for (int toIndex = fromIndex+1; toIndex < galaxies.Count; toIndex++)
            {
                long dist = 0;
                var from = galaxies[fromIndex];
                var to = galaxies[toIndex];

                //row walk
                var row = from.X;
                while (row < to.X)
                {
                    dist += rowHasGalaxy[row] ? 1 : expansion;
                    row++;
                }

                //col walk
                var sign = Math.Sign(to.Y - from.Y);
                var col = from.Y;
                while (Math.Abs(to.Y - col) > 0)
                {
                    dist += colHasGalaxy[col] ? 1 : expansion;
                    if (Math.Abs(to.Y - col) == 0) break;
                    col += sign;
                }
                sum += dist;
            }
        }
        return sum;
    }

    [Puzzle(answer: 8410)]
    public long Part2Example() => Part1(InputExample, 100);

    [Puzzle(answer: 622120986954)]
    public long Part2() => Part1(Input, 1_000_000);
}