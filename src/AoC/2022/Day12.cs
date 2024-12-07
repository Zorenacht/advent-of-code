using ShortestPath;
using Tools.Geometry;

namespace AoC_2022;

public sealed partial class Day12 : Day
{
    [Test]
    public void Example() => Simulate(InputExample).Should().Be(31);
    [Test]
    public void Part1() => Simulate(InputPart1).Should().Be(534);
    [Test]
    public void ExampleP2() => new HillFinder(InputExample).ShortestPath().Should().Be(29);
    [Test]
    public void Part2() => new HillFinder(InputPart1).ShortestPath().Should().Be(525);


    private int Simulate(string[] lines)
    {
        var grid = new Grid<int>(lines.Length, lines[0].Length);
        var start = Index2D.O;
        var end = Index2D.O;
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == 'S') start = new Index2D(i, j);
                if (lines[i][j] == 'E') end = new Index2D(i, j);
                int value = lines[i][j] switch
                {
                    'S' => 1,
                    'E' => 26,
                    _ => lines[i][j] - 'a' + 1
                };
                grid.UpdateAt(i, j, value);
            }
        }
        var sp = new AStarPath<Hill>(new Hill(grid, start, end), new Hill(grid, end, end));
        sp.Run();
        return sp.ShortestPath;
    }

    private class HillFinder
    {
        Grid<int> Grid { get; set; }
        IEnumerable<Index2D> StartPoints { get; set; }
        Index2D End { get; set; }

        public HillFinder(string[] lines)
        {
            Grid = new Grid<int>(lines.Length, lines[0].Length);
            StartPoints = Parse(lines);
        }

        public int ShortestPath()
        {
            int max = int.MaxValue;
            foreach (var start in StartPoints)
            {
                var sp = new AStarPath<Hill>(new Hill(Grid, start, End), new Hill(Grid, End, End));
                sp.Run();
                if (sp.ShortestPath >= 0 && max > sp.ShortestPath)
                {
                    max = sp.ShortestPath;
                }
            }
            return max;
        }

        private IEnumerable<Index2D> Parse(string[] lines)
        {
            var list = new List<Index2D>();
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    int value = lines[i][j] switch
                    {
                        'S' => 1,
                        'E' => 26,
                        _ => lines[i][j] - 'a' + 1
                    };
                    Grid.UpdateAt(i, j, value);
                    if (lines[i][j] == 'E') End = new Index2D(i, j);
                    if (lines[i][j] == 'S' || lines[i][j] == 'a') list.Add(new Index2D(i, j));
                }
            }
            return list;
        }
    }

    public class Hill : IState<Hill>
    {
        public readonly Index2D Current;
        public readonly Index2D Goal;
        public readonly Grid<int> Hills;

        public Hill(Grid<int> hills, Index2D current, Index2D goal)
        {
            Hills = hills;
            Current = current;
            Goal = goal;
        }

        public IEnumerable<Node<Hill>> NextNodes(int initialDistance)
        {
            var c = Hills.ValueAt(Current);
            var n = Current.Neighbor(Direction.N);
            var s = Current.Neighbor(Direction.S);
            var e = Current.Neighbor(Direction.E);
            var w = Current.Neighbor(Direction.W);
            if (Hills.IsValid(n) && Hills.ValueAt(n) - c <= 1) yield return new Node<Hill>(new(Hills, n, Goal), initialDistance + 1);
            if (Hills.IsValid(s) && Hills.ValueAt(s) - c <= 1) yield return new Node<Hill>(new(Hills, s, Goal), initialDistance + 1);
            if (Hills.IsValid(e) && Hills.ValueAt(e) - c <= 1) yield return new Node<Hill>(new(Hills, e, Goal), initialDistance + 1);
            if (Hills.IsValid(w) && Hills.ValueAt(w) - c <= 1) yield return new Node<Hill>(new(Hills, w, Goal), initialDistance + 1);
        }

        public int Heuristic() => Math.Abs(Goal.Row - Current.Row) + Math.Abs(Goal.Col - Current.Col);
        public bool Equals(Hill? other) => Current == other?.Current;
        public override int GetHashCode() => Current.GetHashCode();
    }
}