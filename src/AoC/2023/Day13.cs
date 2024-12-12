namespace AoC._2023;

public sealed class Day13 : Day
{
    [Puzzle(answer: 405)]
    public int Part1Example() => P2(InputExampleAsText, 0);

    [Puzzle(answer: 35538)]
    public int Part1() => P2(InputAsText, 0);

    [Puzzle(answer: 400)]
    public int Part2Example() => P2(InputExampleAsText, 1);

    [Puzzle(answer: 30442)]
    public int Part2() => P2(InputAsText, 1);

    public int P2(string input, int amountOfInvalid)
    {
        int result = 0;
        var boards = input.Split($"{Environment.NewLine}{Environment.NewLine}")
            .Select(x => x.Split(Environment.NewLine)).ToList();
        foreach (var board in boards)
        {
            var mirrorRow = -1;
            var mirrorCol = -1;
            for (int row = 1; row < board.Length; row++)
            {
                var first = board[..row].Reverse().ToArray();
                var second = board[row..];
                if (Differences(first, second) == amountOfInvalid)
                {
                    mirrorRow = row;
                    break;
                }
            }
            for (int col = 1; col < board[0].Length; col++)
            {
                var first = board.Select(x => new string(x[..col].Reverse().ToArray())).ToArray();
                var second = board.Select(x => x[col..]).ToArray();
                if (Differences(first, second) == amountOfInvalid)
                {
                    mirrorCol = col;
                    break;
                }
            }
            if (mirrorRow > 0) result += mirrorRow * 100;
            else if (mirrorCol > 0) result += mirrorCol;
            else throw new Exception();
        }
        return result;
    }

    private int Differences(string[] first, string[] second)
    {
        int unequal = 0;
        for (int i = 0; i < Math.Min(first.Length, second.Length); i++)
        {
            for (int j = 0; j < Math.Min(first[0].Length, second[0].Length); j++)
            {
                if (first[i][j] != second[i][j]) unequal++;
            }
        }
        return unequal;
    }

}