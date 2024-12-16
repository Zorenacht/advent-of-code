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
        long result = 0;
        var grid = Input.ToCharGrid();
        var current = grid.FindIndexes('S').First();
        var end = grid.FindIndexes('E').First();
        var pq = new PriorityQueue<IndexDirection, int>();
        pq.Enqueue(new(current, Direction.E), 0);
        var hs = new HashSet<IndexDirection>();
        while (pq.TryDequeue(out var indexDir, out var priority))
        {
            if (hs.Contains(indexDir)) continue;
            if (indexDir.Index == end)
                return priority;
            hs.Add(indexDir);

            var straight = new IndexDirection(indexDir.Index + indexDir.Direction, indexDir.Direction);
            var left = new IndexDirection(indexDir.Index, indexDir.Direction.Left());
            var right = new IndexDirection(indexDir.Index, indexDir.Direction.Right());
            if ("E.".Contains(grid.ValueAt(straight.Index)))
                pq.Enqueue(straight, priority + 1);
            pq.Enqueue(left, priority + 1000);
            pq.Enqueue(right, priority + 1000);
            //var left = new IndexDirection(indexDir.Index + indexDir.Direction.Left(), indexDir.Direction.Left());
            //var right = new IndexDirection(indexDir.Index + indexDir.Direction.Right(), indexDir.Direction.Right());
            //if ("E.".Contains(grid.ValueAt(straight.Index))) 
            //    pq.Enqueue(straight, priority + 1);
            //if ("E.".Contains(grid.ValueAt(left.Index))) 
            //    pq.Enqueue(left, priority + 1001);
            //if ("E.".Contains(grid.ValueAt(right.Index))) 
            //    pq.Enqueue(right, priority + 1001);
        }
        return result;
    }

    public class DijkstraNode(IndexDirection Id, int Value, Index2D Prev)
    {
        public IndexDirection Id { get; } = Id;
        public int Value { get; } = Value;
        public Index2D Prev { get; } = Prev;

        public override bool Equals(object? obj) => obj is DijkstraNode node && Id == node.Id;
        public override int GetHashCode() => Id.GetHashCode();
    };

    public record DijkstraThing(int Value, List<IndexDirection> Previous);

    [Puzzle(answer: null)]
    public long Part2()
    {
        long result = 0;
        var grid = Input.ToCharGrid();
        var current = grid.FindIndexes('S').First();
        var end = grid.FindIndexes('E').First();
        var pq = new PriorityQueue<DijkstraNode, int>();
        pq.Enqueue(new(new(current, Direction.E), 0, Index2D.O), 0);
        var hs = new Dictionary<IndexDirection, DijkstraThing>();
        while (pq.TryDequeue(out var dnode, out var priority))
        {
            var indexDir = dnode.Id;
            if (hs.ContainsKey(indexDir)) continue;
            if (indexDir.Index == end)
                return priority;
            hs.Add(indexDir, [dnode.Prev]);

            var straight = new IndexDirection(indexDir.Index + indexDir.Direction, indexDir.Direction);
            var left = new IndexDirection(indexDir.Index, indexDir.Direction.Left());
            var right = new IndexDirection(indexDir.Index, indexDir.Direction.Right());
            if ("E.".Contains(grid.ValueAt(straight.Index)))
                pq.Enqueue(new(straight, priority + 1, indexDir.Index), priority + 1);
            pq.Enqueue(new(left, priority + 1000, indexDir.Index), priority + 1000);
            pq.Enqueue(new(right, priority + 1000, indexDir.Index), priority + 1000);
        }
        return result;
    }





    public class State : IState<State>
    {
        public IndexDirection IndexDirection { get; init; }
        public CharGrid Grid { get; init; }

        public bool Equals(State? other)
        {
            return IndexDirection.Index == other!.IndexDirection.Index;
        }

        public override int GetHashCode()
        {
            return IndexDirection.Index.GetHashCode();
        }

        public int Heuristic()
        {
            return 0;
        }

        public IEnumerable<Node<State>> NextNodes(int initialDistance)
        {
            var straight = new IndexDirection(IndexDirection.Index + IndexDirection.Direction, IndexDirection.Direction);
            var left = new IndexDirection(IndexDirection.Index + IndexDirection.Direction.Left(), IndexDirection.Direction.Left());
            var right = new IndexDirection(IndexDirection.Index + IndexDirection.Direction.Right(), IndexDirection.Direction.Right());
            if ("E.".Contains(Grid.ValueAt(straight.Index))) yield return new Node<State>(new() { IndexDirection = straight, Grid = Grid }, initialDistance + 1);
            if ("E.".Contains(Grid.ValueAt(left.Index))) yield return new Node<State>(new() { IndexDirection = left, Grid = Grid }, initialDistance + 1000);
            if ("E.".Contains(Grid.ValueAt(right.Index))) yield return new Node<State>(new() { IndexDirection = right, Grid = Grid }, initialDistance + 1000);
        }
    }
};