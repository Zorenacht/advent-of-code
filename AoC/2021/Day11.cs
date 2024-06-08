using Tools.Geometry;

namespace AoC_2021;

public sealed class Day11(bool withAnimation = false) : Day
{
    public Day11() : this(false) { }

    [Puzzle(answer: 1625)]
    public int Part1()
    {
        if (withAnimation) Console.Clear();

        var grid = new FlashingOctopusGrid(Input);
        int count = 0;

        for (int i = 1; i <= 100; i++)
        {
            Grid hasFlashed = new Grid(grid.RowLength, grid.ColumnLength);
            foreach (var (point, val) in grid.EnumerableWithIndex())
            {
                grid.UpdateAt(point, val == 9 ? 0 : val + 1);
            }
            foreach (var (point, val) in grid.EnumerableWithIndex())
            {
                if (grid.ValueAt(point) == 0 && hasFlashed.Lattice[point.X][point.Y] == 0)
                {
                    giveNeighborsEnergy(point, grid, hasFlashed, ref count, i, withAnimation);
                }
            }
        }
        print(grid, 100, count);
        return count;
    }

    [Puzzle(answer: 244)]
    public int Part2()
    {
        if (withAnimation) Console.Clear();

        var grid = new FlashingOctopusGrid(Input);
        Grid hasFlashed = new Grid(grid.RowLength, grid.ColumnLength);
        int countIterations = 0;
        int count = 0;//needed so that function runs

        while (hasFlashed.As1D().Where(x => x == 1).Count() != hasFlashed.ColumnLength * hasFlashed.RowLength)
        {
            hasFlashed.Reset();
            foreach (var (point, val) in grid.EnumerableWithIndex())
            {
                grid.UpdateAt(point, val == 9 ? 0 : val + 1);
            }
            foreach (var (point, val) in grid.EnumerableWithIndex())
            {
                if (grid.ValueAt(point) == 0 && hasFlashed.Lattice[point.X][point.Y] == 0)
                {
                    giveNeighborsEnergy(point, grid, hasFlashed, ref count, countIterations, withAnimation);
                }
            }
            countIterations++;
        }
        print(grid, countIterations, count);
        return countIterations;
    }

    void giveNeighborsEnergy(Point point, FlashingOctopusGrid grid, Grid hasFlashed, ref int count, int iteration, bool withAnimation)
    {
        if (grid.ValueAt(point) != 0)
        {
            return;
        }
        hasFlashed.UpdateAt(point, 1);
        count++;

        for (int j = 0; j < 8; j++)
        {
            Point neighbor = point.Neighbor((Direction)j);
            if (grid.IsValid(neighbor))
            {
                int value = grid.ValueAt(neighbor);
                if (value != 0)
                {
                    if (value != 9)
                        grid.UpdateAt(neighbor, value + 1);
                    else
                    {
                        grid.UpdateAt(neighbor, 0);
                        if (withAnimation)
                            print(grid, iteration, count);
                        giveNeighborsEnergy(neighbor, grid, hasFlashed, ref count, iteration, withAnimation);
                    }
                }
            }
        }
    }

    void print(FlashingOctopusGrid grid, int step, int count, int delay = 10)
    {
        grid.Print(step, count, withAnimation);
        Thread.Sleep(delay);
    }

    internal class FlashingOctopusGrid : Grid
    {
        public FlashingOctopusGrid(string[] text) : base(text) { }

        public void Print(int step, int count, bool withAnimation = false)
        {
            if (withAnimation)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"After step {step}:");
            }
            foreach (int[] row in Lattice)
            {
                foreach (int element in row)
                {
                    if (element == 0)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(element + " ");
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Count: " + count);
        }
    }

}