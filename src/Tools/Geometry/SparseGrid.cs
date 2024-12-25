using Collections;
using System.Diagnostics.CodeAnalysis;

namespace Tools.Geometry;

[SuppressMessage("ReSharper", "InvertIf")]
public class SparseGrid
{
    public Dictionary<int, HashSet<int>> WallCols { get; }

    public Dictionary<int, HashSet<int>> WallRows { get; }

    public SparseGrid(CharGrid grid, char wall)
    {
        WallRows = new Dictionary<int, HashSet<int>>();
        WallCols = new Dictionary<int, HashSet<int>>();
        foreach ((var index, char value) in grid.EnumerableWithIndex())
        {
            if (value != wall) continue;
            if (WallRows.TryGetValue(index.Row, out var cols)) cols.Add(index.Col);
            else WallRows.Add(index.Row, [index.Col]);
            if (WallCols.TryGetValue(index.Col, out var rows)) rows.Add(index.Row);
            else WallCols.Add(index.Col, [index.Row]);
        }
    }

    public IndexDirectionV2 NextBlockade(IndexDirectionV2 indexDirection)
    {
        if (indexDirection.Direction == Index2D.N)
        {
            if (!WallCols.TryGetValue(indexDirection.Index.Col, out var rows)) return IndexDirectionV2.OutOfBounds;
            return indexDirection with
            {
                Index = new Index2D(
                    rows.Where(x => x < indexDirection.Index.Row)
                        .MaxOrNull() ?? int.MinValue,
                    indexDirection.Index.Col)
            };
        }
        if (indexDirection.Direction == Index2D.S)
        {
            if (!WallCols.TryGetValue(indexDirection.Index.Col, out var rows)) return IndexDirectionV2.OutOfBounds;
            return indexDirection with
            {
                Index = new Index2D(
                    rows.Where(x => x > indexDirection.Index.Row)
                        .MinOrNull() ?? int.MaxValue,
                    indexDirection.Index.Col)
            };
        }
        if (indexDirection.Direction == Index2D.W)
        {
            if (!WallRows.TryGetValue(indexDirection.Index.Row, out var cols)) return IndexDirectionV2.OutOfBounds;
            return indexDirection with
            {
                Index = new Index2D(
                    indexDirection.Index.Row,
                    cols.Where(x => x < indexDirection.Index.Col)
                        .MaxOrNull() ?? int.MinValue)
            };
        }
        if (indexDirection.Direction == Index2D.E)
        {
            if (!WallRows.TryGetValue(indexDirection.Index.Row, out var cols)) return IndexDirectionV2.OutOfBounds;
            return indexDirection with
            {
                Index = new Index2D(
                    indexDirection.Index.Row,
                    cols.Where(x => x > indexDirection.Index.Col)
                        .MinOrNull() ?? int.MaxValue)
            };
        }
        throw new NotSupportedException("Direction should be one of N,W,S,E");
    }
}