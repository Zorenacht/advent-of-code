using Tools.Geometry;

namespace AoC_2022;

public sealed partial class Day08 : Day
{
    [Test]
    public void Example()
    {
        var result = 0;
        Trees(@"InputFiles\Day08-Example.txt", out Grid grid, out Grid counter);
        foreach (var count in counter.Enumerable())
        {
            result += count;
        }
        result.Should().Be(21);
    }

    [Test]
    public void ExamplePart2()
    {
        Trees(@"InputFiles\Day08-Example.txt", out Grid grid, out Grid counter);
        TreeView2(grid, out Grid left, out Grid right, out Grid up, out Grid down);
        int max = 0;
        for (int i = 0; i < grid.RowLength; i++)
        {
            for (int j = 0; j < grid.ColumnLength; j++)
            {
                var prod = left.Lattice[i][j] * right.Lattice[i][j] * up.Lattice[i][j] * down.Lattice[i][j];
                if(prod > max)
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
        Trees(@"InputFiles\Day08-1.txt", out Grid grid, out Grid counter);
        foreach (var count in counter.Enumerable())
        {
            result += count;
        }
        result.Should().Be(1708);
    }

    [Test]
    public void Part2()
    {
        Trees(@"InputFiles\Day08-1.txt", out Grid grid, out Grid counter);
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

    private void Trees(string fileName, out Grid grid, out Grid counter)
    {
        grid = new Grid(Reader.ReadAsText(fileName));
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

    private void TreeView(Grid grid,
        out Grid left, out Grid right, out Grid up, out Grid down)
    {
        left = new Grid(grid.RowLength, grid.ColumnLength);
        right = new Grid(grid.RowLength, grid.ColumnLength);
        up = new Grid(grid.RowLength, grid.ColumnLength);
        down = new Grid(grid.RowLength, grid.ColumnLength);
        var leftRecent = Enumerable.Repeat(-1, grid.RowLength).ToArray();
        var rightRecent = Enumerable.Repeat(-1, grid.RowLength).ToArray();
        var upRecent = Enumerable.Repeat(-1, grid.ColumnLength).ToArray();
        var downRecent = Enumerable.Repeat(-1, grid.ColumnLength).ToArray();
        for (int i = 0; i < grid.RowLength; i++)
        {
            for (int j = 0; j < grid.ColumnLength; j++)
            {
                if (j == 2 && i == 1)
                {
                    var a = 1;
                }
                int iReverse = grid.RowLength - i - 1;
                int jReverse = grid.ColumnLength - j - 1;

                if (leftRecent[i] == -1 || left.Lattice[i][leftRecent[i]] < left.Lattice[i][j])
                {
                    left.Lattice[i][j] = j;
                    leftRecent[i] = j;
                }
                else
                {
                    int sightCounter = 1;
                    for (int k = j - 1; k >= leftRecent[i]; k--)
                    {
                        if (grid.Lattice[i][j] <= grid.Lattice[i][k])
                        {
                            break;
                        }
                        sightCounter++;
                    }
                    left.Lattice[i][j] = sightCounter;
                }

                if (rightRecent[i] == -1 || right.Lattice[i][rightRecent[i]] < right.Lattice[i][jReverse])
                {
                    right.Lattice[i][jReverse] = j;
                    rightRecent[i] = jReverse;
                }
                else
                {
                    int sightCounter = 1;
                    for (int k = jReverse + 1; k <= rightRecent[i]; k++)
                    {
                        if (grid.Lattice[i][jReverse] <= grid.Lattice[i][k])
                        {
                            break;
                        }
                        sightCounter++;
                    }
                    right.Lattice[i][jReverse] = sightCounter;
                }


                if (upRecent[j] == -1 || up.Lattice[upRecent[j]][j] < up.Lattice[i][j])
                {
                    up.Lattice[i][j] = i;
                    upRecent[j] = i;
                }
                else
                {
                    int sightCounter = 1;
                    for (int k = i - 1; k >= upRecent[j]; k--)
                    {
                        if (grid.Lattice[i][j] <= grid.Lattice[k][j])
                        {
                            break;
                        }
                        sightCounter++;
                    }
                    up.Lattice[i][j] = sightCounter;
                }


                if (downRecent[j] == -1 || up.Lattice[downRecent[j]][j] < up.Lattice[iReverse][j])
                {
                    down.Lattice[iReverse][j] = i;
                    downRecent[j] = iReverse;
                }
                else
                {
                    int sightCounter = 1;
                    for (int k = iReverse + 1; k <= downRecent[j]; k++)
                    {
                        if (grid.Lattice[iReverse][j] <= grid.Lattice[k][j])
                        {
                            break;
                        }
                        sightCounter++;
                    }
                    down.Lattice[iReverse][j] = sightCounter;
                }
            }
        }
        grid.Print();
        left.Print();
        right.Print();
        up.Print();
        down.Print();
    }


    //nlog(n) (n=total input digits)
    private void TreeView2(Grid grid, out Grid left, out Grid right, out Grid up, out Grid down)
    {
        left = new Grid(grid.RowLength, grid.ColumnLength);
        right = new Grid(grid.RowLength, grid.ColumnLength);
        up = new Grid(grid.RowLength, grid.ColumnLength);
        down = new Grid(grid.RowLength, grid.ColumnLength);
        var view = new Grid(grid.RowLength, grid.ColumnLength);
        int max = 0;
        for (int i = 0; i < grid.RowLength; i++)
        {
            for (int j = 0; j < grid.ColumnLength; j++)
            {
                if (i == 0 && j == 2)
                {
                    var a = 1;
                }
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