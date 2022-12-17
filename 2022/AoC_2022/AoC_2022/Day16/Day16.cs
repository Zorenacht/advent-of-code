namespace AoC_2022;

public sealed partial class Day16 : Day
{
    [Test]
    public void Example() => CaveValves.Parse(InputExample).Reduce().Max(30).Should().Be(1651);
    [Test]
    public void Part1() => CaveValves.Parse(InputPart1).Reduce().Max(30).Should().Be(1751);

    [Test]
    public void ExampleP2() => CaveValves.Parse(InputExample).Reduce().MaxWithElephant(26).Should().Be(1707);
    [Test]
    public void Part2() => CaveValves.Parse(InputPart1).Reduce().MaxWithElephant(26).Should().Be(2207);
}
