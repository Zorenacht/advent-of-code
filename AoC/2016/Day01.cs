using Tools.Shapes;

namespace AoC_2016;

public sealed class Day01 : Day
{
    [Puzzle(287)]
    public int Part1()
    {
        var parse = Input[0]
            .Split(", ")
            .Select(x => (x[0], int.Parse(x[1..].ToString())))
            .ToList();
        int up = 0;
        int right = 0;
        int dir = 0;
        var lines = new List<Line>();
        foreach (var ele in parse)
        {
            dir = ele.Item1 == 'R' ? dir + 1 : dir + 3;
            if (dir % 4 == 0) up += ele.Item2;
            if (dir % 4 == 1) right += ele.Item2;
            if (dir % 4 == 2) up -= ele.Item2;
            if (dir % 4 == 3) right -= ele.Item2;
        }
        return Math.Abs(up) + Math.Abs(right);
    }

    [Puzzle(133)]
    public int Part2()
    {
        var parse = Input[0]
            .Split(", ")
            .Select(x => (x[0], int.Parse(x[1..].ToString())))
            .ToList();
        int up = 0;
        int right = 0;
        int dir = 0;
        var lines = new List<StraightLine>();
        foreach (var ele in parse)
        {
            dir = ele.Item1 == 'R' ? dir + 1 : dir + 3;
            var start = new Point(up, right);
            if (dir % 4 == 0) up += ele.Item2;
            if (dir % 4 == 1) right += ele.Item2;
            if (dir % 4 == 2) up -= ele.Item2;
            if (dir % 4 == 3) right -= ele.Item2;
            var end = new Point(up, right);
            var newLine = new StraightLine(start, end);
            if (lines.Take(lines.Count - 1).Select(line => line.Intersection(newLine)).FirstOrDefault(x => x is not null) is { } intersection)
            {
                return Math.Abs(intersection.Row) + Math.Abs(intersection.Col);
            }
            lines.Add(new StraightLine(start, end));
        }
        throw new Exception("No answer found");
    }
}