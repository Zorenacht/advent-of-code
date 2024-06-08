namespace AoC_2023;

public sealed class Day03 : Day
{
    [Puzzle(answer: 538046)]
    public int Part1()
    {
        var result = new List<int>();
        var lines = Input.AddBorder('.');
        for (int i = 0; i < lines.Length; i++)
        {
            int accu = 0;
            int numberStartIndex = -1;
            for (int j = 0; j < lines[0].Length; j++)
            {
                if (Digits.Contains(lines[i][j]))
                {
                    accu = accu * 10 + lines[i][j].ToInt();
                    if (numberStartIndex == -1) numberStartIndex = j;
                }
                else if (numberStartIndex != -1)
                {
                    bool contains = false;
                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        if (lines[row][(numberStartIndex - 1)..(j + 1)].Any(x => !DigitsOrDot.Contains(x)))
                        {
                            contains = true;
                            break;
                        }
                    }
                    if (contains) result.Add(accu);
                    numberStartIndex = -1;
                    accu = 0;
                }
            }
        }
        return result.Sum();
    }

    public string DigitsOrDot = "0123456789.";
    public string Digits = "0123456789";

    [Puzzle(answer: 81709807)]
    public int Part2()
    {
        var result = new List<int>();
        var lines = Input.AddBorder('.');
        var dict = new Dictionary<(int, int), List<int>>();
        for (int i = 0; i < lines.Length; i++)
        {
            int accu = 0;
            int numberStartIndex = -1;
            for (int j = 0; j < lines[0].Length; j++)
            {
                if (char.IsDigit(lines[i][j]))
                {
                    accu = accu * 10 + lines[i][j].ToInt();
                    if (numberStartIndex == -1) numberStartIndex = j;
                }
                else if (numberStartIndex != -1)
                {
                    for (int col = i - 1; col <= i + 1; col++)
                    {
                        int index = lines[col][(numberStartIndex - 1)..(j + 1)].IndexOf('*');
                        if (index != -1)
                        {
                            index += numberStartIndex - 1;
                            if (dict.ContainsKey((col, index))) dict[(col, index)].Add(accu);
                            else dict[(col, index)] = new List<int>() { accu };
                        }
                    }
                    numberStartIndex = -1;
                    accu = 0;
                }
            }
        }
        return dict.Where(x => x.Value.Count == 2).Sum(x => x.Value[0] * x.Value[1]);
    }
}