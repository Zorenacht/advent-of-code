using System.Collections;

namespace Tools.Geometry;

public class Grid<T> : IEnumerable, IEnumerable<T> where T : struct
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
    
    public T[] As1D()
    {
        return Enumerable().ToArray();
    }
    
    public void Reset()
    {
        foreach (var (index, val) in EnumerableWithIndex())
        {
            UpdateAt(index, default);
        }
    }
    
    public bool IsValid(int i, int j)
    {
        return i >= 0 && i < Lattice.Length
                      && j >= 0 && j < Lattice[0].Length;
    }
    
    public bool IsValid(Index2D point)
    {
        return point.Row >= 0 && point.Row < Lattice.Length
                            && point.Col >= 0 && point.Col < Lattice[0].Length;
    }
    
    public T? Value(int row, int col)
    {
        if (IsValid(row, col))
            return Lattice[row][col];
        return null;
    }
    
    public T Value(Index2D point)
    {
        if (IsValid(point.Row, point.Col))
            return Lattice[point.Row][point.Col];
        return new T();
    }
    
    public T? ValueOrDefault(Index2D point)
    {
        if (IsValid(point.Row, point.Col))
            return Lattice[point.Row][point.Col];
        return null;
    }
    
    public T? ValueOrDefault(int row, int col)
    {
        if (IsValid(row, col))
            return Lattice[row][col];
        return null;
    }
    
    public bool UpdateAt(int row, int col, T value)
    {
        if (IsValid(row, col))
            Lattice[row][col] = value;
        else
        {
            throw new IndexOutOfRangeException();
        }
        return IsValid(row, col);
    }
    
    public bool UpdateAt(Index2D point, T value)
    {
        if (IsValid(point))
            Lattice[point.Row][point.Col] = value;
        return IsValid(point);
    }
    
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
        foreach (var (point, val) in grid1.EnumerableWithIndex())
        {
            if (!val.Equals(grid2.Value(point))) return false;
        }
        return true;
    }
    
    public static bool operator !=(Grid<T> grid1, Grid<T> grid2) => !(grid1 == grid2);
}