namespace AoC._2022;


public sealed partial class Day18 : Day
{
    [Test]
    public void Example() => LavaDroplets.Parse(InputExample).SurfaceArea().Should().Be(64);
    [Test]
    public void Part1() => LavaDroplets.Parse(InputPart1).SurfaceArea().Should().Be(4456);

    [Test]
    public void ExampleP2() => LavaDroplets.Parse(InputExample).DetermineInternal().SurfaceArea().Should().Be(58);
    [Test]
    public void Part2() => LavaDroplets.Parse(InputPart1).DetermineInternal().SurfaceArea().Should().Be(2510);

    private class LavaDroplets
    {
        private readonly HashSet<Cube> Droplets;
        private readonly HashSet<Cube> Enclosed;
        private readonly HashSet<Cube> Outside;

        public LavaDroplets(HashSet<Cube> droplets)
        {
            Droplets = droplets;
            Enclosed = new HashSet<Cube>();
            Outside = new HashSet<Cube>();
        }

        public int SurfaceArea()
        {
            int count = 0;
            foreach (var cube in Droplets)
            {
                foreach (var nb in cube.GetNeighbors)
                {
                    if (!Enclosed.Contains(nb) && !Droplets.Contains(nb))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public LavaDroplets DetermineInternal()
        {
            //Some point on the exterior
            var point = Droplets.MaxBy(x => x.X)!;
            var goal = new Cube(point.X + 1, point.Y, point.Z);
            foreach (var cube in Droplets)
            {
                foreach (var nb in cube.GetNeighbors)
                {
                    if (Droplets.Contains(nb) || Outside.Contains(nb) || Enclosed.Contains(nb))
                    {
                        continue;
                    }
                    BFS(nb, goal);
                }
            }
            return this;
        }

        private void BFS(Cube nb, Cube goal)
        {
            var queue = new Queue<Cube>();
            var explored = new HashSet<Cube>();
            queue.Enqueue(nb);
            while (queue.TryDequeue(out var current))
            {
                if (explored.Contains(current) || Droplets.Contains(current))
                {
                    continue;
                }
                if (current == goal || Outside.Contains(current))
                {
                    Outside.UnionWith(explored);
                    return;
                }
                if (Enclosed.Contains(current))
                {
                    Enclosed.UnionWith(explored);
                    return;
                }
                foreach (var next in current.GetNeighbors)
                {
                    if (!explored.Contains(next))
                    {
                        queue.Enqueue(next);
                    }
                }
                explored.Add(current);
            }
            Enclosed.UnionWith(explored);
        }

        public static LavaDroplets Parse(string[] lines)
        {
            var set = new HashSet<Cube>();
            foreach (var line in lines)
            {
                var split = line.Split(',');
                var cube = new Cube(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
                set.Add(cube);
            }
            return new LavaDroplets(set);
        }
    }

    private record Cube(int X, int Y, int Z)
    {
        public IEnumerable<Cube> GetNeighbors
        {
            get
            {
                yield return new Cube(X + 1, Y, Z);
                yield return new Cube(X - 1, Y, Z);
                yield return new Cube(X, Y + 1, Z);
                yield return new Cube(X, Y - 1, Z);
                yield return new Cube(X, Y, Z + 1);
                yield return new Cube(X, Y, Z - 1);
            }
        }
    }
}
