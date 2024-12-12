using Tools.Geometry;

namespace AoC._2022;


public sealed partial class Day23 : Day
{
    [Test]
    public void Example() => ElfRectangle.Parse(InputExample).Simulate().Should().Be(110);
    [Test]
    public void Part1() => ElfRectangle.Parse(InputPart1).Simulate().Should().Be(4249);

    [Test]
    public void ExampleP2() => ElfRectangle.Parse(InputExample).FirstTurnNoMoves().Should().Be(20);
    [Test]
    public void Part2() => ElfRectangle.Parse(InputPart1).FirstTurnNoMoves().Should().Be(980);



    private class ElfRectangle
    {
        public readonly HashSet<Point> Elves;
        public readonly Dictionary<Point, Point> FromTo;
        public readonly Dictionary<Point, int> ToCount;
        public readonly int XMax;
        public readonly int YMax;

        public ElfRectangle(HashSet<Point> elves, int xMax, int yMax)
        {
            Elves = elves;
            XMax = xMax;
            YMax = yMax;
            FromTo = new Dictionary<Point, Point>();
            ToCount = new Dictionary<Point, int>();
        }

        private void Print()
        {
            var x = Elves.OrderBy(elf => elf.X).ToList();
            var y = Elves.OrderBy(elf => elf.Y).ToList();
            Console.WriteLine("-----------------------------------");
            for (int i = y.First().Y; i <= y.Last().Y; i++)
            {
                for (int j = x.First().X; j <= x.Last().X; j++)
                {
                    if (Elves.Contains(new Point(j, i))) Console.Write('#');
                    else Console.Write('.');
                }
                Console.WriteLine();
            }
        }
        public int FirstTurnNoMoves()
        {
            int turn = 0;
            bool moved = true;
            while (moved)
            {
                moved = false;
                foreach (var elf in Elves)
                {
                    if (!AnyAdjecent(elf)) continue;
                    FirstHalf(elf, turn);
                }
                foreach (var pair in FromTo)
                {
                    if (ToCount[pair.Value] == 1)
                    {
                        moved = true;
                        Elves.Remove(pair.Key);
                        Elves.Add(pair.Value);
                    }
                }
                FromTo.Clear();
                ToCount.Clear();
                turn++;
            }
            return turn;
        }

        public int Simulate()
        {
            Print();
            for (int i = 0; i < 10; i++)
            {
                foreach (var elf in Elves)
                {
                    if (!AnyAdjecent(elf)) continue;
                    FirstHalf(elf, i);
                }
                foreach (var pair in FromTo)
                {
                    if (ToCount[pair.Value] == 1)
                    {
                        Elves.Remove(pair.Key);
                        Elves.Add(pair.Value);
                    }
                }
                FromTo.Clear();
                ToCount.Clear();
                Print();
            }
            return MinRectangle() - Elves.Count;
        }

        private bool AnyAdjecent(Point elf)
        {
            for (int i = 0; i < 8; i++)
            {
                if (Elves.Contains(elf.NeighborV((Direction)i)))
                    return true;
            }
            return false;
        }

        public void FirstHalf(Point elf, int cmd)
        {
            for (int i = 0; i < 4; i++)
            {
                var dirs = Directions(cmd + i);
                var nb1 = elf.NeighborV(dirs[0]);
                var nb2 = elf.NeighborV(dirs[1]);
                var nb3 = elf.NeighborV(dirs[2]);
                if (!Elves.Contains(nb1) &&
                    !Elves.Contains(nb2) &&
                    !Elves.Contains(nb3))
                {
                    FromTo.Add(elf, nb1);
                    if (ToCount.ContainsKey(nb1)) ToCount[nb1]++;
                    else ToCount.Add(nb1, 1);
                    return;
                }
            }
        }
        public List<Direction> Directions(int cmd)
        {
            var result = (cmd % 4) switch
            {
                0 => new List<Direction>() { Direction.N, Direction.NE, Direction.NW },
                1 => new List<Direction>() { Direction.S, Direction.SE, Direction.SW },
                2 => new List<Direction>() { Direction.W, Direction.NW, Direction.SW },
                3 => new List<Direction>() { Direction.E, Direction.NE, Direction.SE },
                _ => throw new NotImplementedException()
            };
            //cmd = (cmd + 1) % 4;
            return result;
        }

        private int MinRectangle()
        {
            var x = Elves.OrderBy(elf => elf.X).ToList();
            var y = Elves.OrderBy(elf => elf.Y).ToList();
            return (x.Last().X - x.First().X + 1) * (y.Last().Y - y.First().Y + 1);
        }

        public static ElfRectangle Parse(string[] lines)
        {
            var set = new HashSet<Point>();
            for (int row = 0; row < lines.Length; row++)
            {
                var line = lines[row];
                for (int col = 0; col < line.Length; col++)
                {
                    var point = new Point(col, row);
                    if (line[col] == '#') set.Add(point);
                }
            }
            return new ElfRectangle(set, lines[0].Length, lines.Length);
        }

        public class Elf
        {
            public Point Point { get; set; }
        }
    }
}
