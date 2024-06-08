using Tools.Geometry;

namespace AoC_2022;

public sealed partial class Day08 : Day
{
    [Test]
    public void Example()
    {
        var result = 0;
        Trees(InputExample, out Grid grid, out Grid counter);
        foreach (var count in counter.Enumerable())
        {
            result += count;
        }
        result.Should().Be(21);
    }

    [Test]
    public void ExamplePart2()
    {
        Trees(InputExample, out Grid grid, out Grid counter);
        TreeView2(grid, out Grid left, out Grid right, out Grid up, out Grid down);
        int max = 0;
        for (int i = 0; i < grid.RowLength; i++)
        {
            for (int j = 0; j < grid.ColumnLength; j++)
            {
                var prod = left.Lattice[i][j] * right.Lattice[i][j] * up.Lattice[i][j] * down.Lattice[i][j];
                if (prod > max)
                {
                    max = prod;
                }
            }
        }
        max.Should().Be(8);
    }

    [Test]
    public void Part1()
    {
        var result = 0;
        Trees(InputPart1, out Grid grid, out Grid counter);
        foreach (var count in counter.Enumerable())
        {
            result += count;
        }
        result.Should().Be(1708);
    }

    [Test]
    public void Part2()
    {
        Trees(InputPart2, out Grid grid, out Grid counter);
        TreeView2(grid, out Grid left, out Grid right, out Grid up, out Grid down);
        int max = 0;
        for (int i = 0; i < grid.RowLength; i++)
        {
            for (int j = 0; j < grid.ColumnLength; j++)
            {
                var prod = left.Lattice[i][j] * right.Lattice[i][j] * up.Lattice[i][j] * down.Lattice[i][j];
                if (prod > max)
                {
                    max = prod;
                }
            }
        }
        max.Should().Be(504000);
    }

    private void Trees(string[] input, out Grid grid, out Grid counter)
    {
        grid = new Grid(input);
        counter = new Grid(grid.RowLength, grid.ColumnLength);
        var rowMin = Enumerable.Repeat(-1, grid.RowLength).ToArray();
        var rowMinRev = Enumerable.Repeat(-1, grid.RowLength).ToArray();
        var colMin = Enumerable.Repeat(-1, grid.ColumnLength).ToArray();
        var colMinRev = Enumerable.Repeat(-1, grid.ColumnLength).ToArray();
        for (int i = 0; i < grid.RowLength; i++)
        {
            for (int j = 0; j < grid.ColumnLength; j++)
            {
                int iReverse = grid.RowLength - i - 1;
                int jReverse = grid.ColumnLength - j - 1;
                if (rowMin[i] < grid.Lattice[i][j])
                {
                    counter.Lattice[i][j] = 1;
                    rowMin[i] = grid.Lattice[i][j];
                }
                if (rowMinRev[i] < grid.Lattice[i][jReverse])
                {
                    counter.Lattice[i][jReverse] = 1;
                    rowMinRev[i] = grid.Lattice[i][jReverse];
                }
                if (colMin[j] < grid.Lattice[i][j])
                {
                    counter.Lattice[i][j] = 1;
                    colMin[j] = grid.Lattice[i][j];
                }
                if (colMinRev[j] < grid.Lattice[iReverse][j])
                {
                    counter.Lattice[iReverse][j] = 1;
                    colMinRev[j] = grid.Lattice[iReverse][j];
                }
            }
        }
    }

    //nlog(n) (n=total input digits)
    private void TreeView2(Grid grid, out Grid left, out Grid right, out Grid up, out Grid down)
    {
        left = new Grid(grid.RowLength, grid.ColumnLength);
        right = new Grid(grid.RowLength, grid.ColumnLength);
        up = new Grid(grid.RowLength, grid.ColumnLength);
        down = new Grid(grid.RowLength, grid.ColumnLength);
        for (int i = 0; i < grid.RowLength; i++)
        {
            for (int j = 0; j < grid.ColumnLength; j++)
            {
                var counter = new int[4];
                for (int k = i - 1; k >= 0; k--)
                {
                    counter[0]++;
                    if (grid.Lattice[i][j] <= grid.Lattice[k][j])
                    {
                        break;
                    }
                }
                for (int k = i + 1; k < grid.RowLength; k++)
                {
                    counter[1]++;
                    if (grid.Lattice[i][j] <= grid.Lattice[k][j])
                    {
                        break;
                    }
                }
                for (int k = j - 1; k >= 0; k--)
                {
                    counter[2]++;
                    if (grid.Lattice[i][j] <= grid.Lattice[i][k])
                    {
                        break;
                    }
                }
                for (int k = j + 1; k < grid.ColumnLength; k++)
                {
                    counter[3]++;
                    if (grid.Lattice[i][j] <= grid.Lattice[i][k])
                    {
                        break;
                    }
                }
                up.Lattice[i][j] = counter[0];
                down.Lattice[i][j] = counter[1];
                left.Lattice[i][j] = counter[2];
                right.Lattice[i][j] = counter[3];
            }
        }
        grid.Print();
        left.Print();
        right.Print();
        up.Print();
        down.Print();
    }
}