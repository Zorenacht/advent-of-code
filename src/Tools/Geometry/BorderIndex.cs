namespace Tools.Geometry;

public readonly struct BorderIndex(Index2D index, Index2D direction)
{
    public Index2D Index { get; } = index;
    public Index2D Direction { get; } = direction;
    public Index2D Of { get; } = index - direction;
    public DoubleIndex2D HalfIndex { get; } = new DoubleIndex2D(index.Row - 0.5 * direction.Row, index.Col - 0.5 * direction.Col);
}