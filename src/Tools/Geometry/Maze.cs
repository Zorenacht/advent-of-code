using Collections;
using System.Collections;

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
    
    public struct IndexDirectionStruct(Index2D index, Direction direction)
    {
        public Index2D Index { get; } = index;
        public Direction Direction { get; } = direction;
    }
    
    private record DijkstraClassNode<T>(T Current, T? Previous) where T : class;
    
    private record DijkstraStructNode<T>(T Current, T? Previous) where T : struct;
    
    private class DijkstraInfo(int distance, HashSet<IndexDirection> previous)
    {
        public int Distance { get; set; } = distance;
        public HashSet<IndexDirection> Previous { get; set; } = previous;
    }
    
    private class DijkstraStructInfo<T>(int distance, HashSet<T> previous) where T : struct
    {
        public int Distance { get; set; } = distance;
        public HashSet<T> Previous { get; set; } = previous;
    }
    
    public int ShortestPath(IndexDirection start, Index2D end)
    {
        var pq = new PriorityQueue<DijkstraClassNode<IndexDirection>, int>();
        pq.Enqueue(new DijkstraClassNode<IndexDirection>(start, null), 0);
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
                if (dijkstraInfo.Distance == distance && dnode.Previous is { })
                {
                    dijkstraInfo.Previous.Add(dnode.Previous);
                }
                if (dijkstraInfo.Distance > distance)
                {
                    dijkstraInfo.Distance = distance;
                    dijkstraInfo.Previous = dnode.Previous is { } ? [dnode.Previous] : [];
                }
            }
            else
            {
                nodes.Add(currentIndex, new DijkstraInfo(distance, dnode.Previous is { } ? [dnode.Previous] : []));
            }
            
            var straight = new IndexDirection(currentIndex.Index + currentIndex.Direction, currentIndex.Direction);
            var left = new IndexDirection(currentIndex.Index, currentIndex.Direction.Left());
            var right = new IndexDirection(currentIndex.Index, currentIndex.Direction.Right());
            if (".".Contains(ValueAt(straight.Index)))
                pq.Enqueue(new DijkstraClassNode<IndexDirection>(straight, currentIndex), distance + 1);
            pq.Enqueue(new DijkstraClassNode<IndexDirection>(left, currentIndex), distance + 1000);
            pq.Enqueue(new DijkstraClassNode<IndexDirection>(right, currentIndex), distance + 1000);
        }
        
        return nodes.Where(x => x.Key.Index == end)
            .MinBy(kv => kv.Value.Distance)
            .Value.Distance;
    }
    
    public int ShortestPathV2(Index2D start, Index2D end)
    {
        var pq = new PriorityQueue<DijkstraStructNode<Index2D>, int>()
            .With(new DijkstraStructNode<Index2D>(start, null), 0);
        var nodes = new Dictionary<Index2D, DijkstraStructInfo<Index2D>>();
        var goalDistance = int.MaxValue;
        while (pq.TryDequeue(out var dnode, out var distance) && distance <= goalDistance)
        {
            var currentIndex = dnode.Current;
            if (nodes.TryGetValue(currentIndex, out var dijkstraInfo))
            {
                if (dijkstraInfo.Distance < distance) continue;
                if (dijkstraInfo.Distance == distance && dnode.Previous is { })
                {
                    if (dijkstraInfo.Previous.Add(dnode.Previous.Value)) continue;
                }
                else if (dijkstraInfo.Distance > distance)
                {
                    dijkstraInfo.Distance = distance;
                    dijkstraInfo.Previous = dnode.Previous is { } previous ? [previous] : [];
                    if (currentIndex == end) goalDistance = distance;
                }
            }
            else
            {
                nodes.Add(currentIndex, new DijkstraStructInfo<Index2D>(distance, dnode.Previous is { } ? [dnode.Previous.Value] : []));
                if (currentIndex == end) goalDistance = distance;
            }
            
            foreach (var nb in Directions.CardinalIndex
                         .Select(dir => currentIndex + dir)
                         .Where(nb => ValueOrDefault(nb) == '.'))
            {
                pq.Enqueue(new DijkstraStructNode<Index2D>(nb, currentIndex), distance + 1);
            }
        }
        return goalDistance != int.MaxValue
            ? goalDistance
            : -1;
    }
}

public class Path : IEnumerable<Index2D>
{
    public IEnumerator<Index2D> GetEnumerator() => throw new NotImplementedException();
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}