namespace AoC_2022;


public sealed partial class Day16 : Day
{
    [Test]
    public void Example() => Class.Parse(InputExample).Result().Should().Be(0);
    [Test]
    public void Part1() => Class.Parse(InputPart1).Result().Should().Be(0);

    [Test]
    public void ExampleP2() => Class.Parse(InputExample).Result().Should().Be(0);
    [Test]
    public void Part2() => Class.Parse(InputPart1).Result().Should().Be(0);

    private class Class
    {

        public int Result() => 0;

        public static Class Parse(string[] lines)
        {
            foreach (var line in lines)
            {
                var split = line.Split(new string[] { "x=", ", y=", ":" }, StringSplitOptions.None);
            }
            return new Class();
        }

    }
}