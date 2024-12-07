using System.Collections;

namespace Tools.Geometry;

public interface IGrid<T> : IEnumerable, IEnumerable<T> where T : struct
{
    public T[][] Lattice { get; set; }
    public int TotalLength { get; }
    public int RowLength { get; }
    public int ColLength { get; }
    public T[] this[int row] { get; set; }
    public T[] As1D();
    public void Reset();
    public bool IsValid(int i, int j);
    public bool IsValid(Index2D point);
    public T? ValueOrDefault(int row, int col);
    public T? ValueOrDefault(Index2D point) => ValueOrDefault(point.Row, point.Col);
    public bool UpdateAt(int row, int col, T value);
    public bool UpdateAt(Index2D point, T value);
    public void ApplyRange(Index2D from, Index2D to, T value);
    public IEnumerable<Index2D> FindIndexes(T value);
    public IEnumerable<T> Enumerable();
    public IEnumerable<(Index2D Index, T Value)> EnumerableWithIndex();
    public IEnumerable<(Index2D, T)> Enumerable(Func<Index2D, bool> condition);
}