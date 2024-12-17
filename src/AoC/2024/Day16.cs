using FluentAssertions;
using ShortestPath;
using System.Linq;
using Tools.Geometry;
using static AoC._2021.Day20;

namespace AoC._2024;

public sealed class Day16 : Day
{
    [Puzzle(answer: 94444)]
    public long Part1()
    {
        var maze = Input.ToMaze();
        var start = maze.FindIndexes('S').First();
        var end = maze.FindIndexes('E').First();
        return maze.ShortestPath(new IndexDirection(start, Direction.E), end, null!);
    }
    
    private record DijkstraNode(IndexDirection Current, IndexDirection? Prev);
    
    private class DijkstraInfo(int distance, HashSet<IndexDirection> previous)
    {
        public int Distance { get; set; } = distance;
        public HashSet<IndexDirection> Previous { get; set; } = previous;
    }
    
    [Puzzle(answer: 502)]
    public long Part2()
    {
        var grid = Input.ToCharGrid();
        var start = grid.FindIndexes('S').First();
        var end = grid.FindIndexes('E').First();
        var pq = new PriorityQueue<DijkstraNode, int>();
        pq.Enqueue(new DijkstraNode(new IndexDirection(start, Direction.E), null), 0);
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
            if ("E.".Contains(grid.ValueAt(straight.Index))) pq.Enqueue(new DijkstraNode(straight, currentIndex), distance + 1);
            pq.Enqueue(new DijkstraNode(left, currentIndex), distance + 1000);
            pq.Enqueue(new DijkstraNode(right, currentIndex), distance + 1000);
        }
        var optimal = new HashSet<Index2D>();
        var queue = new Queue<IndexDirection>();
        var eastEnd = new IndexDirection(end, Direction.E);
        var northEnd = new IndexDirection(end, Direction.N);
        if (nodes[eastEnd].Distance <= nodes[northEnd].Distance) queue.Enqueue(new IndexDirection(end, Direction.E));
        if (nodes[eastEnd].Distance >= nodes[northEnd].Distance) queue.Enqueue(new IndexDirection(end, Direction.N));
        while (queue.TryDequeue(out var element))
        {
            grid.UpdateAt(element.Index, 'O');
            optimal.Add(element.Index);
            foreach (var prev in nodes[element].Previous)
            {
                queue.Enqueue(prev);
            }
        }
        grid.Print();
        return optimal.Count;
    }
};