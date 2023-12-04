namespace AoC_2020;

public sealed class Day03 : Day
{
    [Puzzle(145)]
    public int Part1()
    {
        int row = 0, col = 0;
        int trees = 0;
        while (row < Input.Length)
        {
            if (Input[row][col] == '#') trees++;
            row += 1;
            col = (col + 3) % Input[0].Length;
        }
        return trees;
    }

    [Puzzle(3424528800)]
    public long Part2()
    {
        (int, int)[] slopes = [(1, 1), (1, 3), (1, 5), (1, 7), (2, 1)];
        long multiplier = 1;
        foreach ((int r, int c) in slopes)
        {
            int row = 0, col = 0;
            int trees = 0;
            while (row < Input.Length)
            {
                if (Input[row][col] == '#') trees++;
                row += r;
                col = (col + c) % Input[0].Length;
            }
            multiplier *= trees;
        }
        return multiplier;
    }
}