using System.Text.RegularExpressions;
using Tools.Geometry;

namespace AoC_2022;


public sealed partial class Day22 : Day
{
    [Test]
    public void Example() => Maze.Parse(InputExample).Simulate().Should().Be(6032);
    [Test]
    public void Part1() => Maze.Parse(InputPart1).Simulate().Should().Be(57350);

    [Test]
    public void ExampleP2() => Maze.Parse(InputPart1).Simulate().Should().BeGreaterThan(-100);
    [Test]
    public void Part2() => Maze.Parse(InputPart1).Simulate().Should().Be(-100);



    private class Maze
    {
        public readonly Dictionary<Point, LinkedPoint> Points;
        public readonly List<Cmd> Cmds;

        public Maze(Dictionary<Point, LinkedPoint> points, List<Cmd> cmds)
        {
            Points = points;
            Cmds = cmds;
        }

        public int Simulate()
        {
            var start = Points.Where(x => x.Key.Y == 1).OrderBy(x => x.Key.Y).First().Value!;
            var currentPos = start;
            var currentDir = Direction.E;
            foreach (var cmd in Cmds)
            {
                if (cmd is Turn turn) currentDir = Turn(currentDir, turn.Dir);
                if (cmd is Move move) currentPos = Move(currentPos, currentDir, move.Times);
            }
            return 1000 * currentPos.Point.Y + 4 * currentPos.Point.X + DirectionValue(currentDir);
        }

        private static int DirectionValue(Direction dir) => dir switch
        {
            Direction.E => 0,
            Direction.S => 1,
            Direction.W => 2,
            Direction.N => 3,
        };

        private static IEnumerable<Direction> Directions => new Direction[] { Direction.N, Direction.W, Direction.E, Direction.S };
        private static Direction Turn(Direction dir, string turn) => turn switch
        {
            "L" => (Direction)(((int)dir + 2) % 8),
            "R" => (Direction)(((int)dir + 6) % 8),
            _ => (Direction)(((int)dir + 4) % 8),
        };

        private LinkedPoint Move(LinkedPoint point, Direction dir, int times)
        {
            var current = point;
            for (int i = 0; i < times; i++)
            {
                if (!current.TryMove(dir, out current)) break;
            }
            return current;
        }

        private LinkedPoint FindCircular(LinkedPoint point, Direction dir)
        {
            var current = point;
            while(true)
            {
                if (!current.TryMoveToCycle(dir, point, out current)) break;
            }
            return current;
        }

        private void Connect()
        {
            foreach (var point in Points.Select(x => x.Value))
            {
                foreach (var dir in Directions)
                {
                    if (Points.TryGetValue(point.Point.NeighborV(dir), out var nb))
                    {
                        point.Neighbors.Add(dir, nb);
                    }
                }
            }
            foreach (var point in Points.Select(x => x.Value))
            {
                foreach (var dir in Directions)
                {
                    if (!Points.TryGetValue(point.Point.NeighborV(dir), out var nb))
                    {
                        point.Neighbors.Add(dir, FindCircular(point, Turn(dir, "")));
                    }
                }
            }
        }

        private void ConnectCube()
        {

        }

        public static Maze Parse(string[] lines)
        {
            var mazeLines = lines.Take(lines.Length - 2).ToArray();
            var pathLine = lines.Last();
            var points = new Dictionary<Point, LinkedPoint>();
            for (int row = 0; row < lines.Length - 2; row++)
            {
                var line = lines[row];
                for (int col = 0; col < line.Length; col++)
                {
                    var point = new Point(col + 1, row + 1);
                    var linkedPoint = new LinkedPoint() { Point = point, Type = line[col] };
                    if (line[col] != ' ') points.Add(point, linkedPoint);
                }
            }
            var regex = Regex.Matches(pathLine, @"(\d+)|([RL])");
            var cmds = regex.Select(r => (Cmd)(
                int.TryParse(r.Groups[1].Value, out int cmd)
                    ? new Move(cmd)
                    : new Turn(r.Groups[2].Value))).ToList();
            var maze = new Maze(points, cmds);
            maze.Connect();
            return maze;
        }
    }

    private record Cmd();
    private record Move(int Times) : Cmd;
    private record Turn(string Dir) : Cmd;

    private class LinkedPoint
    {
        public char Type { get; set; }
        public Point Point { get; set; }
        public Dictionary<Direction, LinkedPoint> Neighbors { get; set; } = new Dictionary<Direction, LinkedPoint>();

        public bool TryMoveToCycle(Direction dir, LinkedPoint original, out LinkedPoint point)
        {
            if (Neighbors.TryGetValue(dir, out var newPoint) && newPoint != original)
            {
                point = newPoint;
                return true;
            }
            point = this;
            return false;
        }

        public bool TryMove(Direction dir, out LinkedPoint point)
        {
            if (Neighbors.TryGetValue(dir, out var newPoint) && newPoint.Type == '.')
            {
                point = newPoint;
                return true;
            }
            point = this;
            return false;
        }
    }
}
