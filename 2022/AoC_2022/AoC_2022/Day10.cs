namespace AoC_2022;

public sealed partial class Day10 : Day
{
    [Test]
    public void Example() => Simulate(InputExample).Should().Be(13);

    [Test]
    public void Part1() => Simulate(InputPart1).Should().Be(6044);

    [Test]
    public void Part2() => Simulate(InputPart1).Should().Be(2384);


    private static int Simulate(string[] input)
    {
        int visited = 0;
        foreach (string line in input)
        {

        }
        return visited;
    }
}