using FluentAssertions;
using System.Collections;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using System.Runtime.InteropServices.JavaScript;
using Tools.Geometry;

namespace AoC_2024;

public sealed class Day06 : Day
{
    [Puzzle(answer: 4559)]
    public int Part1() => Path(Input).GroupBy(x => x.Index).Count();
    
    private HashSet<IndexDir> Path(string[] lines)
    {
        var grid = lines.ToGrid();
        var start = grid.FindIndexes('^').First();
        var dir = Direction.N;
        var set = new HashSet<IndexDir>();
        while (true)
        {
            if (grid.ValueOrDefault(start) == null) break;
            set.Add(new IndexDir(start, dir));
            
            var attempt = start.Neighbor(dir);
            while (grid.ValueOrDefault(attempt) == '#')
            {
                dir = dir.Right();
                attempt = start.Neighbor(dir);
            }
            start = attempt;
        }
        return set;
    }
    
    
    //not 551 or 550 or 3965 or 3964 or 1658 or 1646 or 1656 or 1623
    [Puzzle(answer: 1604)]
    public int Part2() => Loop(Input);
    
    [Puzzle(answer: 6)]
    public int Part2Example() => Loop(InputExample);
    
    private record IndexDir(Index2D Index, Direction Direction);
    
    private int Loop(string[] lines)
    {
        var grid = lines.ToGrid();
        var start = grid.FindIndexes('^').FirstOrDefault();
        var dir = Direction.N;
        int count = 0;
        var path = Path(lines);
        foreach (var pair in path.Zip(path.Skip(1))
                     .Where(pair => grid.ValueOrDefault(pair.Second.Index) is '.')
                     .GroupBy(pair => pair.Second.Index)
                     .Select(x => x.First()))
        {
            if (HasCycle(pair.First, pair.Second.Index, grid))
                count++;
        }
        return count;
    }
    
    private static bool HasCycle(IndexDir indexDir, Index2D blockCandidate, Grid<char> grid)
    {
        var simulated = new HashSet<IndexDir>();
        var start = indexDir.Index;
        var dir = indexDir.Direction;
        while (true)
        {
            if (grid.ValueOrDefault(start) == null) break;
            
            if (simulated.Contains(new IndexDir(start, dir)))
                return true;
            simulated.Add(new IndexDir(start, dir));
            
            var next = start.Neighbor(dir);
            while (grid.ValueOrDefault(next) == '#' || next == blockCandidate)
            {
                dir = dir.Right();
                next = start.Neighbor(dir);
            }
            start = next;
        }
        return false;
    }
}