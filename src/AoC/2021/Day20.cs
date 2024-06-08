using System.Text;
using Tools.Geometry;

namespace AoC_2021;

public sealed class Day20 : Day
{
    [Puzzle(answer: 35)]
    public int Part1_Example()
    {
        init(InputExample, out string coding, out Grid grid);
        grid.Print();
        Console.WriteLine("Iteration: " + 0 + " Count: " + grid.Count());
        for (int i = 0; i < 2; i++)
        {
            grid = run(grid, coding, i);
        }
        Console.WriteLine("Iteration: " + 2 + " Count: " + grid.Count());
        return grid.Count();
    }

    [Puzzle(answer: 5479)]
    public int Part1()
    {
        init(Input, out string coding, out Grid grid);
        grid.Print();
        Console.WriteLine("Iteration: " + 0 + " Count: " + grid.Count());
        for (int i = 0; i < 2; i++)
        {
            grid = run(grid, coding, i);
        }
        Console.WriteLine("Iteration: " + 2 + " Count: " + grid.Count());
        return grid.Count();
    }

    [Puzzle(answer: 19012)]
    public int Part2()
    {
        init(Input, out string coding, out Grid grid);
        grid.Print();
        Console.WriteLine("Iteration: " + 0 + " Count: " + grid.Count());
        for (int i = 0; i < 50; i++)
        {
            grid = run(grid, coding, i);
        }
        Console.WriteLine("Iteration: " + 50 + " Count: " + grid.Count());
        return grid.Count();
    }

    Grid run(Grid grid, string coding, int iteration)
    {
        bool border = borderVal(coding, iteration);
        grid.ExtendBorder(border);

        grid.Print();
        Console.WriteLine("Iteration: " + iteration + " Count: " + grid.Count());

        Grid nextIteration = new Grid(grid.RowLength, grid.ColLength);
        for (int i = 0; i < grid.RowLength; i++)
        {
            for (int j = 0; j < grid.ColLength; j++)
            {
                nextIteration.Rows[i][j] = findCoding(grid, coding, i, j, border);
            }
        }
        nextIteration.Print();
        return nextIteration;
    }

    void init(string[] input, out string coding, out Grid grid)
    {
        List<string> lines = input.ToList();
        coding = lines[0];

        grid = new Grid();
        foreach (var line in lines.GetRange(2, lines.Count - 2))
        {
            grid.Rows.Add(Parser(line));
        }
    }


    bool borderVal(string coding, int iteration)
    {
        if (coding[0] == '.') return false;
        if (coding[^1] == '.') return iteration % 2 == 1;
        return iteration > 0;
    }

    bool findCoding(Grid grid, string coding, int i, int j, bool border)
    {
        Point point = new Point(i, j);
        int code = 0;
        code = 2 * code + (grid.Value(point.Neighbor2(Direction.NW), border) ? 1 : 0);
        code = 2 * code + (grid.Value(point.Neighbor2(Direction.N), border) ? 1 : 0);
        code = 2 * code + (grid.Value(point.Neighbor2(Direction.NE), border) ? 1 : 0);
        code = 2 * code + (grid.Value(point.Neighbor2(Direction.W), border) ? 1 : 0);
        code = 2 * code + (grid.Value(point, border) ? 1 : 0);
        code = 2 * code + (grid.Value(point.Neighbor2(Direction.E), border) ? 1 : 0);
        code = 2 * code + (grid.Value(point.Neighbor2(Direction.SW), border) ? 1 : 0);
        code = 2 * code + (grid.Value(point.Neighbor2(Direction.S), border) ? 1 : 0);
        code = 2 * code + (grid.Value(point.Neighbor2(Direction.SE), border) ? 1 : 0);
        return coding[code] == '#' ? true : false;
    }

    List<bool> Parser(string input)
    {
        List<bool> row = new List<bool>();
        foreach (char c in input)
        {
            if (c == '#')
                row.Add(true);
            else if (c == '.')
                row.Add(false);
        }
        return row;
    }

    public class Grid
    {
        public List<List<bool>> Rows = new();

        public int RowLength => Rows.Count;
        public int ColLength => Rows.FirstOrDefault()?.Count ?? 0;

        public Grid()
        {

        }

        public Grid(int row, int col)
        {
            for (int i = 0; i < row; i++)
            {
                Rows.Add(new List<bool>(new bool[col]));
            }
        }

        public void Print()
        {
            Console.WriteLine(new string('-', ColLength));
            StringBuilder sb = new StringBuilder();
            foreach (var row in Rows)
            {
                foreach (var val in row)
                {
                    sb = sb.Append(val ? "#" : ".");
                }
                sb = sb.Append('\n');
            }
            Console.WriteLine(sb);
        }

        public bool Value(Point point, bool border)
        {
            return point.X >= 0 && point.Y >= 0 && point.X < RowLength && point.Y < ColLength
                ? Rows[point.X][point.Y]
                : border;
        }

        public int Count()
        {
            int count = 0;
            foreach (var row in Rows)
            {
                foreach (var val in row)
                {
                    if (val) count++;
                }
            }
            return count;
        }

        public void ExtendBorder(bool border)
        {
            Rows.Insert(0, new List<bool>(Enumerable.Repeat(border, ColLength).ToArray()));
            Rows.Add(new List<bool>(Enumerable.Repeat(border, ColLength).ToArray()));
            for (int i = 0; i < RowLength; i++)
            {
                Rows[i].Insert(0, border);
                Rows[i].Add(border);
            }
        }
    }
}