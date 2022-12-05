using System.Text.RegularExpressions;

namespace AoC_2022;

public sealed class Day05 : Day
{
    [Test]
    public void Example()
    {
        const int NoContainers = 3;
        const int InitialHeight = 3;

        Stack<char>[] containers = Enumerable.Range(1, NoContainers).Select(i => new Stack<char>()).ToArray();

        for (int j = 0; j < NoContainers; j++)
        {
            for (int i = InitialHeight - 1; i >= 0; i--)
            {
                var container = InputExample[i][1 + 4 * j];
                if (container != ' ')
                {
                    containers[j].Push(container);
                }
            }
        }
        foreach (string line in InputExample.Skip(InitialHeight+2))
        {
            var order = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)").Groups;
            int amount = int.Parse(order[1].Value);
            int from = int.Parse(order[2].Value) - 1;
            int to = int.Parse(order[3].Value) - 1;

            for (int i = 0; i < amount; i++)
            {
                containers[to].Push(containers[from].Pop());
            }
        }
        string result = new(containers.Select(c => c.Peek()).ToArray());
        result.Should().Be("CMZ");
    }

    [Test]
    public void Part1()
    {
        const int NoContainers = 9;
        const int InitialHeight = 8;

        Stack<char>[] containers = Enumerable.Range(1, NoContainers).Select(i => new Stack<char>()).ToArray();

        for (int j = 0; j < NoContainers; j++)
        {
            for (int i = InitialHeight-1; i >= 0; i--)
            {
                var container = InputPart1[i][1 + 4 * j];
                if (container != ' ')
                {
                    containers[j].Push(container);
                }
            }
        }
        foreach (string line in InputPart1.Skip(InitialHeight+2))
        {
            var order = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)").Groups;
            int amount = int.Parse(order[1].Value);
            int from = int.Parse(order[2].Value) - 1;
            int to = int.Parse(order[3].Value) - 1;

            for (int i = 0; i < amount; i++)
            {
                containers[to].Push(containers[from].Pop());
            }
        }
        string result = new(containers.Select(c => c.Peek()).ToArray());
        result.Should().Be("ZRLJGSCTR");
    }

    [Test]
    public void Part2()
    {
        const int NoContainers = 9;
        const int InitialHeight = 8;

        Stack<char>[] containers = Enumerable.Range(1, NoContainers).Select(i => new Stack<char>()).ToArray();

        for (int j = 0; j < NoContainers; j++)
        {
            for (int i = InitialHeight - 1; i >= 0; i--)
            {
                var container = InputPart2[i][1 + 4 * j];
                if (container != ' ')
                {
                    containers[j].Push(container);
                }
            }
        }
        foreach (string line in InputPart2.Skip(InitialHeight + 2))
        {
            var order = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)").Groups;
            int amount = int.Parse(order[1].Value);
            int from = int.Parse(order[2].Value) - 1;
            int to = int.Parse(order[3].Value) - 1;

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
        string result = new(containers.Select(c => c.Peek()).ToArray());
        result.Should().Be("PRTTGRFPB");
    }
}