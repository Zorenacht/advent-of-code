using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Tools.Geometry;

namespace AoC_2022;


public sealed partial class Day23 : Day
{
    [Test]
    public void Example() => ElfRectangle.Parse(InputExample).Simulate().Should().Be(6032);
    [Test]
    public void Part1() => ElfRectangle.Parse(InputPart1).Simulate().Should().Be(57350);

    [Test]
    public void ExampleP2() => ElfRectangle.Parse(InputPart1).Simulate().Should().BeGreaterThan(-100);
    [Test]
    public void Part2() => ElfRectangle.Parse(InputPart1).Simulate().Should().Be(-100);



    private class ElfRectangle
    {
        public readonly Dictionary<Point, Elf> Elves;
        public readonly Dictionary<Point, Point> FromTo;
        public readonly Dictionary<Point, int> ToCount;

        public ElfRectangle(Dictionary<Point, Elf> elves)
        {
            Elves = elves;
            FromTo = new Dictionary<Point,Point>();
            ToCount = new Dictionary<Point, int>();
        }

        public int Simulate()
        {
            for (int i = 0; i < 10; i++)
            {
                foreach (var pair in Elves)
                {
                    FirstHalf(pair.Value);
                }
                foreach(var pair in FromTo)
                {
                    if(ToCount[pair.Value] == 1)
                    {
                        var elf = Elves[pair.Key];
                        Elves.Remove(pair.Key);
                         
                        elf.Point = pair.Value;
                        Elves.Add(pair.Value, elf);
                    }
                }
                FromTo.Clear();
                ToCount.Clear();
            }
            return MinRectangle();
        }

        public void FirstHalf(Elf elf)
        {
            for (int i = 0; i < 4; i++)
            {
                foreach (var dir in elf.Directions())
                {
                    var nb = elf.Point.NeighborV(dir);
                    if (!Elves.ContainsKey(nb))
                    {
                        FromTo.Add(elf.Point, nb);
                        if (ToCount.ContainsKey(nb)) ToCount[nb]++;
                        else ToCount.Add(nb, 1);
                        return;
                    }
                }
            }
        }

        private int MinRectangle()
        {
            var x = Elves.OrderBy(elf => elf.Key.X).ToList();
            var y = Elves.OrderBy(elf => elf.Key.Y).ToList();
            return (x.Last().Key.X - x.First().Key.X + 1) * (y.Last().Key.Y - y.First().Key.Y + 1);
        }

        public static ElfRectangle Parse(string[] lines)
        {
            var dict = new Dictionary<Point, Elf>();
            for (int row = 0; row < lines.Length; row++)
            {
                var line = lines[row];
                for (int col = 0; col < line.Length; col++)
                {
                    var point = new Point(col + 1, row + 1);
                    if (line[col] == '#') dict.Add(point, new Elf() { Point = point });
                }
            }
            return new ElfRectangle(dict);
        }

        public class Elf
        {
            public Point Point { get; set; }
            public int Cmd { get; set; }
            public List<Direction> Directions()
            {
                var result = Cmd switch
                {
                    0 => new List<Direction>() { Direction.N, Direction.NE, Direction.NW },
                    1 => new List<Direction>() { Direction.S, Direction.SE, Direction.SW },
                    2 => new List<Direction>() { Direction.W, Direction.NW, Direction.SW },
                    3 => new List<Direction>() { Direction.E, Direction.NE, Direction.SE },
                    _ => throw new NotImplementedException()
                };
                Cmd = (Cmd + 1) % 4;
                return result;
            }

            public int IncreaseMod(int cmd)
            {
                int original = cmd;
                cmd = (cmd + 1) % 4;
                return original;
            }
        }
    }
}
