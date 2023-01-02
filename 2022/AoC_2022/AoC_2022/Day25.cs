using System.Text;

namespace AoC_2022;


public sealed partial class Day25 : Day
{
    [Test]
    public void Example() => Parse(InputExample).Should().Be("2=-1=0");
    [Test]
    public void Part1() => Parse(InputPart1).Should().Be("2011-=2=-1020-1===-1");

    [Test]
    public void Base5() => Base5To10("1121-1110-1=0").Should().Be(314159265);


    public static string Parse(string[] input)
    {
        var total = input.Select(Base5To10).Sum();
        return Base10To5(total);
    }

    public static string Base10To5(long base10)
    {
        var sb = new StringBuilder();
        while (base10 != 0)
        {
            long remainder = SnafuMod(base10);
            sb.Append(ValueSymbol(remainder));
            base10 -= remainder;
            base10 /= 5;
        }
        return new string(sb.ToString().Reverse().ToArray());
    }

    private static long SnafuMod(long value) => value % 5 < 3 ? value % 5 : (value % 5) - 5;

    public static long Base5To10(string base5)
    {
        long count = 0;
        for (int i = base5.Length - 1; i >= 0; i--)
        {
            int pow = base5.Length - 1 - i;
            count += (long)Math.Pow(5, pow) * SymbolValue(base5[i]);
        }
        return count;
    }

    private static char ValueSymbol(long val) => val switch
    {
        -2 => '=',
        -1 => '-',
        0 => '0',
        1 => '1',
        2 => '2',
        _ => throw new NotSupportedException(),
    };

    private static int SymbolValue(char c) => c switch
    {
        '=' => -2,
        '-' => -1,
        '0' => 0,
        '1' => 1,
        '2' => 2,
        _ => throw new NotSupportedException(),
    };
}
