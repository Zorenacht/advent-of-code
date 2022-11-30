namespace AoC_2022;

public class Day_01
{
    readonly string[] InputPart_1 = Reader.ReadLines(@"InputFiles\Day_01-1.txt").ToArray();
    readonly string[] InputPart_2 = Reader.ReadLines(@"InputFiles\Day_01-2.txt").ToArray();

    [Test]
    public void Part_1()
    {
        int result = 0;
        List<int> input = new List<int>();
        foreach (string line in InputPart_1)
        {
            input.Add(int.Parse(line));
        }
        Console.WriteLine("hey");
        result.Should().Be(1653);
    }

    [Test]
    public void Part_2()
    {
        int result = 0;
        List<int> input = new List<int>();
        foreach (string line in InputPart_2)
        {
            input.Add(int.Parse(line));
        }
        result.Should().Be(1653);
    }
}