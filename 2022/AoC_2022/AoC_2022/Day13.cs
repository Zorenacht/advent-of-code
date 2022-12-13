using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework.Internal;
using System.Text;

namespace AoC_2022;

public static class ListExtensionForDistressSignal
{
    public static int DecoderKey(this IEnumerable<string> sorted)
    {
        var two = sorted.ToList().FindIndex(str => str == "[[2]]");
        var six = sorted.ToList().FindIndex(str => str == "[[6]]");
        return (two + 1) * (six + 1);
    }
}


public sealed partial class Day13 : Day
{

    [TestCase("[1,2]", "[1,3]")]
    [TestCase("[1,[2]]", "[1,3]")]
    [TestCase("[[]]", "[[], 1]")]
    [TestCase("[1,[]]", "[1,[],1]")]
    [TestCase("[1,[], 1]", "[1,[],2]")]
    [TestCase("[[[]],2]", "[[[], 1],1]")]
    public void Ordered(string first, string second) => CompareLines(first,second).Should().Be(1);

    [TestCase("[1,2]", "[1,2]")]
    [TestCase("[1,[2]]", "[1,2]")]
    [TestCase("[[]]", "[[]]")]
    [TestCase("[]", "[]")]
    [TestCase("[1,[]]", "[1,[]]")]
    public void Equivalent(string first, string second) => CompareLines(first, second).Should().Be(0);

    [TestCase("[1,2]", "[1,1]")]
    [TestCase("[1,[2]]", "[1,1]")]
    [TestCase("[[1]]", "[[]]")]
    [TestCase("[1]", "[]")]
    [TestCase("[[],2]", "[[]]")]
    [TestCase("[[],2]", "[[],1]")]
    public void NotOrdered(string first, string second) => CompareLines(first, second).Should().Be(-1);

    [Test]
    public void Example() => Simulate(InputExample).Should().Be(13);
    [Test]
    public void Part1() => Simulate(InputPart1).Should().Be(5350);


    private static readonly List<string> DividerPackets = new List<string> { "[[2]]", "[[6]]" };
    [Test]
    public void ExampleP2() => InputExample
        .Where(signal => signal != string.Empty)
        .Concat(DividerPackets)
        .OrderBy(signal => signal, new DistressSignalComparer())
        .DecoderKey()
        .Should()
        .Be(140);

    [Test]
    public void Part2() => InputPart1
        .Where(signal => signal != string.Empty)
        .Concat(DividerPackets)
        .OrderBy(signal => signal, new DistressSignalComparer())
        .DecoderKey()
        .Should()
        .Be(19570);

    private class DistressSignalComparer : IComparer<string>
    {
        public int Compare(string? x, string? y) => CompareLines(y!, x!);
    }

    private static int Simulate(string[] lines)
    {
        int result = 0;
        for (int i = 0; i < (lines.Length + 1) / 3; i++)
        {
            var first = lines[i * 3];
            var second = lines[i * 3 + 1];
            var compare = CompareLines(first, second);
            result += compare >= 0 ? i + 1 : 0;
        }
        return result;
    }

    static IEnumerable<string> ToList(string line)
    {
        var sb = new StringBuilder();
        int depth = 1;
        foreach (char c in line[1..^1])
        {
            if (depth == 1 && c == ',')
            {
                yield return sb.ToString();
                sb.Clear();
                continue;
            }
            if (c == '[') depth++;
            if (c == ']') depth--;
            sb.Append(c);
        }
        yield return sb.ToString();
    }

    static bool IsArray(string line) => line != string.Empty && line[0] == '[';

    static int CompareLines(string first1, string second2)
    {
        var firstArray = ToList(first1).ToArray();
        var secondArray = ToList(second2).ToArray();
        var shortCircuit = Math.Sign(secondArray.Length - firstArray.Length);
        foreach (var (first, second) in Enumerable.Range(0, Math.Min(firstArray.Length, secondArray.Length)).Select(i => (firstArray[i], secondArray[i])))
        {
            if (first == "" && second == "") continue;
            else if (first == "") return 1;
            else if (second == "") return -1; 
            if (!IsArray(first) && !IsArray(second))
            {
                int f = int.Parse(first);
                int s = int.Parse(second);
                if (s - f != 0) return s - f;
                else continue;
            }
            var firsty = IsArray(first) ? first : Arrayfy(first);
            var secondy = IsArray(second) ? second : Arrayfy(second);
            var compare = CompareLines(firsty, secondy);
            if (compare != 0) return compare;
        }
        return shortCircuit;
    }

    static string Arrayfy(string number) => "[" + number + "]";
}