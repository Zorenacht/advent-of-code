part1();
part2();


string example = "Example.txt";
string filename = "Input.txt";
void part1() => Reactor(ParseInputBounded("Input.txt"));
void part2() => Reactor(ParseInput("Input.txt"));


void Reactor(IEnumerable<ReactorCuboid> cmds)
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
}


IEnumerable<ReactorCuboid> ParseInputBounded(string filename)
{
    string fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\", filename);
    var text = File.ReadLines(fileLocation);
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

IEnumerable<ReactorCuboid> ParseInput(string filename)
{
    string fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\", filename);
    var text = File.ReadLines(fileLocation);
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