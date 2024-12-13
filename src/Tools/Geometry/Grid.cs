using System.Collections;

namespace Tools.Geometry;

public partial class Grid<T> : IEnumerable, IEnumerable<T> where T : struct
{
    public T[][] Lattice { get; set; }
    
    public int TotalLength => RowLength * ColLength;
    public int RowLength => Lattice.Length;
    public int ColLength => Lattice[0].Length;
    
    public Grid(T[][] lines)
    {
        Lattice = new T[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            Lattice[i] = new T[lines[i].Length];
            for (int j = 0; j < lines[i].Length; j++)
            {
                Lattice[i][j] = lines[i][j];
            }
        }
    }
    
    public Grid(int row, int col)
    {
        Lattice = new T[row][];
        for (int i = 0; i < row; i++)
            Lattice[i] = new T[col];
    }
    
    public T[] this[int row]
    {
        get => Lattice[row];
        set => Lattice[row] = value;
    }
    
    public T[] As1D() => Enumerable().ToArray();
    
    public void Reset()
    {
        foreach (var (index, val) in EnumerableWithIndex())
        {
            UpdateAt(index, default);
        }
    }
    
    public bool IsValid(int i, int j) =>
        i >= 0 && i < Lattice.Length &&
        j >= 0 && j < Lattice[0].Length;
    
    public bool IsValid(Index2D index) =>
        index.Row >= 0 && index.Row < Lattice.Length &&
        index.Col >= 0 && index.Col < Lattice[0].Length;
    
    public T ValueAt(Index2D index) => this[index.Row][index.Col];
    
    public T? ValueOrDefault(int row, int col) => IsValid(row, col)
        ? Lattice[row][col]
        : null;
    
    public T? ValueOrDefault(Index2D index) => ValueOrDefault(index.Row, index.Col);
    
    public bool UpdateAt(int row, int col, T value)
    {
        if (!IsValid(row, col)) return false;
        Lattice[row][col] = value;
        return true;
    }
    
    public bool UpdateAt(Index2D index, T value) => UpdateAt(index.Row, index.Col, value);
    
    public Grid<T> Copy() => new(Lattice);
    
    public void ApplyRange(Index2D from, Index2D to, T value)
    {
        for (int i = Math.Min(from.Row, to.Row); i <= Math.Max(from.Row, to.Row); i++)
        {
            for (int j = Math.Min(from.Col, to.Col); j <= Math.Max(from.Col, to.Col); j++)
            {
                UpdateAt(i, j, value);
            }
        }
    }
    
    public IEnumerable<Index2D> FindIndexes(T value)
    {
        foreach (var (index, val) in EnumerableWithIndex())
        {
            if (val.Equals(value)) yield return index;
        }
    }
    
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        for (int i = 0; i < Lattice.Length; i++)
        {
            for (int j = 0; j < Lattice[i].Length; j++)
            {
                yield return Lattice[i][j];
            }
        }
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
    
    public IEnumerable<T> Enumerable()
    {
        for (int i = 0; i < Lattice.Length; i++)
        {
            for (int j = 0; j < Lattice[i].Length; j++)
            {
                yield return Lattice[i][j];
            }
        }
    }
    
    public IEnumerable<(Index2D Index, T Value)> EnumerableWithIndex()
    {
        for (int i = 0; i < Lattice.Length; i++)
        {
            for (int j = 0; j < Lattice[i].Length; j++)
            {
                yield return (new Index2D(i, j), Lattice[i][j]);
            }
        }
    }
    
    public IEnumerable<(Index2D, T)> Enumerable(Func<Index2D, bool> condition)
    {
        for (int i = 0; i < Lattice.Length; i++)
        {
            for (int j = 0; j < Lattice[i].Length; j++)
            {
                yield return (new Index2D(i, j), Lattice[i][j]);
            }
        }
    }
    
    public static bool operator ==(Grid<T> grid1, Grid<T> grid2)
    {
        if (grid1.ColLength != grid2.ColLength) return false;
        if (grid1.RowLength != grid2.RowLength) return false;
        foreach (var (index, val) in grid1.EnumerableWithIndex())
        {
            if (!val.Equals(grid2.ValueOrDefault(index))) return false;
        }
        return true;
    }
    
    public virtual void Print()
    {
        Console.WriteLine(new string('-', Lattice[0].Length));
        foreach (T[] row in Lattice)
        {
            foreach (T element in row)
            {
                Console.Write(element);
            }
            Console.WriteLine();
        }
        Console.WriteLine(new string('-', Lattice[0].Length));
    }
    
    public static bool operator !=(Grid<T> grid1, Grid<T> grid2) => !(grid1 == grid2);
    
    public override bool Equals(object? obj) => obj is Grid<T> grid && this == grid;
    
    public override int GetHashCode() => throw new NotSupportedException();
}