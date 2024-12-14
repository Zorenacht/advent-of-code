using Tools.Geometry;

namespace AoC._2024;

public sealed class Day14 : Day
{
    [Puzzle(answer: 218619120)]
    public long Part1() => new RobotIterator(103, 101, Input).SafetyFactor();

    [Puzzle(answer: 7055)]
    public long Part2() => new RobotIterator(103, 101, Input).IndexOfBiggestArea();

    private class RobotIterator
    {
        public int Height { get; }
        public int Width { get; }
        public List<Robot> Robots { get; } = [];

        public RobotIterator(
            int height,
            int width,
            string[] lines)
        {
            Height = height;
            Width = width;
            Robots = new List<Robot>();
            foreach (var line in lines)
            {
                var splitted = line.Split("= ,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var index = new Index2D(int.Parse(splitted[2]), int.Parse(splitted[1]));
                var velocity = new Index2D(int.Parse(splitted[5]), int.Parse(splitted[4]));
                var robot = new Robot(index, velocity);
                Robots.Add(robot);
            }
        }

        public int IndexOfBiggestArea()
        {
            var biggestArea = new List<int>() { -100 };
            for (int i = 1; i <= 101 * 103; ++i)
            {
                var positions = new HashSet<Index2D>();
                foreach (var robot in Robots)
                {
                    robot.Current = new Index2D(
                        (robot.Current.Row + robot.Velocity.Row).Modulo(Height),
                        (robot.Current.Col + robot.Velocity.Col).Modulo(Width));
                    positions.Add(robot.Current);
                }

                var areas = new HashSet<Area>();
                var visited = new HashSet<Index2D>();
                foreach (var robot in Robots)
                {
                    if (visited.Contains(robot.Current)) continue;
                    var area = new Area();
                    var queue = new Queue<Index2D>();
                    queue.Enqueue(robot.Current);
                    while (queue.TryDequeue(out var current))
                    {
                        if (!visited.Add(current)) continue;
                        foreach (var dir in Directions.CardinalIndex)
                        {
                            var nb = current + dir;
                            if (positions.Contains(nb) && !visited.Contains(nb))
                            {
                                queue.Enqueue(nb);
                            }
                        }
                        area.Add(current);
                    }
                    areas.Add(area);
                }
                biggestArea.Add(areas.Max(x => x.Count));
            }
            return biggestArea.IndexOf(biggestArea.Max());
        }

        public long SafetyFactor()
        {
            var grid = new Grid<int>(Height, Width);
            for (int i = 0; i < 100; ++i)
            {
                foreach (var robot in Robots)
                {
                    robot.Current = new Index2D(
                        (robot.Current.Row + robot.Velocity.Row).Modulo(Height),
                        (robot.Current.Col + robot.Velocity.Col).Modulo(Width));
                }
            }
            var quadrants = new long[4] { 0, 0, 0, 0 };
            foreach (var robot in Robots)
            {
                grid.UpdateAt(robot.Current, grid.ValueAt(robot.Current) + 1);
                if (robot.Current.Row < Height / 2 && robot.Current.Col < Width / 2) quadrants[0]++;
                if (robot.Current.Row < Height / 2 && robot.Current.Col > Width / 2) quadrants[1]++;
                if (robot.Current.Row > Height / 2 && robot.Current.Col < Width / 2) quadrants[2]++;
                if (robot.Current.Row > Height / 2 && robot.Current.Col > Width / 2) quadrants[3]++;
            }
            return quadrants[0] * quadrants[1] * quadrants[2] * quadrants[3];
        }

        public class Robot(Index2D start, Index2D velocity)
        {
            public Index2D Start { get; } = start;
            public Index2D Velocity { get; } = velocity;
            public Index2D Current { get; set; } = start;

            public override bool Equals(object? obj) => obj is Robot other && Current.Equals(other.Current);
            public override int GetHashCode() => Current.GetHashCode();
        }
    }
}
