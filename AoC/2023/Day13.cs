using AoC;
using MathNet.Numerics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using Tools.Geometry;
using Tools.Shapes;

namespace AoC_2023;

public sealed class Day13 : Day
{
    [Puzzle(answer: 405)]
    public int Part1Example() => P2(InputExampleAsText,0);

    [Puzzle(answer: 35538)]
    public int Part1() => P2(InputAsText,0);

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
                var smudges = new HashSet<(int, int)>();
                bool valid = true;
                for (int i = 0; i < Math.Min(row, board.Length - row); i++)
                {
                    for (int j = 0; j < board[0].Length; j++)
                    {
                        if (board[row + i][j] != board[row - i - 1][j])
                        {
                            if (smudges.Count < amountOfInvalid)
                            {
                                smudges.Add((i, j));
                            }
                            else
                            {
                                i = int.MaxValue - 10;
                                j = int.MaxValue - 10;
                                valid = false;
                            }
                        }
                    }
                }
                if (valid && smudges.Count == amountOfInvalid)
                {
                    mirrorRow = row;
                    break;
                }
            }
            for (int col = 1; col < board[0].Length; col++)
            {
                var smudges = new HashSet<(int, int)>();
                bool valid = true;
                for (int i = 0; i < board.Length; i++)
                {
                    for (int j = 0; j < Math.Min(col, board[0].Length - col); j++)
                    {
                        if (board[i][col + j] != board[i][col - j - 1])
                        {
                            if (smudges.Count < amountOfInvalid)
                            {
                                smudges.Add((i, j));
                            }
                            else
                            {
                                i = int.MaxValue - 10;
                                j = int.MaxValue - 10;
                                valid = false;
                            }
                        }
                    }
                }
                if (valid && smudges.Count == amountOfInvalid)
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

}