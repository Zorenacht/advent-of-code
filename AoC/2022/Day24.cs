using ShortestPath;
using Tools.Geometry;

namespace AoC_2022;


public sealed partial class Day24 : Day
{
    [Test]
    public void Example() => Blizzard.Parse(InputExample).ShortestPath().Should().Be(18);
    [Test]
    public void Part1() => Blizzard.Parse(InputPart1).ShortestPath().Should().Be(343);

    [Test]
    public void ExampleP2() => Blizzard.Parse(InputExample).ShortestPath(true).Should().Be(54);
    [Test]
    public void Part2() => Blizzard.Parse(InputPart1).ShortestPath(true).Should().Be(960);



    private class BlizzardState : IState<BlizzardState>
    {
        public List<Grid> FieldStates { get; set; }

        public int StateIndex { get; set; }
        public Point Pos { get; set; }
        public Point Goal { get; set; }

        public Point Start { get; set; }
        public bool ReachedOnce { get; set; }
        public bool WentBack { get; set; }


        public bool Equals(BlizzardState? other)
        {
            return (StateIndex % FieldStates.Count) == (other?.StateIndex % FieldStates.Count)
                && Pos == other?.Pos && ReachedOnce == other?.ReachedOnce && WentBack == other?.WentBack;
        }

        public bool ReachedGoal() => Pos == Goal && ReachedOnce && WentBack;

        public override int GetHashCode() => Pos.GetHashCode() ^ ((StateIndex % FieldStates.Count) << 16);

        public int Heuristic()
        {
            var diffToGoal = Manhattan(Goal.Difference(Pos));
            var diffToStart = Manhattan(Start.Difference(Pos));
            var diffStartToGoal = Manhattan(Start.Difference(Goal));
            if (!ReachedOnce) return diffToGoal + 2 * diffStartToGoal;
            if (!WentBack) return diffToStart + diffStartToGoal;
            return diffToGoal;
        }
        private static int Manhattan(Point point) => Math.Abs(point.X) + Math.Abs(point.Y);

        public IEnumerable<Node<BlizzardState>> NextNodes(int initialDistance)
        {
            var nextField = FieldStates[(StateIndex + 1) % FieldStates.Count];
            foreach (var nb in Neighbors(Pos))
            {
                if (nextField.IsValid(nb) && nextField.ValueAt(nb) == 0)
                {
                    var state = new BlizzardState()
                    {
                        FieldStates = FieldStates,
                        StateIndex = StateIndex + 1,
                        Pos = nb,
                        Start = Start,
                        Goal = Goal,
                        ReachedOnce = ReachedOnce || nb == Goal,
                        WentBack = WentBack || ReachedOnce && nb == Start
                    };
                    yield return new Node<BlizzardState>(state, initialDistance + 1);
                }
            }
        }

        private IEnumerable<Point> Neighbors(Point point)
        {
            yield return point.Neighbor2(Direction.N);
            yield return point.Neighbor2(Direction.S);
            yield return point.Neighbor2(Direction.E);
            yield return point.Neighbor2(Direction.W);
            yield return point;
        }
    }

    private class Blizzard
    {

        private readonly List<Grid> FieldStates;

        public Blizzard(List<Grid> states)
        {
            FieldStates = states;
        }


        public int ShortestPath(bool retrieve = false)
        {
            var fromCol = FieldStates[0][0].ToList().FindIndex(x => x == 0);
            var lastRow = FieldStates[0].RowLength - 1;
            var toCol = FieldStates[0][lastRow].ToList().FindIndex(x => x == 0);

            var fromPoint = new Point(0, fromCol);
            var toPoint = new Point(lastRow, toCol);

            var from = new BlizzardState()
            {
                FieldStates = FieldStates,
                Pos = fromPoint,
                StateIndex = 0,
                Start = fromPoint,
                Goal = toPoint,
                ReachedOnce = !retrieve,
                WentBack = !retrieve,
            };
            int min = int.MaxValue;
            var to = new BlizzardState()
            {
                FieldStates = FieldStates,
                Pos = toPoint,
                Goal = toPoint
            };
            var astar = new AStarPath<BlizzardState>(from, to, state => state.ReachedGoal());
            astar.Run();
            min = Math.Min(min, astar.ShortestPath);
            return min;
        }


        public static Blizzard Parse(string[] lines)
        {
            var grids = new List<Grid>();
            var grid = new Grid(lines.Length, lines[0].Length);
            for (int row = 0; row < lines.Length; row++)
            {
                var line = lines[row];
                for (int col = 0; col < line.Length; col++)
                {
                    grid[row][col] = WindDir(line[col]);
                }
            }
            do
            {
                grids.Add(grid);
                var newGrid = new Grid(grid.RowLength, grid.ColumnLength);
                for (int i = 0; i < grid.RowLength; i++)
                {
                    for (int j = 0; j < grid.ColumnLength; j++)
                    {
                        if (grid[i][j] == 1 << 4)
                        {
                            newGrid[i][j] = 1 << 4;
                            continue;
                        }

                        if ((grid[i][j] & (1 << 0)) > 0)
                        {
                            var to = Correct(new Point(i, j + 1), grid);
                            newGrid.UpdateAt(to, newGrid.ValueAt(to) + (1 << 0));
                        }
                        if ((grid[i][j] & (1 << 1)) > 0)
                        {
                            var to = Correct(new Point(i, j - 1), grid);
                            newGrid.UpdateAt(to, newGrid.ValueAt(to) + (1 << 1));
                        }
                        if ((grid[i][j] & (1 << 2)) > 0)
                        {
                            var to = Correct(new Point(i + 1, j), grid);
                            newGrid.UpdateAt(to, newGrid.ValueAt(to) + (1 << 2));
                        }
                        if ((grid[i][j] & (1 << 3)) > 0)
                        {
                            var to = Correct(new Point(i - 1, j), grid);
                            newGrid.UpdateAt(to, newGrid.ValueAt(to) + (1 << 3));
                        }

                    }
                }
                grid = newGrid;
            } while (grid != grids[0]);
            return new Blizzard(grids);
        }
    }

    private static Point Correct(Point point, Grid grid)
    {
        int length = grid.RowLength;
        int width = grid.ColumnLength;
        if (point.X == 0) return new Point(length - 2, point.Y);
        else if (point.Y == 0) return new Point(point.X, width - 2);
        else if (point.X == length - 1) return new Point(1, point.Y);
        else if (point.Y == width - 1) return new Point(point.X, 1);
        return point;
    }

    private static int WindDir(char c) => c switch
    {
        '>' => 1 << 0,
        '<' => 1 << 1,
        'v' => 1 << 2,
        '^' => 1 << 3,
        '#' => 1 << 4,
        '.' => 0,
        _ => throw new NotImplementedException(),
    };
}
