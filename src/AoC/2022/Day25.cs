using System.Text;

namespace AoC._2022;

public sealed partial class Day25 : Day
{
    [Test]
    public void Example() => Parse(InputExample).Should().Be("2=-1=0");
    [Test]
    public void Part1() => Parse(InputPart1).Should().Be("2011-=2=-1020-1===-1");

    [Test]
    public void Base5() => SnafuToLong("1121-1110-1=0").Should().Be(314159265);

    private static string Parse(string[] input)
    {
        var total = input.Select(SnafuToLong).Sum();
        return LongToSnafu(total);
    }

    private static string LongToSnafu(long base10)
    {
        var sb = new StringBuilder();
        while (base10 != 0)
        {
            base10 += 2;
            int remainder = (int)(base10 % 5);
            sb.Insert(0, "=-012"[remainder]);
            base10 /= 5;
        }
        return sb.ToString();
    }

    private static long SnafuToLong(string snafu)
    {
        long count = 0;
        foreach (var ch in snafu)
        {
            count = count * 5 + "=-012".IndexOf(ch) - 2;
        }
        return count;
    }
}
