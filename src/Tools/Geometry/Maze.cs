namespace Tools.Geometry;

public class Maze : CharGrid
{
    public char Wall { get; set; }
    
    public Maze(char[][] lines) : base(lines)
    {
    }
    
    public Maze(int row, int col) : base(row, col)
    {
    }
    
    private record DijkstraNode(IndexDirection Current, IndexDirection? Prev);
    
    private class DijkstraInfo(int distance, HashSet<IndexDirection> previous)
    {
        public int Distance { get; set; } = distance;
        public HashSet<IndexDirection> Previous { get; set; } = previous;
    }
    
    public int ShortestPath(IndexDirection start, Index2D end, Func<IndexDirection, IndexDirection> func)
    {
        var pq = new PriorityQueue<DijkstraNode, int>();
        pq.Enqueue(new DijkstraNode(start, null), 0);
        var nodes = new Dictionary<IndexDirection, DijkstraInfo>();
        while (pq.TryDequeue(out var dnode, out var distance))
        {
            var currentIndex = dnode.Current;
            if (nodes.TryGetValue(currentIndex, out var dijkstraInfo))
            {
                if (dijkstraInfo.Distance < distance)
                {
                    if (currentIndex.Index == end) break;
                    continue;
                }
                if (dijkstraInfo.Distance == distance && dnode.Prev is { })
                {
                    dijkstraInfo.Previous.Add(dnode.Prev);
                }
                if (dijkstraInfo.Distance > distance)
                {
                    dijkstraInfo.Distance = distance;
                    dijkstraInfo.Previous = dnode.Prev is { } ? [dnode.Prev] : [];
                }
            }
            else
            {
                nodes.Add(currentIndex, new DijkstraInfo(distance, dnode.Prev is { } ? [dnode.Prev] : []));
            }
            
            var straight = new IndexDirection(currentIndex.Index + currentIndex.Direction, currentIndex.Direction);
            var left = new IndexDirection(currentIndex.Index, currentIndex.Direction.Left());
            var right = new IndexDirection(currentIndex.Index, currentIndex.Direction.Right());
            if ("E.".Contains(ValueAt(straight.Index))) pq.Enqueue(new DijkstraNode(straight, currentIndex), distance + 1);
            pq.Enqueue(new DijkstraNode(left, currentIndex), distance + 1000);
            pq.Enqueue(new DijkstraNode(right, currentIndex), distance + 1000);
        }
        
        return nodes.Where(x => x.Key.Index == end)
            .MinBy(kv => kv.Value.Distance)
            .Value.Distance;
    }
}