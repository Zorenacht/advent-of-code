using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using Tools.Geometry;

namespace AoC._2024;

[SuppressMessage("ReSharper", "InlineTemporaryVariable")]
public sealed class Day18 : Day
{
    [Puzzle(answer: 22)]
    public long Part1_Example()
    {
        var maze = InitMaze(7, 7, 12, InputExample);
        return ShortestPath(Index2D.O, new Index2D(maze.RowLength - 1, maze.ColLength - 1), maze);
    }
    
    [Puzzle(answer: 336)]
    public long Part1()
    {
        var maze = InitMaze(71, 71, 1024, Input);
        return ShortestPath(Index2D.O, new Index2D(maze.RowLength - 1, maze.ColLength - 1), maze);
    }
    
    //2951
    [Puzzle(answer: "24,30")]
    public string Part2()
    {
        const int size = 71;
        var lines = Input;
        var start = Index2D.O;
        var end = new Index2D(size - 1, size - 1);
        var left = 0;
        var right = lines.Length - 1;
        while (left < right)
        {
            var mid = (left + right + 1) / 2;
            var midMaze = InitMaze(size, size, mid + 1, lines);
            var midValue = ShortestPath(start, end, midMaze);
            if (midValue == -1) right = mid;
            else left = mid + 1;
        }
        return lines[left];
    }
    
    private static Maze InitMaze(int rows, int cols, int take, string[] blockades)
    {
        var maze = new Maze(rows, cols);
        foreach (var line in blockades.Take(take))
        {
            var ints = line.Ints();
            maze.UpdateAt(ints[0], ints[1], '#');
        }
        return maze;
    }
    
    private static int ShortestPath(Index2D start, Index2D end, Maze maze)
    {
        var pq = new PriorityQueue<DijkstraNode, int>();
        pq.Enqueue(new DijkstraNode(start, null), 0);
        var nodes = new Dictionary<Index2D, DijkstraInfo>();
        while (pq.TryDequeue(out var dnode, out var distance))
        {
            var currentIndex = dnode.Current;
            if (nodes.TryGetValue(currentIndex, out var dijkstraInfo))
            {
                if (dijkstraInfo.Distance < distance)
                {
                    continue;
                }
                
                if (dijkstraInfo.Distance >= distance)
                {
                    dijkstraInfo.Distance = distance;
                    dijkstraInfo.Previous = dnode.Previous is { } previous ? [previous] : [];
                }
                if (dijkstraInfo.Distance == distance && dnode.Previous is { })
                {
                    continue;
                }
            }
            else
            {
                nodes.Add(currentIndex, new DijkstraInfo(distance, dnode.Previous is { } ? [dnode.Previous.Value] : []));
            }
            if (currentIndex == end) return distance;
            
            foreach (var nb in Directions.CardinalIndex.Select(dir => currentIndex + dir)
                         .Where(nb => maze.ValueOrDefault(nb) == '.'))
                //.Where(nb => !nodes.ContainsKey(nb)))
            {
                pq.Enqueue(new DijkstraNode(nb, currentIndex), distance + 1);
            }
        }
        return -1;
    }
    
    private record DijkstraNode(Index2D Current, Index2D? Previous);
    
    private class DijkstraInfo(int distance, List<Index2D> previous)
    {
        public int Distance { get; set; } = distance;
        public int Heuristic { get; set; } = 0;
        public List<Index2D> Previous { get; set; } = previous;
    }
};