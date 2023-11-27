using Tools.Geometry;
using static System.Net.Mime.MediaTypeNames;

namespace AoC_2022;


public sealed partial class Day17 : Day
{
    [Test]
    public void Example() => Tetris.Parse(InputExample).BruteForceHeight(2022).Should().Be(3068);
    [Test]
    public void Part1() => Tetris.Parse(InputPart1).BruteForceHeight(2022).Should().Be(3130);

    [Test]
    public void ExampleP2() => TwoTetrices.Parse(InputExample).FloydCycleHeight(1000000000000L).Should().Be(1514285714288L);
    [Test]
    public void Part2() => TwoTetrices.Parse(InputPart1).FloydCycleHeight(1000000000000L).Should().Be(1556521739139L);

    private class TwoTetrices
    {
        private readonly Tetris Me;
        private readonly Tetris Elephant;
        private readonly Tetris MeReset;

        private TwoTetrices(string[] input)
        {
            Me = Tetris.Parse(input);
            Elephant = Tetris.Parse(input);
            MeReset = Tetris.Parse(input);
        }

        public static TwoTetrices Parse(string[] input) => new TwoTetrices(input);

        public long FloydCycleHeight(long rocks)
        {
            //Find cycle
            do
            {
                Me.AddBlock();
                Me.AddBlock();
                Elephant.AddBlock();
            } while (Me.CurrentState != Elephant.CurrentState);

            //Find initial start of cycle
            while (MeReset.CurrentState != Elephant.CurrentState)
            {
                MeReset.AddBlock();
                Elephant.AddBlock();
            }
            var initial = MeReset.BlockCount;
            var initialHeight = MeReset.Height;

            //Find period
            MeReset.AddBlock();
            while (MeReset.CurrentState != Elephant.CurrentState)
            {
                MeReset.AddBlock();
            }
            var period = MeReset.BlockCount - initial;
            var periodHeight = MeReset.Height - initialHeight;

            //Find remainder height
            for (int i = 0; i < (rocks - initial) % period; i++)
            {
                MeReset.AddBlock();
            }
            var remainderHeight = MeReset.Height - periodHeight - initialHeight;
            return initialHeight + remainderHeight + (rocks - initial) / period * periodHeight;
        }
    }

    private class Tetris
    {
        public TetrisState CurrentState => new TetrisState(Rocks, BlockCount, BlockType, WindIndex);
        public long BlockCount { get; set; }
        public long Height => RockHeight + Rocks.ToList().FindLastIndex(x => x != 0);


        private readonly string Wind;
        private int WindIndex { get; set; }
        private int BlockType { get; set; }
        private TetrisBlock? CurrentBlock { get; set; }
        private long RockHeight { get; set; }
        private int[] Rocks { get; set; }

        public static Tetris Parse(string[] lines) => new Tetris(lines[0]);

        private Tetris(string wind)
        {
            Wind = wind;
            Rocks = Enumerable.Repeat(0, 80).ToArray();
            Rocks[0] = (1 << 30) - 1; //-1 is better
        }

        public long BruteForceHeight(int times)
        {
            for (int i = 0; i < times; i++)
            {
                AddBlock();
            }
            return Height;
        }

        public void AddBlock()
        {
            CurrentBlock = new TetrisBlock(BlockType);
            var blockHeight = Rocks.ToList().FindLastIndex(x => x != 0) + 4;
            while (true)
            {
                var windIndex = WindIndex;
                var wind = Wind[WindIndex];

                //try to be blown by wind
                if (wind == '>') CurrentBlock.MoveRight();
                else if (wind == '<') CurrentBlock.MoveLeft();

                //if result overlaps move back
                if (CurrentBlock.Overlap(Rocks[blockHeight..(blockHeight + 4)]))
                {
                    if (wind == '>') CurrentBlock.MoveLeft();
                    else if (wind == '<') CurrentBlock.MoveRight();
                }

                //try move block down
                blockHeight--;
                if (CurrentBlock.Overlap(Rocks[blockHeight..(blockHeight + 4)]))
                {
                    //Add block to rocks
                    blockHeight++;
                    for (int i = 0; i < 4; i++)
                    {
                        Rocks[blockHeight + i] = Rocks[blockHeight + i] | CurrentBlock.Occupied[i];
                    }

                    //Determine obsolete rocks
                    var min = CleanupRocks();
                    if (min > 0)
                    {
                        RockHeight += min;
                        Rocks = Rocks.Skip(min).Concat(Enumerable.Repeat(0, min)).ToArray();
                    }

                    BlockCount++;
                    WindIndex = (WindIndex + 1) % Wind.Length;
                    break;
                }
                WindIndex = (WindIndex + 1) % Wind.Length;
            }
            BlockType = (BlockType + 1) % 5;
        }

        //DFS to determine a path from right to left and return the minimum row value of the path
        private int CleanupRocks()
        {
            var rocks = Rocks.ToList();
            var leftRow = rocks.FindLastIndex(x => (x & (1 << 6)) != 0);
            var rightRow = rocks.FindLastIndex(x => (x & (1 << 0)) != 0);
            var leftPoint = new Point(leftRow, 6);
            var current = new Point(rightRow, 0);
            var stack = new Stack<Point>(Neighbors(current).Where(Valid));
            var visited = new HashSet<Point>(new List<Point>() { current });
            var min = int.MaxValue;
            while (stack.Count > 0)
            {
                current = stack.Pop();
                if (!visited.Contains(current))
                {
                    min = Math.Min(min, current.X);
                    if (current == leftPoint) break;
                    visited.Add(current);
                    foreach (var next in Neighbors(current).Where(Valid))
                    {
                        stack.Push(next);
                    }
                }
            }
            return min;
        }

        private bool Valid(Point point) =>
            point.X >= 0 &&
            point.Y >= 0 && point.Y < 7 &&
            (Rocks[point.X] & (1 << point.Y)) > 0;

        private static List<Point> Neighbors(Point point) => new()
        {
            new Point(point.X - 1, point.Y),
            new Point(point.X, point.Y - 1),
            new Point(point.X, point.Y + 1),
            new Point(point.X + 1, point.Y)
        };
    }
}

public class TetrisBlock
{
    public int Type { get; set; }
    public int[] Occupied = new int[4];

    public TetrisBlock(int blockType)
    {
        Type = blockType;
        if (Type == 0)
        {
            Occupied[0] = 0b11110;
        }
        else if (Type == 1)
        {
            Occupied[2] = 0b01000;
            Occupied[1] = 0b11100;
            Occupied[0] = 0b01000;
        }
        else if (Type == 2)
        {
            Occupied[2] = 0b00100;
            Occupied[1] = 0b00100;
            Occupied[0] = 0b11100;
        }
        else if (Type == 3)
        {
            Occupied[3] = 0b10000;
            Occupied[2] = 0b10000;
            Occupied[1] = 0b10000;
            Occupied[0] = 0b10000;
        }
        else if (Type == 4)
        {
            Occupied[1] = 0b11000;
            Occupied[0] = 0b11000;
        }
    }

    public bool Overlap(int[] rocks)
    {
        for (int i = 0; i < Math.Max(rocks.Length, Occupied.Length); i++)
        {
            if ((rocks[i] & Occupied[i]) != 0) return true;
        }
        return false;
    }

    public bool MoveLeft()
    {
        if (Occupied.Any(row => (row & (1 << 6)) != 0))
            return false;
        for (int i = 0; i < Occupied.Length; i++)
        {
            Occupied[i] = Occupied[i] << 1;
        }
        return true;
    }

    public bool MoveRight()
    {
        if (Occupied.Any(row => (row & (1 << 0)) != 0))
            return false;
        for (int i = 0; i < Occupied.Length; i++)
        {
            Occupied[i] = Occupied[i] >> 1;
        }
        return true;
    }
}

public class TetrisState
{
    int[] Rocks;
    int BlockType;
    int WindIndex;

    public TetrisState(int[] rocks, long blockCount, int blockType, int windIndex)
    {
        Rocks = rocks;
        BlockType = blockType;
        WindIndex = windIndex;
    }

    public static bool operator ==(TetrisState self, TetrisState other) => self.Equals(other);

    public static bool operator !=(TetrisState self, TetrisState other) => !(self == other);

    public override bool Equals(object? obj)
    {
        if (obj is TetrisState { } state)
        {
            return
                BlockType == state.BlockType &&
                WindIndex == state.WindIndex &&
                Enumerable.SequenceEqual(Rocks, state.Rocks);
        }
        return false;
    }

    public override int GetHashCode() => Rocks.Length;
};