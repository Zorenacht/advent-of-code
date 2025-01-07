using Tools.Geometry;

namespace AoC._2024;

[PuzzleType(PuzzleType.Grid, PuzzleType.ShortestPath)]
public sealed class Day20 : Day
{
    [Puzzle(answer: 1393)]
    public long Part1() => Cheats(cheatDuration: 2, minIncrease: 100);

    [Puzzle(answer: 990096)]
    public long Part2() => Cheats(cheatDuration: 20, minIncrease: 100);

    private long Cheats(int cheatDuration, int minIncrease = 100)
    {
        // Initialize
        var maze = Input.ToMaze();
        var start = maze.FindIndexes('S').First();
        var end = maze.FindIndexes('E').First();
        maze.UpdateAt(start, '.');
        maze.UpdateAt(end, '.');

        // Determine path and all distances till the end
        var current = start;
        var distancesTillEnd = new Dictionary<Index2D, int> { { start, 0 } };
        for (int i = 1; i < int.MaxValue; ++i)
        {
            if (current == end) break;
            var next = Directions.CardinalIndex.Select(dir => current + dir)
                .FirstOrDefault(nxt => !distancesTillEnd.ContainsKey(nxt) && maze.ValueOrDefault(nxt) == '.');
            distancesTillEnd.Add(next, i);
            current = next;
        }
        var distance = distancesTillEnd[end];
        distancesTillEnd = distancesTillEnd.Select(x => new KeyValuePair<Index2D, int>(x.Key, distance - x.Value)).ToDictionary();
<<<<<<< Updated upstream

        // Loop through the path and look in a manhattan area around it for all possible exit points
=======
        
        // Loop through the path and look in a Manhattan area around it for all possible exit points
>>>>>>> Stashed changes
        var distances = new Dictionary<int, int>();
        var list = distancesTillEnd.Select(kv => (kv.Key, distance - kv.Value)).ToList<(Index2D Index, int Distance)>();
        foreach (var indexDistance in list)
        {
            foreach (var nb in indexDistance.Index.WithinManhattanDistance(cheatDuration))
            {
                if (maze.ValueOrDefault(nb.Index) != '.') continue;
                if (!distances.TryAdd(indexDistance.Distance + nb.Distance + distancesTillEnd[nb.Index], 1))
                    distances[indexDistance.Distance + nb.Distance + distancesTillEnd[nb.Index]] += 1;
            }
        }
        var ordered = distances.Select(x => new KeyValuePair<int, int>(distance - x.Key, x.Value))
            .Where(x => x.Key > 0)
            .OrderBy(x => x.Key)
            .ToDictionary();
        return ordered
            .Where(x => x.Key >= minIncrease)
            .Sum(x => x.Value);
    }
};