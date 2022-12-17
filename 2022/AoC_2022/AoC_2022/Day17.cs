using Tools.Geometry;
namespace AoC_2022;


public sealed partial class Day17 : Day
{
    [Test]
    public void Example() => Tetris.Parse(InputExample).Simulate2(2022).Should().Be(3068);
    [Test]
    public void Part1() => Tetris.Parse(InputPart1).Simulate2(2022).Should().Be(3130);

    [Test]
    public void ExampleP2() => TwoTetrices.Parse(InputExample).FindHeight(1000000000000L).Should().Be(1514285714288L);
    [Test]
    public void Part2() => TwoTetrices.Parse(InputPart1).FindHeight(1000000000000L).Should().Be(1);

    private class TwoTetrices
    {
        public Tetris Me { get; set; }
        public Tetris Elephant { get; set; }
        public Tetris MeReset { get; set; }

        private Dictionary<long, long> Dict = new Dictionary<long, long>();

        public TwoTetrices(string[] input)
        {
            Me = Tetris.Parse(input);
            Elephant = Tetris.Parse(input);
            MeReset = Tetris.Parse(input);
        }

        public long FindHeight(long rocks)
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
            var a = rocks / period;
            return initialHeight + remainderHeight + (rocks - initial) / period * periodHeight;
        }

        public static TwoTetrices Parse(string[] input) => new TwoTetrices(input);
    }


    private class Tetris
    {
        private readonly string Wind;
        private int BlockType { get; set; }

        private int WindIndex { get; set; }

        private static readonly int MaxBlockCount = 2022;

        public Tetris(string wind)
        {
            Wind = wind;

            Rocks = Enumerable.Repeat(0, 80).ToArray();
            Rocks[0] = (1 << 30) - 1;
        }

        public long Simulate2(int times)
        {
            for (int i = 0; i < times; i++)
            {
                AddBlock();
            }
            Console.WriteLine(string.Join("\n", Rocks.Skip(0).Take(3000).Where(x => x != 0).Reverse().Select(x => Convert.ToString(x, 2).PadLeft(7, '0'))));
            return Height;
        }


        public class State
        {
            int[] Rocks;
            public long BlockCount { get; }
            int BlockType;
            int WindIndex;

            public State(int[] rocks, long blockCount, int blockType, int windIndex)
            {
                Rocks = rocks;
                BlockCount = blockCount;
                BlockType = blockType;
                WindIndex = windIndex;
            }

            public static bool operator ==(State self, State other)
            {
                return self?.BlockType == other?.BlockType
                && self?.WindIndex == other?.WindIndex
                && Enumerable.SequenceEqual(self.Rocks, other.Rocks);
            }

            public static bool operator !=(State self, State other) => !(self == other);
        };

        public State CurrentState => new State(Rocks, BlockCount, BlockType, WindIndex);

        public Block CurrentBlock { get; set; }
        private long RockHeight { get; set; }
        public int[] Rocks { get; set; }
        public long BlockCount { get; set; }
        public long Height => RockHeight + Rocks.ToList().FindLastIndex(x => x != 0);

        private int FullRow = (1 << 7) - 1;

        public long AddBlock()
        {
            CurrentBlock = new Block(BlockType);
            var blockHeight = Rocks.ToList().FindLastIndex(x => x != 0) + 4;
            while (true)
            {
                var windIndex = WindIndex;
                var wind = Wind[WindIndex];
                //try to be blown by wind
                if (wind == '>') CurrentBlock.MoveRight();
                else if (wind == '<') CurrentBlock.MoveLeft();
                else throw new Exception();

                //Console.WriteLine(string.Join("\n", rocks.Take(20).Reverse().Select(x => Convert.ToString(x, 2).PadLeft(7, '0'))));

                //if result overlaps move back
                if (CurrentBlock.Overlap(Rocks[blockHeight..(blockHeight + 4)]))
                {
                    if (wind == '>') CurrentBlock.MoveLeft();
                    else if (wind == '<') CurrentBlock.MoveRight();
                }
                //Console.WriteLine("After moving to the side");
                //Console.WriteLine(string.Join("\n", block.Occupied.Reverse().Select(x => Convert.ToString(x, 2).PadLeft(7, '0'))));

                //try move block down
                blockHeight--;
                if (CurrentBlock.Overlap(Rocks[blockHeight..(blockHeight + 4)]))
                {
                    //Add block to rocks
                    blockHeight++;
                    var min = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        Rocks[blockHeight + i] = Rocks[blockHeight + i] | CurrentBlock.Occupied[i];
                        if (Rocks[blockHeight + i] == FullRow) min = blockHeight + i;
                    }

                    //Console.WriteLine("Before remove");
                    //Console.WriteLine(string.Join("\n", rocks.Take(20).Reverse().Select(x => Convert.ToString(x, 2).PadLeft(7, '0'))));
                    //var min = Rocks.ToList().FindLastIndex(x => x == (1 << 7) - 1);
                    min = CleanupRocks();

                    if (min > 0)
                    {
                        RockHeight += min;
                        Rocks = Rocks.Skip(min).Concat(Enumerable.Repeat(0, min)).ToArray();
                    }
                    //Console.WriteLine("After remove");
                    //Console.WriteLine(string.Join("\n", rocks.Take(20).Reverse().Select(x => Convert.ToString(x, 2).PadLeft(7, '0'))));

                    BlockCount++;
                    WindIndex = (WindIndex + 1) % Wind.Length;

                    break;
                }
                WindIndex = (WindIndex + 1) % Wind.Length;
            }
            BlockType = (BlockType + 1) % 5;
            return 0;
        }

        //DFS to determine a path from left to right and save the minimum value
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
                    if (current == leftPoint) break;
                    visited.Add(current);
                    foreach (var next in Neighbors(current).Where(Valid))
                    {
                        stack.Push(next);
                        min = Math.Min(min, next.X);
                    }
                }
            }
            return min;
        }

        private bool Valid(Point point) =>
            point.X >= 0 &&
            point.Y >= 0 && point.Y < 7 &&
            (Rocks[point.X] & (1 << point.Y)) > 0;

        private static List<Point> Neighbors(Point point)
        {
            var list = new List<Point>();
            list.Add(new Point(point.X, point.Y - 1));
            list.Add(new Point(point.X - 1, point.Y));
            list.Add(new Point(point.X, point.Y + 1));
            list.Add(new Point(point.X + 1, point.Y));
            return list;
        }


        public class Block
        {
            public int Type { get; set; }
            public int[] Occupied = new int[4];

            public Block(int blockType)
            {
                Type = blockType;
                if (Type == 0)
                {
                    Occupied[0] = 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4;
                }
                else if (Type == 1)
                {
                    Occupied[2] = 1 << 3;
                    Occupied[1] = 1 << 2 | 1 << 3 | 1 << 4;
                    Occupied[0] = 1 << 3;
                }
                else if (Type == 2)
                {
                    Occupied[2] = 1 << 2;
                    Occupied[1] = 1 << 2;
                    Occupied[0] = 1 << 2 | 1 << 3 | 1 << 4;
                }
                else if (Type == 3)
                {
                    Occupied[3] = 1 << 4;
                    Occupied[2] = 1 << 4;
                    Occupied[1] = 1 << 4;
                    Occupied[0] = 1 << 4;
                }
                else if (Type == 4)
                {
                    Occupied[1] = 1 << 3 | 1 << 4;
                    Occupied[0] = 1 << 3 | 1 << 4;
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

        public static Tetris Parse(string[] lines)
        {
            return new Tetris(lines[0]);
        }

    }
}