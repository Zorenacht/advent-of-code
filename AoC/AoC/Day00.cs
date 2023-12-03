using FluentAssertions;

namespace AoC;

public static class Template
{
    public static string Code(int day) =>
@$"using FluentAssertions;

namespace AoC_2023;

public sealed class Day{day:00} : Day
{{
    [Puzzle(answer: null)]
    public void Part1()
    {{
        int result = 0;
        var parse = Input;
        result.Should().Be(0);
    }}

    [Puzzle(answer: null)]
    public void Part2()
    {{
        int result = 0;
        var parse = Input;
        result.Should().Be(0);
    }}
}}";
}