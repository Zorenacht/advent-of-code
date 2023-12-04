namespace AoC_2020;

public sealed class Day01 : Day
{
    [Puzzle(776064)]
    public int Part1()
    {
        int result = 0;
        var parse = Input.Select(Int32.Parse).ToArray();
        var set = new HashSet<int>(parse);
        for (int i = 0; i < parse.Count(); i++)
        {
            if (set.Contains(2020 - parse[i]))
            {
                result = parse[i] * (2020 - parse[i]);
                break;
            }
        }
        return result;
    }

    [Puzzle(6964490)]
    public int Part2()
    {
        int result = 0;
        var parse = Input.Select(Int32.Parse).ToArray();
        var set = new HashSet<int>(parse);
        for (int i = 0; i < parse.Count(); i++)
        {
            for (int j = i + 1; j < parse.Count(); j++)
            {
                if (set.Contains(2020 - parse[i] - parse[j]))
                {
                    result = parse[i] * parse[j] * (2020 - parse[i] - parse[j]);
                    break;
                }
            }
        }
        return result;
    }
}