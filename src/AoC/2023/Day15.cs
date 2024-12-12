namespace AoC._2023;

public sealed class Day15 : Day
{
    [Puzzle(answer: 1320)]
    public long Part1Example() => InputExampleAsText.Split(",").Sum(word => HASH(word));

    [Puzzle(answer: 517015)]
    public long Part1() => InputAsText.Split(",").Sum(word => HASH(word));

    private int[] Rows = [-1, 0, 1, 0];
    private int[] Cols = [0, -1, 0, 1];

    [Puzzle(answer: 145)]
    public long Part2Example() => Part2(InputExampleAsText);

    [Puzzle(answer: 286104)]
    public long Part2() => Part2(InputAsText);

    private record Lens(string Label)
    {
        public int Strength { get; set; }
    }

    private int HASH(string word)
    {
        int sum = 0;
        foreach (char ch in word)
        {
            sum = (sum + (int)ch) * 17;
            sum %= 256;
        }
        return sum;
    }

    public long Part2(string input)
    {
        List<List<Lens>> boxes = Enumerable.Range(0, 256).Select(x => new List<Lens>()).ToList();

        var split = input.Split(",");
        foreach (var cmd in split)
        {
            if (cmd[^1] == '-')
            {
                var remove = cmd[..^1];
                int box = HASH(remove);
                for (int i = 0; i < boxes[box].Count; i++)
                {
                    if (boxes[box][i].Label == remove)
                    {
                        boxes[box].RemoveAt(i);
                    }
                }
            }
            else
            {
                var replace = cmd.Split("=");
                int box = HASH(replace[0]);
                int value = int.Parse(replace[1]);
                if (boxes[box].FirstOrDefault(x => x.Label == replace[0]) is { } res)
                {
                    res.Strength = value;
                }
                else
                {
                    boxes[box].Add(new Lens(replace[0]) { Strength = value });
                }
            }
        }
        int result = 0;
        for (int i = 0; i < boxes.Count; i++)
        {
            for (int j = 0; j < boxes[i].Count; j++)
            {
                result += (i + 1) * (j + 1) * boxes[i][j].Strength;
            }
        }
        return result;
    }
}