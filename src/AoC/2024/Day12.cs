using FluentAssertions;
using FluentAssertions.Data;
using Tools.Geometry;

namespace AoC_2024;

public sealed class Day12 : Day
{
    [Puzzle(answer: null)]
    public int Part1()
    {
        int result = 0;
        var grid = Input.ToGrid();
        var areas = grid.FloodFillInclude("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        foreach (var area in areas.KeyedAreas)
        {
            var nbCount = new Dictionary<(double, double), int>();
            foreach (var index in area.Value)
            {
                foreach (var nb in new (double, double)[] { (0, 0.5), (0.5, 0), (-0.5, 0), (0, -0.5) }
                             .Select(x => (index.Row + x.Item1, index.Col + x.Item2)))
                {
                    if (nbCount.ContainsKey(nb)) nbCount[nb]++;
                    else nbCount[nb] = 1;
                }
            }
            var perim = nbCount.Count(x => x.Value == 1);
            var are = area.Value.Count;
            result += perim * are;
        }
        return result;
    }
    
    public record Border(double Y, double X, (double Y, double X) Dir)
    {
        public virtual bool Equals(Border? other) => other is { } && (Y, X) == (other.Y, other.X);
        public override int GetHashCode() => (Y, X).GetHashCode();
    }
    
    [Puzzle(answer: 818286)]
    public int Part2()
    {
        int result = 0;
        var grid = Input.ToGrid();
        var areas = grid.FloodFillInclude("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        foreach (var area in areas.KeyedAreas)
        {
            var nbCount = new Dictionary<Border, int>();
            foreach (var index in area.Value)
            {
                foreach (var nb in new (double, double)[] { (0, 0.5), (0.5, 0), (-0.5, 0), (0, -0.5) }
                             .Select(x => new Border(index.Row + x.Item1, index.Col + x.Item2, x)))
                {
                    if (!nbCount.TryAdd(nb, 1)) nbCount[nb]++;
                }
            }
            var perim = nbCount.Where(x => x.Value == 1);
            var perimX = perim
                .Select(x => x.Key)
                .Where(x => x.Y % 1 != 0)
                .GroupBy(x => x.Y);
            var px1Tot = 0;
            foreach (var group in perimX)
            {
                var prev = new Border(-100d, -100d, (-100d, -100d));
                foreach (var yxDir in group.OrderBy(x => x.X).ToArray())
                {
                    if (yxDir.X - prev.X > 1 || yxDir.Dir != prev.Dir)
                    {
                        px1Tot++;
                    }
                    prev = yxDir;
                }
            }
            
            var perimY = perim
                .Select(x => x.Key)
                .Where(x => x.X % 1 != 0)
                .GroupBy(x => (x.X, x.Dir));
            var px2Tot = 0;
            foreach (var group in perimY)
            {
                var prev = new Border(-100d, -100d, (-100d, -100d));
                foreach (var yxDir in group.OrderBy(x => x.Y).ToArray())
                {
                    if (yxDir.Y - prev.Y > 1 || yxDir.Dir != prev.Dir)
                    {
                        px1Tot++;
                    }
                    prev = yxDir;
                }
            }
            var are = area.Value.Count;
            result += (px1Tot + px2Tot) * are;
        }
        return result;
    }
};