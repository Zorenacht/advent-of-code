using FluentAssertions;
using System.Collections;
using Tools.Geometry;

namespace AoC_2024;

public sealed class Day06 : Day
{
    [Puzzle(answer: 4559)]
    public int Part1()
    {
        int result = 0;
        var lines = Input;
        //var grouped = lines.GroupBy(string.Empty);
        var grid = lines.ToGrid();
        Index2D? start = grid.FindIndexes('^').FirstOrDefault();
        var dir = Direction.N;
        var set = new HashSet<Index2D>();
        while (start != null)
        {
            grid.ValueOrDefault(start.Value);
            set.Add(start.Value);
            
            //next
            var attempt = start.Value.Neighbor(dir);
            while (grid.ValueOrDefault(attempt) == '#' && attempt != start)
            {
                dir = dir.Right();
                attempt = start.Value.Neighbor(dir);
            }
            //if (set.Contains(start.Value)) break;
            if (grid.ValueOrDefault(attempt) == null) break;
            start = attempt;
        }
        return set.Count;
    }
    
    private record IndexDir(Index2D Index, Direction Direction);
    
    private void ReverseWalk(IndexDir indexDir, HashSet<IndexDir> cycleIndices, Grid<char> grid)
    {
        var queue = new Queue<IndexDir>();
        queue.Enqueue(indexDir);
        while (queue.Any())
        {
            var current = queue.Dequeue();
            if (grid.ValueOrDefault(current.Index) is null || !cycleIndices.Add(current)) continue;
            
            var left = current.Direction.Left();
            if(grid.ValueOrDefault(current.Index.Neighbor(left)) == '#')
                queue.Enqueue(new IndexDir(
                    current.Index.Neighbor(left.Backwards()), 
                    left));
            queue.Enqueue(current with { Index = current.Index.Neighbor(current.Direction.Backwards()) });
        }
    }
    
    //not 551 or 550 or 3965 or 3964
    [Puzzle(answer: null)]
    public int Part2()
    {
        var lines = InputExample;
        var grid = lines.ToGrid();
        Index2D? start = grid.FindIndexes('^').FirstOrDefault();
        var dir = Direction.N;
        var set = new HashSet<IndexDir>();
        var cycleIndices = new HashSet<IndexDir>();
        var blockades = new HashSet<Index2D>();
        while (start != null)
        {
            grid.ValueOrDefault(start.Value);
            set.Add(new IndexDir(start.Value, dir));
            ReverseWalk(new IndexDir(start.Value, dir), cycleIndices, grid);
            
            var attempt = start.Value.Neighbor(dir);
            while (grid.ValueOrDefault(attempt) == '#' && attempt != start)
            {
                dir = dir.Right();
                attempt = start.Value.Neighbor(dir);
            }
            if (cycleIndices.Contains(new IndexDir(attempt, dir.Right())) 
                && grid.ValueOrDefault(attempt.Neighbor(dir)) is not '#' and not null)
            {
                blockades.Add(attempt.Neighbor(dir));
            }
            if (grid.ValueOrDefault(attempt) == null) break;
            start = attempt;
        }
        return blockades.Count;
    }
};