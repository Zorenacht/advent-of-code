using MathNet.Numerics;
using Tools.Geometry;
namespace AoC_2022;


public sealed partial class Day17 : Day
{
    [Test]
    public void Example() => Tetris.Parse(InputExample).Simulate().Should().Be(26);
    [Test]
    public void Part1() => Tetris.Parse(InputPart1).Simulate().Should().Be(5144286);

    [Test]
    public void ExampleP2() => Tetris.Parse(InputExample).SimulateP2().Should().Be(56000011);
    [Test]
    public void Part2() => Tetris.Parse(InputPart1).SimulateP2().Should().Be(1);

    private class Tetris
    {
        private readonly string Wind;
        private int _blockType = 0;
        private int BlockType
        {
            get
            {
                return (_blockType++ % 5);
            }
        }
        private int _windIndex = 0;
        private int WindIndex
        {
            get
            {
                return (_windIndex++ % Wind.Length);
            }
        }

        private static readonly int MaxBlockCount = 2022;

        public Tetris(string wind)
        {
            Wind = wind;
        }

        public int Simulate()
        {
            var rocks = Enumerable.Repeat(0, 100_000).ToArray();
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
                Console.WriteLine("After moving to the side");
                Console.WriteLine(string.Join("\n", block.Occupied.Reverse().Select(x => Convert.ToString(x, 2).PadLeft(7, '0'))));

                //try move block down
                blockHeight--;
                if (block.Overlap(rocks[blockHeight..(blockHeight + 4)]))
                {
                    blockHeight++;
                    for(int i = 0; i < 4; i++)
                    {
                        rocks[blockHeight+i] = rocks[blockHeight+i] | block.Occupied[i];
                    }
                    block = new Block(BlockType);
                    blockCount++;
                    blockHeight = rocks.ToList().FindLastIndex(x => x != 0) + 4;
                }

            }
            Console.WriteLine(string.Join("\n", rocks.Skip(1).Take(20).Reverse().Select(x => Convert.ToString(x, 2).PadLeft(7, '0'))));
            return rocks.ToList().FindLastIndex(x => x != 0);
        }
        
        public long SimulateP2()
        {
            var rocks = Enumerable.Repeat(0, 100_000).ToArray();
            var rocks2 = new int[7];
            rocks[0] = (1 << 30) - 1;

            long blockCount = 1;
            int blockHeight = 4; //needs to be long
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
                Console.WriteLine("After moving to the side");
                Console.WriteLine(string.Join("\n", block.Occupied.Reverse().Select(x => Convert.ToString(x, 2).PadLeft(7, '0'))));

                //try move block down
                blockHeight--;
                if (block.Overlap(rocks[blockHeight..(blockHeight + 4)]))
                {
                    blockHeight++;
                    for(int i = 0; i < 4; i++)
                    {
                        rocks[blockHeight+i] = rocks[blockHeight+i] | block.Occupied[i];
                    }
                    block = new Block(BlockType);
                    blockCount++;
                    blockHeight = rocks.ToList().FindLastIndex(x => x != 0) + 4;
                }

            }
            Console.WriteLine(string.Join("\n", rocks.Skip(1).Take(20).Reverse().Select(x => Convert.ToString(x, 2).PadLeft(7, '0'))));
            return rocks.ToList().FindLastIndex(x => x != 0);
        }

        public class Block
        {
            public int Type { get; set; }
            public int[] Occupied = new int[4];
            public int[] Start = new int[7];
            public int[] End = new int[7];

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
                for(int i=0; i<Math.Max(rocks.Length, Occupied.Length); i++)
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