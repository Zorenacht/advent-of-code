namespace AoC_2022;

public class Day02 : Day
{
    [Test]
    public void Example()
    {
        int result = 0;
        foreach (string line in InputExample)
        {
            var split = line.Split(',', '.', ':');
        }
        result.Should().Be(68923);
        Assert.Pass();
    }

    [Test]
    public void Part1()
    {
        int result = 0;
        foreach (string line in InputPart1)
        {
        }
        result.Should().Be(68923);
        Assert.Pass();
    }

    [Test]
    public void Part2()
    {
        int result = 0;
        foreach (string line in InputPart2)
        {
        }
        result.Should().Be(68923);
        Assert.Pass();
    }
}