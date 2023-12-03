using FluentAssertions;
using Tools;

namespace AoC_2023;

public sealed class Day03 : Day
{
    [Puzzle(answer: 538046)]
    public int Part1(string input)
    {
        var result = new List<int>();
        var lines = input.Lines();
        var overlap = new char[lines.Length][];
        for(int i = 0; i < lines.Length; i++)
        {
            overlap[0] = new char[lines[i].Length];
        }
        for(int i=0; i<lines.Length; i++)
        {
            int accu = 0;
            int startI = -1;
            int endI = -1;
            for (int j = 0; j < lines[0].Length; j++)
            {
                if(lines[i][j] == '.')
                {
                    if (startI != -1)
                    {
                        endI = j;

                        int minRow = i > 0 ? i - 1 : i;
                        int maxRow = i < lines.Length-1 ? i + 1 : i;
                        int minCol = startI > 0 ? startI - 1 : startI;
                        int maxCol = j;
                        bool contains = false;
                        for (int a = minRow; a <= maxRow; a++)
                        {
                            if (lines[a][minCol..(maxCol + 1)].Any(x => !DigitsOrDot.Contains(x)))
                            {
                                contains = true;
                                break;
                            }
                        }
                        if(contains) result.Add(accu);
                        startI = -1;
                        accu = 0;
                    }
                }
                else if(Digits.Contains(lines[i][j])) 
                {
                    accu = accu * 10 + (lines[i][j] - '0');
                    if (startI == -1) startI = j;
                }
                else
                {
                    if (startI != -1)
                    {
                        result.Add(accu);
                        startI = -1;
                        accu = 0;
                    }
                }
            }
            if (startI != -1)
            {
                int minRow = i > 0 ? i - 1 : i;
                int maxRow = i < lines.Length - 1 ? i + 1 : i;
                int minCol = startI > 0 ? startI - 1 : startI;
                int maxCol = lines[0].Length - 1;
                bool contains = false;
                for (int a = minRow; a <= maxRow; a++)
                {
                    if (lines[a][minCol..(maxCol + 1)].Any(x => !DigitsOrDot.Contains(x)))
                    {
                        contains = true;
                        break;
                    }
                }
                if (contains) result.Add(accu);
                accu = 0;
            }
        }
        return result.Sum();
    }

    public string DigitsOrDot = "0123456789.";
    public string Digits = "0123456789";

    [Puzzle(answer: 81709807)]
    public int Part2(string input)
    {
        var result = new List<int>();
        var lines = input.Lines();
        var overlap = new char[lines.Length][];
        var dict = new Dictionary<(int, int), List<int>>();
        for (int i = 0; i < lines.Length; i++)
        {
            overlap[0] = new char[lines[i].Length];
        }
        for (int i = 0; i < lines.Length; i++)
        {
            int accu = 0;
            int startI = -1;
            int endI = -1;
            for (int j = 0; j < lines[0].Length; j++)
            {
                if (lines[i][j] == '.' || lines[i][j] == '*')
                {
                    if (startI != -1)
                    {
                        endI = j;

                        int minRow = i > 0 ? i - 1 : i;
                        int maxRow = i < lines.Length - 1 ? i + 1 : i;
                        int minCol = startI > 0 ? startI - 1 : startI;
                        int maxCol = j;
                        bool contains = false;
                        for (int a = minRow; a <= maxRow; a++)
                        {
                            int index = minCol + lines[a][minCol..(maxCol + 1)].IndexOf('*');
                            if (lines[a][minCol..(maxCol + 1)].IndexOf('*') != -1)
                            {
                                if (dict.ContainsKey((a, index))) dict[(a, index)].Add(accu);
                                else dict[(a, index)] = new List<int>() { accu };
                            }
                        }
                        startI = -1;
                        accu = 0;
                    }
                }
                else if (Digits.Contains(lines[i][j]))
                {
                    accu = accu * 10 + (lines[i][j] - '0');
                    if (startI == -1) startI = j;
                }
                else
                {
                    if (startI != -1)
                    {
                        startI = -1;
                        accu = 0;
                    }
                }
            }
            if (startI != -1)
            {
                int minRow = i > 0 ? i - 1 : i;
                int maxRow = i < lines.Length - 1 ? i + 1 : i;
                int minCol = startI > 0 ? startI - 1 : startI;
                int maxCol = lines[0].Length - 1;
                bool contains = false;
                for (int a = minRow; a <= maxRow; a++)
                {
                    int index = minCol + lines[a][minCol..(maxCol + 1)].IndexOf('*');
                    if (lines[a][minCol..(maxCol + 1)].IndexOf('*') != -1)
                    {
                        if (dict.ContainsKey((a, index))) dict[(a, index)].Add(accu);
                        else dict[(a, index)] = new List<int>() { accu };
                    }
                }
                accu = 0;
            }
        }
        return dict.Where(x => x.Value.Count == 2).Sum(x => x.Value[0] * x.Value[1]);
    }
}