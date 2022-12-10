using System.Runtime.CompilerServices;
using System.Text;
using static AoC_2022.Day10;

namespace AoC_2022;

public sealed partial class Day10 : Day
{
    [Test]
    public void Example() => P1(Simulate(InputExample)).Should().Be(13140);

    [Test]
    public void Part1() => P1(Simulate(InputPart1)).Should().Be(15020);

    [Test]
    public void ExampleP2() => P2(Simulate(InputExample)).Should().Be(ExampleAnswer);
    [Test]
    public void Part2() => P2(Simulate(InputPart1)).Should().Be(EFLUGLPAP);

    static string ExampleAnswer => @"
        ##..##..##..##..##..##..##..##..##..##..
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

    private static Queue<Cmd> Simulate(string[] input)
    {
        var cmds = new Queue<Cmd>();
        foreach (string line in input)
        {
            var cmd = line[..4];
            int arg = 0;
            int turns = 1;
            if (cmd == "addx") {
                arg = int.Parse(line[5..]);
                turns = 2;
            }
            cmds.Enqueue(new Cmd() { Turns = turns, Value = arg });
        }
        return cmds;
    }

    private static int P1(Queue<Cmd> cmds)
    {
        int result = 0;
        int value = 1;
        int cycle = 1;
        while (cmds.Any())
        {
            if ((cycle + 20) % 40 == 0) result += value * cycle;

            var current = cmds.Peek();
            if (current.Turns == 1)
            {
                cmds.Dequeue();
                value += current.Value;
            }
            else current.Turns--;



            cycle++;
        }
        return result;
    }

    private static string P2(Queue<Cmd> cmds)
    {
        char[] image = new char[240];
        int value = 1;
        int cycle = 1;
        while (cmds.Any())
        {
            int cycleRowIndex = Modulo(cycle - 1, 40);
            int spriteRowIndex = Modulo(value, 40);
            if (SpriteOverlap(cycleRowIndex, spriteRowIndex))
            {
                image[cycle - 1] = '#';
            }
            else image[cycle - 1] = '.';



            var current = cmds.Peek();
            if (current.Turns == 1)
            {
                cmds.Dequeue();
                value += current.Value;
            }
            else current.Turns--;
            cycle++;
        }
        return new string(image);
    }

    /*private static string CharArrayToString(char[] chars, int lb)
    {
        var sb = new StringBuilder();
        *//*for(int i=0; i<chars.Length; i += lb) 
        {
            sb.Append(chars[i..i)])
        }*//*
    }*/

    private static bool SpriteOverlap(int pixel, int sprite) =>
        pixel >= sprite - 1 && pixel <= sprite + 1;

    public class Cmd {
        public int Turns { get; set; }
        public int Value { get; set; }
    }; 
    
    public static int Modulo(int a, int b)
    {
        int remainder = a % b;
        return remainder >= 0 ? remainder : remainder + b;
    }
}