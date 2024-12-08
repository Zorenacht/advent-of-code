using System.Collections;

namespace Tools.Geometry;

public partial class Grid<T> : IEnumerable, IEnumerable<T> where T : struct
{
    public List<HashSet<Index2D>> FloorFill()
    {
        return [];
    }
}