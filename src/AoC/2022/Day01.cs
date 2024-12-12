namespace AoC._2022;

public sealed class Day01 : Day
{
    [Test]
    public void Part1()
    {
        int result = 0;
        int current = 0;
        foreach (string line in InputPart1)
        {
            if (line == string.Empty)
            {
                if (current > result)
                {
                    result = current;
                }
                current = 0;
            }
            else
            {
                current += int.Parse(line);
            }
        }
        result.Should().Be(68923);
    }

    [Test]
    public void Part2()
    {
        int current = 0;
        var top3 = new PriorityQueue<int, int>(3);
        foreach (string line in InputPart2)
        {
            if (line == string.Empty)
            {
                if (top3.Count < 3)
                {
                    top3.Enqueue(current, current);
                }
                else if (top3.Peek() < current)
                {
                    top3.Dequeue();
                    top3.Enqueue(current, current);
                }
                current = 0;
            }
            else
            {
                current += int.Parse(line);
            }
        }
        (top3.Dequeue() + top3.Dequeue() + top3.Dequeue()).Should().Be(200044);
    }
}