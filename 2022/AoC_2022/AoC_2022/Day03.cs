using MathNet.Numerics.Providers.LinearAlgebra;
using System.Runtime.Serialization;

namespace AoC_2022;

public class Day03 : Day
{
    [Test]
    public void Example()
    {
        int score = 0;
        var seen = new HashSet<char>(52);
        foreach (string line in InputExample)
        {
            for(int i=0; i < line.Length/2; i++)
            {
                seen.Add(line[i]);
            }
            for (int i = line.Length / 2; i < line.Length; i++)
            {
                if (seen.Contains(line[i]))
                {
                    if (line[i] >= 'a' && line[i] <= 'z')
                    {
                        score += line[i] - 'a' + 1;
                        break;
                    }
                    else
                    {
                        score += line[i] - 'A' + 1 + 26;
                        break;
                    }
                };
            }
            seen.Clear();
        }
        score.Should().Be(157);
    }

    [Test]
    public void Part1()
    {
        int score = 0;
        var seen = new HashSet<char>(52);
        foreach (string line in InputPart1)
        {
            for (int i = 0; i < line.Length / 2; i++)
            {
                seen.Add(line[i]);
            }
            for (int i = line.Length / 2; i < line.Length; i++)
            {
                if (seen.Contains(line[i]))
                {
                    if (line[i] >= 'a' && line[i] <= 'z')
                    {
                        score += line[i] - 'a' + 1;
                        break;
                    }
                    else
                    {
                        score += line[i] - 'A' + 1 + 26;
                        break;
                    }
                };
            }
            seen.Clear();
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
            if(groupCounter == 3)
            {
                seen = new int[52];
                groupCounter = 0;
            }
            foreach (var uniqueChar in line.Distinct())
            {
                int index;
                if (uniqueChar >= 'a' && uniqueChar <= 'z')
                {
                    index = uniqueChar - 'a';
                }
                else
                {
                    index = uniqueChar - 'A' + 26;
                }

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
        score.Should().Be(7903);
    }
}