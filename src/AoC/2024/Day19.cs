namespace AoC._2024;

[PuzzleType(PuzzleType.Permutations, PuzzleType.DP, PuzzleType.Tree)]
public sealed class Day19 : Day
{
    [Puzzle(answer: 330)]
    public long Part1() => Possibilities(Input).Count(x => x > 0);

    [Puzzle(answer: 950763269786650)]
    public long Part2() => Possibilities(Input).Sum(x => x);

    private List<long> Possibilities(string[] lines)
    {
        var root = new TrieNode('0', false);

        var parts = lines[0].Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            var node = root;
            foreach (var ch in part)
            {
                var index = IndexOf(ch);
                var nde = node!.Nexts[index];
                if (nde != null) node = nde;
                else
                {
                    node.Nexts[index] = new TrieNode(ch, false);
                    node = node.Nexts[index];
                }
            }
            node!.IsEnd = true;
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
                    var nde = node.Nexts[IndexOf(line[j])];
                    if (nde != null)
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

    private static int IndexOf(char ch) => ch switch
    {
        'w' => 0,
        'u' => 1,
        'b' => 2,
        'r' => 3,
        'g' => 4,
        _ => throw new NotSupportedException()
    };

    public class TrieNode(char character, bool isEnd)
    {
        public char Character { get; } = character;
        public bool IsEnd { get; set; } = isEnd;
        public TrieNode?[] Nexts { get; } = [null, null, null, null, null];
    }
};