namespace AoC_2022;

public sealed partial class Day10 : Day
{
    [Test]
    public void Example() => CRT.Parse(InputExample).SignalStrength().Should().Be(13140);

    [Test]
    public void Part1() => CRT.Parse(InputPart1).SignalStrength().Should().Be(15020);

    [Test]
    public void ExampleP2() => CRT.Parse(InputExample).Display().Should().Be(ExampleAnswer);
    [Test]
    public void Part2() => CRT.Parse(InputPart1).Display().Should().Be(EFLUGLPAP);

    static string ExampleAnswer => 
@"##..##..##..##..##..##..##..##..##..##..
###...###...###...###...###...###...###.
####....####....####....####....####....
#####.....#####.....#####.....#####.....
######......######......######......####
#######.......#######.......#######.....";

    static string EFLUGLPAP =>
@"####.####.#..#..##..#....###...##..###.#
.....#....#..#.#..#.#....#..#.#..#.#..#.
###..###..#..#.#....#....#..#.#..#.#..#.
#....#....#..#.#.##.#....###..####.###..
.....#....#..#.#..#.#....#....#..#.#....
####.#.....##...###.####.#....#..#.#....";


    private class CRT
    { 
        private Queue<Cmd> Cmds { get; set; }
        private CRT(Queue<Cmd> cmds) => Cmds = cmds;

        public static CRT Parse(string[] input)
        {
            var cmds = new Queue<Cmd>();
            foreach (var line in input)
            {
                if (line[..4] == "noop")
                {
                    cmds.Enqueue(new Cmd(0));
                }
                else
                {
                    cmds.Enqueue(new Cmd(0));
                    cmds.Enqueue(new Cmd(int.Parse(line[5..])));
                }
            }
            return new CRT(cmds);
        }

        public int SignalStrength()
        {
            int result = 0;
            int value = 1;
            int cycle = 1;
            while (Cmds.Any())
            {
                if ((cycle + 20) % 40 == 0)
                {
                    result += value * cycle;
                }

                value += Cmds.Dequeue().Value;
                cycle++;
            }
            return result;
        }


        public string Display()
        {
            char[][] image = new char[6].Select(_ => new char[40]).ToArray();
            int value = 1;
            int cycle = 1;
            while (Cmds.Any())
            {
                int cycleRow = (cycle - 1)/40;
                int cycleCol = Modulo(cycle - 1, 40);
                int spriteCol = Modulo(value, 40);
                if (SpriteOverlap(cycleCol, spriteCol))
                {
                    image[cycleRow][cycleCol] = '#';
                }
                else image[cycleRow][cycleCol] = '.';

                value += Cmds.Dequeue().Value;
                cycle++;
            }
            var result = string.Join("\r\n", image.Select(x => new string(x)));
            return result;
        }

        private static bool SpriteOverlap(int pixel, int sprite) =>
            pixel >= sprite - 1 && pixel <= sprite + 1;

        public record Cmd(int Value);

        public static int Modulo(int a, int b)
        {
            int remainder = a % b;
            return remainder >= 0 ? remainder : remainder + b;
        }
    }
}