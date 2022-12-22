using MathNet.Numerics.Providers.LinearAlgebra;
using Tools.Geometry;

namespace AoC_2022;


public sealed partial class Day22 : Day
{
    [Test]
    public void Example() => Maze.Parse(InputExample).Should().Be(-100);
    [Test]
    public void Part1() => Maze.Parse(InputPart1).Should().Be(-100);

    [Test]
    public void ExampleP2() => Maze.Parse(InputPart1).Should().Be(-100);
    [Test]
    public void Part2() => Maze.Parse(InputPart1).Should().Be(-100);


    private class Maze
    {
        public Maze()
        {
        }

        public static Maze Parse(string[] lines)
        {
            var mazeLines = lines.Take(lines.Length - 2).ToArray();
            var pathLines = lines.Skip(lines.Length - 1).ToArray();
            LinkedPoint start;
            for (int row = 0; row <= lines.Length; row++)
            {
                var line = lines[row];
                for (int col = 0; col <= line.Length; col++)
                {
                    
                    var point = new LinkedPoint() {  }
                    //if(start is null) 
                }
            }
            return new Maze();
        }
    }

    private class LinkedPoint
    {
        public bool Blocked { get; set; }
        public Point Location { get; set; }
        public Dictionary<Direction, LinkedPoint> Neighbors { get; set; } = new Dictionary<Direction, LinkedPoint>();
        public LinkedPoint Neightbor(Direction direction) => Neighbors[direction];
    }
}
