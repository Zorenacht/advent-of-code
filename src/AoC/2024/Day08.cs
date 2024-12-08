using Tools.Shapes;

namespace AoC_2024;

public sealed class Day08 : Day
{
    [Puzzle(answer: 228)]
    public int Part1() => Antinodes(Input, new Interval1D(1, 1));

    [Puzzle(answer: 14)]
    public int Part1Example() => Antinodes(InputExample, new Interval1D(1, 1));

    [Puzzle(answer: 766)]
    public int Part2() => Antinodes(Input, new Interval1D(0, int.MaxValue));

    [Puzzle(answer: 34)]
    public int Part2Example() => Antinodes(InputExample, new Interval1D(0, int.MaxValue));

    private int Antinodes(string[] lines, Interval1D interval)
    {
        var grid = lines.ToGrid();
        var grouped = grid.EnumerableWithIndex()
            .GroupBy(x => x.Value)
            .Where(x => x.Key != '.');
        foreach (var group in grouped)
        {
            var arr = group.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    var diff = arr[j].Index - arr[i].Index;
                    foreach (int k in interval)
                        if (!grid.UpdateAt(arr[j].Index + k * diff, '#'))
                            break;
                    foreach (int k in interval)
                        if (!grid.UpdateAt(arr[i].Index - k * diff, '#'))
                            break;
                }
            }
        }
        grid.Print();
        return grid.Enumerable().Count(x => x == '#');
    }
};