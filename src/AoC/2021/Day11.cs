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
            var hasFlashed = new Grid<int>(grid.RowLength, grid.ColLength);
            foreach (var (index, val) in grid.EnumerableWithIndex())
            {
                grid.UpdateAt(index, val == 9 ? 0 : val + 1);
            }
            foreach (var (index, val) in grid.EnumerableWithIndex())
            {
                if (grid.ValueAt(index) == 0 && hasFlashed.Lattice[index.Row][index.Col] == 0)
                {
                    giveNeighborsEnergy(index, grid, hasFlashed, ref count, i, withAnimation);
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
        var hasFlashed = new Grid<int>(grid.RowLength, grid.ColLength);
        int countIterations = 0;
        int count = 0;//needed so that function runs

        while (hasFlashed.As1D().Where(x => x == 1).Count() != hasFlashed.RowLength * hasFlashed.ColLength)
        {
            hasFlashed.Reset();
            foreach (var (index, val) in grid.EnumerableWithIndex())
            {
                grid.UpdateAt(index, val == 9 ? 0 : val + 1);
            }
            foreach (var (index, val) in grid.EnumerableWithIndex())
            {
                if (grid.ValueAt(index) == 0 && hasFlashed.Lattice[index.Row][index.Col] == 0)
                {
                    giveNeighborsEnergy(index, grid, hasFlashed, ref count, countIterations, withAnimation);
                }
            }
            countIterations++;
        }
        print(grid, countIterations, count);
        return countIterations;
    }

    void giveNeighborsEnergy(Index2D index, FlashingOctopusGrid grid, Grid<int> hasFlashed, ref int count, int iteration, bool withAnimation)
    {
        if (grid.ValueAt(index) != 0)
        {
            return;
        }
        hasFlashed.UpdateAt(index, 1);
        count++;

        for (int j = 0; j < 8; j++)
        {
            var neighbor = index.Neighbor((Direction)j);
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

    internal class FlashingOctopusGrid : Grid<int>
    {
        public FlashingOctopusGrid(string[] text) : base(
            text
                .Select(x => x
                    .Select(x => x - '0')
                    .ToArray())
                .ToArray())
        { }

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