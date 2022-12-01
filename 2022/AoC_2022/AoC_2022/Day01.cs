namespace AoC_2022;

public class Day01
{
    readonly string[] InputPart_1 = Reader.ReadLines(@"InputFiles\Day01-1.txt").ToArray();
    readonly string[] InputPart_2 = Reader.ReadLines(@"InputFiles\Day01-2.txt").ToArray();

    [Test]
    public void Part1()
    {
        int result = 0;
        int current = 0;
        foreach (string line in InputPart_1)
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
        foreach (string line in InputPart_2)
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