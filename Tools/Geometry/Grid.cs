using System.Collections;

namespace Tools.Geometry;

public class Grid : IEnumerable
{
    public int[][] Lattice { get; set; }

    public int TotalLength { get { return RowLength * ColumnLength; } }
    public int RowLength { get { return Lattice.Length; } }
    public int ColumnLength { get { return Lattice[0].Length; } }


    public Grid(string text)
    {
        string[] lines = text.Split(Environment.NewLine);
        Lattice = new int[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            Lattice[i] = new int[lines[i].Length];
            for (int j = 0; j < lines[i].Length; j++)
            {
                Lattice[i][j] = Convert.ToInt32(lines[i][j]) - '0';
            }
        }
    }

    public Grid(int row, int col)
    {
        Lattice = new int[row][];
        for(int i=0; i<row; i++)
            Lattice[i] = new int[col];
    }

    public int[] As1D()
    {
        return Enumerable().ToArray();
    }
    public void Reset()
    {
        foreach(var (index, val) in EnumerableWithIndex())
        {
            UpdateAt(index, 0);
        }
    }

    public bool IsValid(int i, int j)
    {
        return i >= 0 && i < Lattice.Length
            && j >= 0 && j < Lattice.Length;
    }
    public bool IsValid(Point point)
    {
        return point.X >= 0 && point.X < Lattice.Length
            && point.Y >= 0 && point.Y < Lattice.Length;
    }

    public int ValueAt(int row, int col)
    {
        if (IsValid(row, col))
            return Lattice[row][col];
        return -100;
    }
    public int ValueAt(Point point)
    {
        if (IsValid(point.X, point.Y))
            return Lattice[point.X][point.Y];//maybe confusing that X is row
        return -100;
    }

    public bool UpdateAt(int row, int col, int value)
    {
        if (IsValid(row, col))
            Lattice[row][col] = value;
        return IsValid(row, col);
    }
    public bool UpdateAt(Point point, int value)
    {
        if (IsValid(point))
            Lattice[point.X][point.Y] = value;
        return IsValid(point);
    }

    public void ApplyToEach(Action<int,int> func)
    {
        for (int i = 0; i < Lattice.Length; i++)
        {
            for (int j = 0; j < Lattice[i].Length; j++)
            {
                func(i,j);
            }
        }
    }

    public virtual void Print()
    {
        Console.WriteLine(new string('-', 2*Lattice[0].Length));
        foreach(int[] row in Lattice)
        {
            foreach(int element in row)
            {
                Console.Write(element + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine(new string('-', 2*Lattice[0].Length));
    }

    public IEnumerator GetEnumerator()
    {
        for (int i = 0; i < Lattice.Length; i++)
        {
            for (int j = 0; j < Lattice[i].Length; j++)
            {
                yield return Lattice[i][j];
            }
        }
    }
    public IEnumerable<int> Enumerable()
    {
        for (int i = 0; i < Lattice.Length; i++)
        {
            for (int j = 0; j < Lattice[i].Length; j++)
            {
                yield return Lattice[i][j];
            }
        }
    }

    public IEnumerable<(Point, int)> EnumerableWithIndex()
    {
        for (int i = 0; i < Lattice.Length; i++)
        {
            for (int j = 0; j < Lattice[i].Length; j++)
            {
                yield return (new Point(i, j), Lattice[i][j]);
            }
        }
    }
}
