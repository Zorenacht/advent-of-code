using System.Numerics;

namespace AoC_2023;

public sealed class Day18 : Day
{
    [Puzzle(answer: 62)]
    public long Part1Example() => new LavaDigger(InputExample).Part1();

    [Puzzle(answer: 52055)]
    public long Part1() => new LavaDigger(Input).Part1();

    [Puzzle(answer: 952408144115)]
    public long Part2Example() => new LavaDigger(InputExample).Part2();

    [Puzzle(answer: 67622758357096)]
    public long Part2() => new LavaDigger(Input).Part2();


    private class LavaDigger
    {
        private readonly List<Command> Part1Commands;
        private readonly List<Command> Part2Commands;

        public LavaDigger(string[] input)
        {
            Part1Commands = CommandsPart1(input);
            Part2Commands = CommandsPart2(input);
        }

        public long Part1() => Solve(Part1Commands);
        public long Part2() => Solve(Part2Commands);

        private record Command(int Steps, Complex Dir);

        private Complex Dir(string c) => c switch
        {
            "U" => Complex.ImaginaryOne,
            "R" => Complex.One,
            "D" => -Complex.ImaginaryOne,
            "L" => -Complex.One,
            _ => throw new Exception()
        };

        private long Solve(List<Command> commands)
        {
            //create grid with reduced coordinates
            var location = Complex.Zero;
            var horizontal = new HashSet<int>() { 0 };
            var vertical = new HashSet<int>() { 0 };
            foreach (var cmd in commands)
            {
                location += cmd.Steps * cmd.Dir;
                horizontal.Add((int)location.Real);
                vertical.Add((int)location.Imaginary);
            }
            var hValues = horizontal.ToList().OrderBy(x => x).ToList().SelectMany((x, i) => i < horizontal.Count - 1 ? new[] { x, int.MaxValue } : new[] { x }).ToList();
            var vValues = vertical.ToList().OrderBy(x => x).ToList().SelectMany((x, i) => i < vertical.Count - 1 ? new[] { x, int.MaxValue } : new[] { x }).ToList();

            var grid = Enumerable.Repeat(false, vValues.Count).Select(x => Enumerable.Repeat(false, hValues.Count).ToArray()).ToArray();

            //grid with borders
            var border = new HashSet<Complex>() { };
            int rowIndex = vValues.IndexOf(0);
            int colIndex = hValues.IndexOf(0);
            foreach (var cmd in commands)
            {
                location += cmd.Steps * cmd.Dir;
                if (cmd.Dir.Real != 0)
                {
                    while (hValues[colIndex] != location.Real)
                    {
                        colIndex += Math.Sign(cmd.Dir.Real);
                        border.Add(new Complex(colIndex, rowIndex));
                        grid[rowIndex][colIndex] = true;
                    }
                }
                if (cmd.Dir.Imaginary != 0)
                {
                    while (vValues[rowIndex] != location.Imaginary)
                    {
                        rowIndex += Math.Sign(cmd.Dir.Imaginary);
                        border.Add(new Complex(colIndex, rowIndex));
                        grid[rowIndex][colIndex] = true;
                    }
                }
            }

            //flood fill
            var outside = new HashSet<Complex>() { Complex.Zero };
            var inside = new HashSet<Complex>() { };
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[0].Length; j++)
                {
                    var loc = new Complex(j, i);
                    if (border.Contains(loc) || outside.Contains(loc) || inside.Contains(loc)) continue;
                    var que = new Queue<Complex>(); que.Enqueue(loc);
                    bool foundOutside = false;
                    var flood = new HashSet<Complex>();
                    while (que.Count > 0)
                    {
                        var ele = que.Dequeue();
                        if (flood.Contains(ele)) continue;
                        foreach (var ch in "URDL")
                        {
                            var temp = ele + Dir(new string(ch, 1));
                            if (!(temp.Real >= 0 && temp.Real < grid[0].Length && temp.Imaginary >= 0 && temp.Imaginary < grid.Length)) foundOutside = true;
                            else if (!border.Contains(temp) && !flood.Contains(temp))
                            {
                                que.Enqueue(temp);
                            }
                        }
                        flood.Add(ele);
                    }
                    if (foundOutside) outside.UnionWith(flood);
                    else
                    {
                        foreach (var ele in flood) grid[(int)ele.Imaginary][(int)ele.Real] = true;
                        inside.UnionWith(flood);
                    }
                }
            }

            //count interior
            long interiorCount = 0;
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[0].Length; j++)
                {
                    var loc = new Complex(j, i);
                    if (!inside.Contains(loc)) continue;
                    long verticalLength = vValues[i] == int.MaxValue ? vValues[i + 1] - vValues[i - 1] - 1 : 1;
                    long horizontalLength = hValues[j] == int.MaxValue ? hValues[j + 1] - hValues[j - 1] - 1 : 1;
                    interiorCount += horizontalLength * verticalLength;
                }
            }
            long borderCount = commands.Sum(x => x.Steps);
            return interiorCount + borderCount;
        }


        private List<Command> CommandsPart1(string[] input)
            => input.Select(x =>
            {
                var split = x.Split();
                return new Command(int.Parse(split[1]), Dir(split[0]));
            }).ToList();

        private List<Command> CommandsPart2(string[] input)
            => input.Select(x =>
            {
                var split = x.Split()[2][1..^1];
                Complex dir = Complex.One;
                for (int i = 0; i < split[^1] - '0'; i++)
                {
                    dir *= Complex.ImaginaryOne;
                }
                return new Command(Convert.ToInt32(split[1..^1], 16), dir);
            }).ToList();
    }
}