namespace AoC_2024;

public sealed class Day12 : Day
{
    [Puzzle(answer: 1370100)]
    public int Part1()
    {
        int result = 0;
        var grid = Input.ToGrid();
        var areas = grid.FloodFillRegions();
        foreach (var area in areas.KeyedAreas)
        {
            var nbCount = new Dictionary<Border, int>();
            foreach (var index in area.Value)
            {
                foreach (var nb in BorderIndex.Cardinals
                             .Select(x => new Border(index.Row + x.Row, index.Col + x.Col, x)))
                {
                    if (!nbCount.TryAdd(nb, 1)) ++nbCount[nb];
                }
            }
            var perimeterCount = nbCount.Count(x => x.Value == 1);
            var areaCount = area.Value.Count;
            result += perimeterCount * areaCount;
        }
        return result;
    }
    
    public record Border(double Row, double Col, BorderIndex Dir)
    {
        public virtual bool Equals(Border? other) => other is { } && (Row, Col) == (other.Row, other.Col);
        public override int GetHashCode() => (Row, Col).GetHashCode();
    }
    
    public struct BorderIndex(double row, double col)
    {
        public double Row { get; } = row;
        public double Col { get; } = col;
        
        public static readonly BorderIndex N = new(-0.5, +0.0);
        public static readonly BorderIndex S = new(+0.5, +0.0);
        public static readonly BorderIndex E = new(+0.0, +0.5);
        public static readonly BorderIndex W = new(+0.0, -0.5);
        public static readonly BorderIndex NE = N + E;
        public static readonly BorderIndex NW = N + W;
        public static readonly BorderIndex SW = S + W;
        public static readonly BorderIndex SE = S + E;
        
        public static BorderIndex operator +(BorderIndex left, BorderIndex right) => new(left.Row + right.Row, left.Col + right.Col);
        
        public bool Equals(BorderIndex other) => Math.Abs(other.Row - Row) < 0.00001 && Math.Abs(other.Col - Col) < 0.00001;
        public static bool operator ==(BorderIndex left, BorderIndex right) => left.Equals(right);
        public static bool operator !=(BorderIndex left, BorderIndex right) => !(left == right);
        
        public static readonly BorderIndex[] Cardinals = [E, N, W, S];
    };
    
    [Puzzle(answer: 818286)]
    public int Part2()
    {
        int result = 0;
        var grid = Input.ToGrid();
        var areas = grid.FloodFillRegions();
        foreach (var area in areas.KeyedAreas)
        {
            var nbCount = new Dictionary<Border, int>();
            foreach (var index in area.Value)
            {
                foreach (var nb in BorderIndex.Cardinals
                             .Select(x => new Border(index.Row + x.Row, index.Col + x.Col, x)))
                {
                    if (!nbCount.TryAdd(nb, 1)) ++nbCount[nb];
                }
            }
            var perimeter = nbCount.Where(x => x.Value == 1).ToArray();
            var horizontalCost = perimeter
                .Select(x => x.Key)
                .Where(x => x.Row % 1 != 0)
                .GroupBy(x => (x.Row, x.Dir))
                .Select(x => x.OrderBy(border => border.Col).ToArray())
                .Sum(group => group.Zip(group.Skip(1))
                    .Count(x => x.Second.Col - x.First.Col > 1 || x.Second.Dir != x.First.Dir)
                    .Plus(1));
            var verticalCost = perimeter
                .Select(x => x.Key)
                .Where(x => x.Col % 1 != 0)
                .GroupBy(x => (x.Col, x.Dir))
                .Select(x => x.OrderBy(border => border.Row).ToArray())
                .Sum(group => group.Zip(group.Skip(1))
                    .Count(x => x.Second.Row - x.First.Row > 1 || x.Second.Dir != x.First.Dir)
                    .Plus(1));
            var areaCost = area.Value.Count;
            result += (horizontalCost + verticalCost) * areaCost;
        }
        return result;
    }
};