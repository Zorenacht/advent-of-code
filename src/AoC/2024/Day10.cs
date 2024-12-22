using System.Diagnostics.CodeAnalysis;
using Tools.Geometry;

namespace AoC._2024;

[PuzzleType(PuzzleType.Grid, PuzzleType.ShortestPath)]
public sealed class Day10 : Day
{
    [Puzzle(answer: 733)]
    public int Part1()
    {
        var grid = Input.ToIntGrid();
        var bfs = new BFS<int>(
            (int current) => current == 0,
            (int current, int next) => next == current + 1,
            (int next) => next == 9);
        return bfs.Paths(grid).GroupBy(x => (x[0], x[^1])).Count();
    }

    [Puzzle(answer: 1514)]
    public int Part2()
    {
        var grid = Input.ToIntGrid();
        var bfs = new BFS<int>(
            (int current) => current == 0,
            (int current, int next) => next == current + 1,
            (int next) => next == 9);
        return bfs.Paths(grid).Count;
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private class BFS<T>(
        Func<T, bool> startingCondition,
        Func<T, T, bool> nextCondition,
        Func<T, bool> endCondition) where T : struct
    {
        public List<List<Index2D>> Paths(Grid<T> grid)
        {
            var final = new List<List<Index2D>>();
            foreach (var (start, val) in grid.EnumerableWithIndex())
            {
                if (!startingCondition(val)) continue;
                var current = new List<List<Index2D>> { new() { start } };
                while (current.Count > 0)
                {
                    var next = new List<List<Index2D>> { };
                    foreach (var curr in current)
                    {
                        var currIndex = curr[^1];
                        var currValue = grid.ValueAt(curr[^1]);
                        foreach (var dir in Directions.Cardinal)
                        {
                            var newIndex = currIndex + dir;
                            if (grid.ValueOrDefault(newIndex) is T nextValue && nextCondition(currValue, nextValue))
                            {
                                var copy = curr.ToList();
                                copy.Add(newIndex);
                                if (endCondition(nextValue)) final.Add(copy);
                                else next.Add(copy);
                            }
                        }
                    }
                    current = next;
                }
            }
            return final;
        }
    }
};