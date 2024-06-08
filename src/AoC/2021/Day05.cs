namespace AoC_2021;

public sealed class Day05 : Day
{
    [Puzzle(answer: 19081)]
    public int Part2()
    {
        const int max_size = 1000;

        int[,] plane = new int[max_size, max_size];
        List<Line> lines = new List<Line>();
        parseInput(lines);
        apply(lines, plane);

        int countGT2 = 0;
        foreach (var entry in plane)
        {
            if (entry >= 2)
                countGT2++;
        }
        return countGT2;

        void parseInput(List<Line> lines)
        {
            foreach (string line in Input)
            {
                string[] coordinates = line.Split(" -> ");
                string[] start = coordinates[0].Split(',');
                string[] end = coordinates[1].Split(',');

                lines.Add(new Line
                {
                    Start = new Tuple<int, int>(int.Parse(start[0]), int.Parse(start[1])),
                    End = new Tuple<int, int>(int.Parse(end[0]), int.Parse(end[1])),
                });
            }
        }

        void apply(List<Line> lines, int[,] plane)
        {
            foreach (var line in lines)
            {
                int currentX = line.Start.Item1;
                int currentY = line.Start.Item2;
                int endX = line.End.Item1;
                int endY = line.End.Item2;
                while (currentX != line.End.Item1 || currentY != line.End.Item2)
                {
                    plane[currentX, currentY] += 1;
                    currentX += Math.Sign(endX - currentX);
                    currentY += Math.Sign(endY - currentY);
                }
                plane[endX, endY] += 1;
            }
        }
    }

    public record Line
    {
        public Tuple<int, int> Start { get; set; } = new Tuple<int, int>(0, 0);
        public Tuple<int, int> End { get; set; } = new Tuple<int, int>(0, 0);
    }
}