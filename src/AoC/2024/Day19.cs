using Collections;
using FluentAssertions;
using System.Text.RegularExpressions;

namespace AoC._2024;

public sealed class Day19 : Day
{
    [Puzzle(answer: 330)]
    public long Part1() => Possibilities(Input).Count(x => x > 0);
    
    
    [Puzzle(answer: 950763269786650)]
    public long Part2() => Possibilities(Input).Sum(x => x);
    
    private static List<long> Possibilities(string[] lines)
    {
        var parts = lines[0].Split(", ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var possibilities = new List<long>(lines[2..].Length);
        foreach (var line in lines[2..])
        {
            var dp = new long[line.Length + 1];
            dp[0] = 1;
            for (int i = 0; i < line.Length; i++)
            {
                foreach (var part in parts.Where(x => line[i..].StartsWith(x)))
                {
                    dp[i + part.Length] += dp[i];
                }
            }
            possibilities.Add(dp[^1]);
        }
        return possibilities;
    }
};