using FluentAssertions;

namespace AoC_2023;

public sealed class Day01 : Day
{
    [Test]
    public void Part1()
    {
        int result = 0;
        var parse = Input.Select(Int32.Parse).ToArray();
        var set = new HashSet<int>(parse);
        result.Should().Be(0);
    }

    [Test]
    public void Part2()
    {
        int result = 0;
        var parse = Input.Select(Int32.Parse).ToArray();
        var set = new HashSet<int>(parse);
        result.Should().Be(0);
    }
}