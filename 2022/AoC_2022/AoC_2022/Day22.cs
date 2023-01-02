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
    public void Part2() => CubeMaze.Parse(InputPart1).Simulate().Should().Be(104385);


    private class CubeMaze : Maze
    {
        public CubeMaze(Dictionary<Point, LinkedPoint> points, List<Cmd> cmds) : base(points, cmds)
        {
        }

        protected override void Connect()
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
                var block = PointToBlock(point.Point);
                var rel = point.Point - 50 * block;

                foreach (var dir in Directions)
                {
                    if (!Points.TryGetValue(point.Point.NeighborV(dir), out var nb))
                    {
                        var nbBorder = BlockChange(new BlockBorder(block, dir));

                        Point border;
                        if (DirHelpers.RightAngle(dir, nbBorder.Direction))
                        {
                            border = new Point(rel.Y, rel.X);
                        }
                        else if (DirHelpers.OppositeAngle(dir, nbBorder.Direction))
                        {
                            border = new Point(rel.X, 51 - rel.Y);
                        }
                        else if (dir == nbBorder.Direction && dir == Direction.N)
                        {
                            border = rel + new Point(0, 49);
                        }
                        else if (dir == nbBorder.Direction && dir == Direction.S)
                        {
                            border = rel - new Point(0, 49);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }

                        var abs = border + 50 * nbBorder.Block;
                        point.Neighbors.Add(dir, Points[abs]);
                    }
                }
            }
        }

        protected override LinkedPoint Move(LinkedPoint point, ref Direction dir, int times)
        {
            var current = point;
            for (int i = 0; i < times; i++)
            {
                var oldBlock = PointToBlock(current.Point);
                var oldBlockBorder = new BlockBorder(oldBlock, dir);
                if (!current.TryMove(dir, out current)) break;
                var newBlock = PointToBlock(current.Point);
                if (oldBlock != newBlock && oldBlockBorder != BlockChange(oldBlockBorder))
                {
                    dir = BlockChange(oldBlockBorder).Direction;
                }
            }
            return current;
        }

        private static Point PointToBlock(Point point) => new((point.X - 1) / 50, (point.Y - 1) / 50);

        public record BlockBorder(Point Block, Direction Direction);
        public static BlockBorder BlockChange(BlockBorder block)
        {
            if (block.Block == new Point(1, 0) && block.Direction == Direction.N) return new BlockBorder(new Point(0, 3), Direction.E);
            if (block.Block == new Point(0, 3) && block.Direction == Direction.W) return new BlockBorder(new Point(1, 0), Direction.S);

            if (block.Block == new Point(1, 0) && block.Direction == Direction.W) return new BlockBorder(new Point(0, 2), Direction.E);
            if (block.Block == new Point(0, 2) && block.Direction == Direction.W) return new BlockBorder(new Point(1, 0), Direction.E);

            if (block.Block == new Point(1, 1) && block.Direction == Direction.W) return new BlockBorder(new Point(0, 2), Direction.S);
            if (block.Block == new Point(0, 2) && block.Direction == Direction.N) return new BlockBorder(new Point(1, 1), Direction.E);

            if (block.Block == new Point(1, 1) && block.Direction == Direction.E) return new BlockBorder(new Point(2, 0), Direction.N);
            if (block.Block == new Point(2, 0) && block.Direction == Direction.S) return new BlockBorder(new Point(1, 1), Direction.W);

            if (block.Block == new Point(2, 0) && block.Direction == Direction.E) return new BlockBorder(new Point(1, 2), Direction.W);
            if (block.Block == new Point(1, 2) && block.Direction == Direction.E) return new BlockBorder(new Point(2, 0), Direction.W);

            if (block.Block == new Point(2, 0) && block.Direction == Direction.N) return new BlockBorder(new Point(0, 3), Direction.N);
            if (block.Block == new Point(0, 3) && block.Direction == Direction.S) return new BlockBorder(new Point(2, 0), Direction.S);

            if (block.Block == new Point(0, 3) && block.Direction == Direction.E) return new BlockBorder(new Point(1, 2), Direction.N);
            if (block.Block == new Point(1, 2) && block.Direction == Direction.S) return new BlockBorder(new Point(0, 3), Direction.W);

            return block;
        }

        public static CubeMaze Parse(string[] lines)
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
            return new CubeMaze(points, cmds);
        }
    }


    private partial class Maze
    {
        public readonly Dictionary<Point, LinkedPoint> Points;
        public readonly List<Cmd> Cmds;

        public Maze(Dictionary<Point, LinkedPoint> points, List<Cmd> cmds)
        {
            Points = points;
            Cmds = cmds;
            Connect();
        }

        public int Simulate()
        {
            var start = Points.Where(x => x.Key.Y == 1).OrderBy(x => x.Key.Y).First().Value!;
            var currentPos = start;
            var currentDir = Direction.E;
            var cmds = new List<Cmd>() { new Turn("L"), new Turn("L"), new Move(10000) };
            foreach (var cmd in Cmds)
            {
                if (cmd is Turn turn) currentDir = Turn(currentDir, turn.Dir);
                if (cmd is Move move) currentPos = Move(currentPos, ref currentDir, move.Times);
            }
            return 1000 * currentPos.Point.Y + 4 * currentPos.Point.X + DirectionValue(currentDir);
        }

        private static int DirectionValue(Direction dir) => dir switch
        {
            Direction.E => 0,
            Direction.S => 1,
            Direction.W => 2,
            Direction.N => 3,
            _ => throw new NotSupportedException()
        };

        protected static IEnumerable<Direction> Directions => new Direction[] { Direction.N, Direction.W, Direction.E, Direction.S };
        private static Direction Turn(Direction dir, string turn) => turn switch
        {
            "L" => (Direction)(((int)dir + 2) % 8),
            "R" => (Direction)(((int)dir + 6) % 8),
            _ => (Direction)(((int)dir + 4) % 8),
        };

        protected virtual LinkedPoint Move(LinkedPoint point, ref Direction dir, int times)
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
            while (true)
            {
                if (!current.TryMoveToCycle(dir, point, out current)) break;
            }
            return current;
        }

        protected virtual void Connect()
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
            return new Maze(points, cmds);
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
