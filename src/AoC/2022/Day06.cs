namespace AoC_2022;

public sealed partial class Day06 : Day
{
    [Test]
    public void Example()
    {
        var result = StartOfMessage(InputExample[0], 4);
        result.Should().Be(11);
    }

    [Test]
    public void Part1()
    {
        var result = StartOfMessage(InputPart1[0], 4);
        result.Should().Be(1282);
    }

    [Test]
    public void Part2()
    {
        var result = StartOfMessage(InputPart2[0], 14);
        result.Should().Be(3513);
    }


    private int StartOfMessage(string message, int unique)
    {
        int result = 0;
        var uniques = new HashSet<char>(unique);
        var queue = new Queue<char>(unique);
        foreach (char c in message)
        {
            if (uniques.Count == unique) break;
            while (uniques.Contains(c))
            {
                var top = queue.Dequeue();
                uniques.Remove(top);
            }
            queue.Enqueue(c);
            uniques.Add(c);
            result++;
        }
        return result;
    }
}