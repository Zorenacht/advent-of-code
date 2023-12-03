using System.Text.RegularExpressions;

namespace AoC_2022;

public sealed partial class Day05 : Day
{
    [Test]
    public void Example()
    {
        const int NoContainers = 3;
        const int InitialHeight = 3;
        Parse(InputExample, InitialHeight, NoContainers, out var containers);
        Commands(InputExample, InitialHeight, containers, CrateMover);
        string result = new(containers.Select(c => c.Peek()).ToArray());
        result.Should().Be("CMZ");
    }

    [Test]
    public void Part1()
    {
        const int NoContainers = 9;
        const int InitialHeight = 8;
        Parse(InputPart1, InitialHeight, NoContainers, out var containers);
        Commands(InputPart1, InitialHeight, containers, CrateMover);
        string result = new(containers.Select(c => c.Peek()).ToArray());
        result.Should().Be("ZRLJGSCTR");
    }

    [Test]
    public void Part2()
    {
        const int NoContainers = 9;
        const int InitialHeight = 8;
        Parse(InputPart2, InitialHeight, NoContainers, out var containers);
        Commands(InputPart2, InitialHeight, containers, CrateMover9001);
        string result = new(containers.Select(c => c.Peek()).ToArray());
        result.Should().Be("PRTTGRFPB");
    }

    private void Parse(string[] input, int height, int width, out Stack<char>[] containers)
    {
        containers = Enumerable.Range(1, width).Select(i => new Stack<char>()).ToArray();
        for (int j = 0; j < width; j++)
        {
            for (int i = height - 1; i >= 0; i--)
            {
                var container = input[i][1 + 4 * j];
                if (container != ' ')
                {
                    containers[j].Push(container);
                }
            }
        }
    }

    [GeneratedRegex("move (\\d+) from (\\d+) to (\\d+)")]
    private static partial Regex CommandRegex();

    private void Commands(string[] input, int height, Stack<char>[] containers, Action<int, int, int, Stack<char>[]> crateMover)
    {
        foreach (string line in input.Skip(height + 2))
        {
            var order = CommandRegex().Match(line).Groups;
            int amount = int.Parse(order[1].Value);
            int from = int.Parse(order[2].Value) - 1;
            int to = int.Parse(order[3].Value) - 1;
            crateMover(amount, from, to, containers);
        }
    }

    private void CrateMover(int amount, int from, int to, Stack<char>[] containers)
    {
        for (int i = 0; i < amount; i++)
        {
            containers[to].Push(containers[from].Pop());
        }
    }

    private void CrateMover9001(int amount, int from, int to, Stack<char>[] containers)
    {
        var temp = new Stack<char>();
        for (int i = 0; i < amount; i++)
        {
            temp.Push(containers[from].Pop());
        }
        for (int i = 0; i < amount; i++)
        {
            containers[to].Push(temp.Pop());
        }
    }

}