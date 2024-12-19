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
        var root = new TrieNode('0', false);
        
        var parts = lines[0].Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            var node = root;
            foreach (var ch in part)
            {
                if (node.Nexts.TryGetValue(ch, out var nde)) node = nde;
                else
                {
                    node.Nexts[ch] = new TrieNode(ch, false);
                    node = node.Nexts[ch];
                }
            }
            node.IsEnd = true;
        }
        
        var permutations = new List<long>(lines[2..].Length);
        foreach (var line in lines[2..])
        {
            var dp = new long[line.Length + 1];
            dp[0] = 1;
            
            for (int i = 0; i < line.Length; i++)
            {
                var node = root;
                for (int j = i; j < line.Length; ++j)
                {
                    if (node.Nexts.TryGetValue(line[j], out var nde))
                    {
                        node = nde;
                        if (node.IsEnd) dp[j + 1] += dp[i];
                    }
                    else break;
                }
            }
            permutations.Add(dp[^1]);
        }
        return permutations;
    }
    
    public class TrieNode(char character, bool isEnd)
    {
        public char Character { get; } = character;
        public bool IsEnd { get; set; } = isEnd;
        public Dictionary<char, TrieNode> Nexts { get; } = [];
    }
};