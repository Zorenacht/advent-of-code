namespace AoC_2021;

public sealed class Day22 : Day
{
    [Puzzle(answer: 655005)]
    public long Part1()
    {
        return Reactor(ParseInputBounded(Input));
    }

    [Puzzle(answer: 1125649856443608)]
    public long Part2()
    {
        return Reactor(ParseInput(Input));
    }

    long Reactor(IEnumerable<ReactorCuboid> cmds)
    {
        var cuboids = new List<ReactorCuboid>();
        foreach (var cmd in cmds)
        {
            if (cmd.IsValid)
            {
                var intersections = new List<ReactorCuboid>();
                foreach (var cuboid in cuboids)
                {
                    var intersection = cuboid.Intersection(cmd);
                    if (intersection is not null)
                        intersections.Add(intersection);
                }
                cuboids.AddRange(intersections);
                if (cmd.On)
                {
                    cuboids.Add(cmd);
                }
            }
        }
        var a = cuboids.ToList().OrderByDescending(x => x.Count).Take(100).ToList();
        long count = 0;
        foreach (var cuboid in cuboids)
        {
            count += (cuboid.On ? 1 : -1) * cuboid.Count;
        }
        Console.WriteLine($"Count: {count}");
        return count;
    }

    IEnumerable<ReactorCuboid> ParseInputBounded(string[] input)
    {
        var text = input;
        string[] separators = new string[] { " x=", "..", ",y=", ",z=" };
        foreach (string line in text)
        {
            string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            yield return new ReactorCuboid(
                words[0] == "on",
                Math.Max(int.Parse(words[1]), -50),
                Math.Min(int.Parse(words[2]), 50),
                Math.Max(int.Parse(words[3]), -50),
                Math.Min(int.Parse(words[4]), 50),
                Math.Max(int.Parse(words[5]), -50),
                Math.Min(int.Parse(words[6]), 50));
        }
    }

    IEnumerable<ReactorCuboid> ParseInput(string[] input)
    {
        var text = input;
        string[] separators = new string[] { " x=", "..", ",y=", ",z=" };
        foreach (string line in text)
        {
            string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            yield return new ReactorCuboid(
                words[0] == "on",
                int.Parse(words[1]),
                int.Parse(words[2]),
                int.Parse(words[3]),
                int.Parse(words[4]),
                int.Parse(words[5]),
                int.Parse(words[6]));
        }
    }

    public record ReactorCuboid(bool On, int x1, int x2, int y1, int y2, int z1, int z2)
    {
        public long Count => (long)(x2 - x1 + 1) * (y2 - y1 + 1) * (z2 - z1 + 1);
        public bool IsValid => x1 <= x2 && y1 <= y2 && z1 <= z2;

        public ReactorCuboid? Intersection(ReactorCuboid newCuboid)
        {
            var ix1 = Math.Max(x1, newCuboid.x1);
            var ix2 = Math.Min(x2, newCuboid.x2);
            var iy1 = Math.Max(y1, newCuboid.y1);
            var iy2 = Math.Min(y2, newCuboid.y2);
            var iz1 = Math.Max(z1, newCuboid.z1);
            var iz2 = Math.Min(z2, newCuboid.z2);
            var on = !On; //Only add on cuboid current one is off
            if (ix1 <= ix2 && iy1 <= iy2 && iz1 <= iz2)
            {
                return new ReactorCuboid(on, ix1, ix2, iy1, iy2, iz1, iz2);
            }
            return null;
        }
    };

}