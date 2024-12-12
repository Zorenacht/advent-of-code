namespace AoC._2023;

public sealed class Day09 : Day
{
    [Puzzle(answer: 114)]
    public long Part1Example() => Part1(
        InputExample
            .Select(x => x.Split().Select(x => int.Parse(x)).ToArray())
            .ToArray());

    [Puzzle(answer: 1806615041)]
    public long Part1() => Part1(
        Input
            .Select(x => x.Split().Select(x => int.Parse(x)).ToArray())
            .ToArray());

    [Puzzle(answer: 2)]
    public long Part2Example() => Part1(
        InputExample
            .Select(x => x.Split()
                .Select(x => int.Parse(x))
                .Reverse()
                .ToArray())
            .Reverse()
            .ToArray());

    [Puzzle(answer: 1211)]
    public long Part2() => Part1(
        Input
            .Select(x => x.Split()
                .Select(x => int.Parse(x))
                .Reverse()
                .ToArray())
            .Reverse()
            .ToArray());

    public long Part1(int[][] input)
    {
        long sum = 0;
        foreach (var line in input)
        {
            var curr = line.ToList();
            while (curr.Any(x => x != 0))
            {
                var next = new List<int>();
                sum += curr[^1];
                for (int i = 0; i < curr.Count - 1; i++)
                {
                    next.Add(curr[i + 1] - curr[i]);
                }
                curr = next;
            }

        }
        return sum;
    }
}