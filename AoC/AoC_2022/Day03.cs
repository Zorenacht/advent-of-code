namespace AoC_2022;

public class Day03 : Day
{
    [Test]
    public void Example()
    {
        int score = 0;
        var seen = new bool[52];
        foreach (string line in InputExample)
        {
            for (int i = 0; i < line.Length / 2; i++)
            {
                seen[CharToIndex(line[i])] = true;
            }
            for (int i = line.Length / 2; i < line.Length; i++)
            {
                if (seen[CharToIndex(line[i])])
                {
                    score += CharToIndex(line[i]) + 1;
                    break;
                };
            }
            seen = new bool[52];
        }
        score.Should().Be(157);
    }

    public int CharToIndex(char ch) =>
        ch >= 'a' && ch <= 'z'
        ? ch - 'a'
        : ch - 'A' + 26;

    [Test]
    public void Part1()
    {
        int score = 0;
        var seen = new bool[52];
        foreach (string line in InputPart1)
        {
            for (int i = 0; i < line.Length / 2; i++)
            {
                seen[CharToIndex(line[i])] = true;
            }
            for (int i = line.Length / 2; i < line.Length; i++)
            {
                if (seen[CharToIndex(line[i])])
                {
                    score += CharToIndex(line[i]) + 1;
                    break;
                };
            }
            seen = new bool[52];
        }
        score.Should().Be(7903);
    }

    [Test]
    public void Part2()
    {
        int score = 0;
        var seen = new int[52];
        int groupCounter = 0;
        foreach (string line in InputPart2)
        {
            if (groupCounter == 3)
            {
                seen = new int[52];
                groupCounter = 0;
            }
            foreach (var uniqueChar in line.Distinct())
            {
                int index = CharToIndex(uniqueChar);
                if (seen[index] == 2)
                {
                    score += index + 1;
                    break;
                }
                else
                {
                    seen[index] += 1;
                }
            }
            groupCounter++;
        }
        score.Should().Be(2548);
    }
}