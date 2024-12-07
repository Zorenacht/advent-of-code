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
        public List<Grid<int>> FieldStates { get; set; } = [];

        public int StateIndex { get; set; }
        public Index2D Pos { get; set; }
        public Index2D Goal { get; set; }

        public Index2D Start { get; set; }
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
            var diffToGoal = Manhattan(Goal - Pos);
            var diffToStart = Manhattan(Start - Pos);
            var diffStartToGoal = Manhattan(Start - Goal);
            if (!ReachedOnce) return diffToGoal + 2 * diffStartToGoal;
            if (!WentBack) return diffToStart + diffStartToGoal;
            return diffToGoal;
        }
        private static int Manhattan(Index2D index) => Math.Abs(index.Row) + Math.Abs(index.Col);

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

        private IEnumerable<Index2D> Neighbors(Index2D index)
        {
            yield return index.Neighbor(Direction.N);
            yield return index.Neighbor(Direction.S);
            yield return index.Neighbor(Direction.E);
            yield return index.Neighbor(Direction.W);
            yield return index;
        }
    }

    private class Blizzard
    {
        private readonly List<Grid<int>> FieldStates;

        public Blizzard(List<Grid<int>> states)
        {
            FieldStates = states;
        }

        public int ShortestPath(bool retrieve = false)
        {
            var fromCol = FieldStates[0][0].ToList().FindIndex(x => x == 0);
            var lastRow = FieldStates[0].RowLength - 1;
            var toCol = FieldStates[0][lastRow].ToList().FindIndex(x => x == 0);

            var fromPoint = new Index2D(0, fromCol);
            var toPoint = new Index2D(lastRow, toCol);

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
            var grids = new List<Grid<int>>();
            var grid = new Grid<int>(lines.Length, lines[0].Length);
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
                var newGrid = new Grid<int>(grid.RowLength, grid.ColLength);
                for (int i = 0; i < grid.RowLength; i++)
                {
                    for (int j = 0; j < grid.ColLength; j++)
                    {
                        if (grid[i][j] == 1 << 4)
                        {
                            newGrid[i][j] = 1 << 4;
                            continue;
                        }

                        if ((grid[i][j] & (1 << 0)) > 0)
                        {
                            var to = Correct(new Index2D(i, j + 1), grid);
                            newGrid.UpdateAt(to, newGrid.ValueAt(to) + (1 << 0));
                        }
                        if ((grid[i][j] & (1 << 1)) > 0)
                        {
                            var to = Correct(new Index2D(i, j - 1), grid);
                            newGrid.UpdateAt(to, newGrid.ValueAt(to) + (1 << 1));
                        }
                        if ((grid[i][j] & (1 << 2)) > 0)
                        {
                            var to = Correct(new Index2D(i + 1, j), grid);
                            newGrid.UpdateAt(to, newGrid.ValueAt(to) + (1 << 2));
                        }
                        if ((grid[i][j] & (1 << 3)) > 0)
                        {
                            var to = Correct(new Index2D(i - 1, j), grid);
                            newGrid.UpdateAt(to, newGrid.ValueAt(to) + (1 << 3));
                        }

                    }
                }
                grid = newGrid;
            } while (grid != grids[0]);
            return new Blizzard(grids);
        }
    }

    private static Index2D Correct(Index2D index, Grid<int> grid)
    {
        int length = grid.RowLength;
        int width = grid.ColLength;
        if (index.Row == 0) return new Index2D(length - 2, index.Col);
        else if (index.Col == 0) return new Index2D(index.Row, width - 2);
        else if (index.Row == length - 1) return new Index2D(1, index.Col);
        else if (index.Col == width - 1) return new Index2D(index.Row, 1);
        return index;
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
