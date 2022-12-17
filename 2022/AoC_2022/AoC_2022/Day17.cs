using MathNet.Numerics;
using System.Runtime.InteropServices;
using Tools.Geometry;
namespace AoC_2022;


public sealed partial class Day17 : Day
{
    [Test]
    public void Example() => Tetris.Parse(InputExample).Simulate2(2022).Should().Be(3068);
    [Test]
    public void Part1() => Tetris.Parse(InputPart1).Simulate2(2022).Should().Be(3130);

    [Test]
    public void ExampleP2() => TwoTetrices.Parse(InputExample).FindCycle().Should().Be(1514285714288L);
    [Test]
    public void Part2() => TwoTetrices.Parse(InputPart1).FindCycle().Should().Be(1);

    private class TwoTetrices
    {
        public Tetris Me { get; set; }
        public Tetris Elephant { get; set; }

        private Dictionary<long, long> Dict = new Dictionary<long, long>();

        public TwoTetrices(string[] input)
        {
            Me = Tetris.Parse(input);
            Elephant = Tetris.Parse(input);
        }

        public long FindCycle()
        {
            int count = 0;
            while (count < 100_000)
            {
                Me.AddBlock();
                Me.AddBlock();
                Elephant.AddBlock();
                if (Me.CurrentState == Elephant.CurrentState)
                {
                    break;
                }
                count++;
            }
            return 1;
        }

        public static TwoTetrices Parse(string[] input) => new TwoTetrices(input);
    }


    private class Tetris
    {
        private readonly string Wind;
        private int _blockType = 0;
        private int BlockType => (_blockType++ % 5);

        private int WindIndex { get; set; }

        private static readonly int MaxBlockCount = 2022;

        public Tetris(string wind)
        {
            Wind = wind;

            Rocks = Enumerable.Repeat(0, 6000).ToArray();
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

        public int Simulate()
        {
            int rockHeight = 0;
            var rocks = Enumerable.Repeat(0, 10000).ToArray();
            rocks[0] = (1 << 30) - 1;

            int blockCount = 1;
            int blockHeight = 4;
            Block block = new Block(BlockType);
            while (blockCount <= MaxBlockCount)
            {
                var wind = Wind[WindIndex];
                //try to be blown by wind
                if (wind == '>') block.MoveRight();
                else if (wind == '<') block.MoveLeft();
                else throw new Exception();

                //Console.WriteLine(string.Join("\n", rocks.Take(20).Reverse().Select(x => Convert.ToString(x, 2).PadLeft(7, '0'))));

                //if result overlaps move back
                if (block.Overlap(rocks[blockHeight..(blockHeight + 4)]))
                {
                    if (wind == '>') block.MoveLeft();
                    else if (wind == '<') block.MoveRight();
                }
                //Console.WriteLine("After moving to the side");
                //Console.WriteLine(string.Join("\n", block.Occupied.Reverse().Select(x => Convert.ToString(x, 2).PadLeft(7, '0'))));

                //try move block down
                blockHeight--;
                if (block.Overlap(rocks[blockHeight..(blockHeight + 4)]))
                {
                    if (block.Type == 3)
                    {
                        var a = 1;
                    }
                    //Add block to rocks
                    blockHeight++;
                    for (int i = 0; i < 4; i++)
                    {
                        rocks[blockHeight + i] = rocks[blockHeight + i] | block.Occupied[i];
                    }

                    //Remove unneeded
                    var highestRockIndices = new int[7];
                    for (int i = 0; i < 7; i++)
                    {
                        var index = rocks.ToList().FindLastIndex(x => (x & 1 << i) != 0);
                        highestRockIndices[i] = index;
                    }
                    //Console.WriteLine("Before remove");
                    //Console.WriteLine(string.Join("\n", rocks.Take(20).Reverse().Select(x => Convert.ToString(x, 2).PadLeft(7, '0'))));
                    var min = rocks.ToList().FindLastIndex(x => x == (1 << 7) - 1);
                    if (min > 0)
                    {
                        rockHeight += min;
                        rocks = rocks.Skip(min).Concat(Enumerable.Repeat(0, min)).ToArray();
                    }
                    //Console.WriteLine("After remove");
                    //Console.WriteLine(string.Join("\n", rocks.Take(20).Reverse().Select(x => Convert.ToString(x, 2).PadLeft(7, '0'))));

                    //Create new block
                    block = new Block(BlockType);
                    blockCount++;
                    blockHeight = rocks.ToList().FindLastIndex(x => x != 0) + 4;
                }

            }
            Console.WriteLine(string.Join("\n", rocks.Skip(1000).Take(20).Reverse().Select(x => Convert.ToString(x, 2).PadLeft(7, '0'))));
            return rockHeight + rocks.ToList().FindLastIndex(x => x != 0);
        }

        public class State
        {
            int[] Rocks;
            long BlockCount;
            int BlockType;
            int WindIndex;

            public State (int[] rocks, long blockCount, int blockType, int windIndex)
            {
                Rocks = rocks;
                BlockCount = blockCount;
                BlockType = blockType;
                WindIndex = windIndex;
            }

            public static bool operator ==(State self, State other)
            {
                return /*self?.BlockCount == other?.BlockCount
                    && self?.BlockType == other?.BlockType
                    && self?.WindIndex == other?.WindIndex
                    &&*/ Enumerable.SequenceEqual(self.Rocks, other.Rocks);
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

            return 0;
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