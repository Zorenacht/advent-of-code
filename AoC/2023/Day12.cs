using AoC;
using MathNet.Numerics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Tools.Geometry;

namespace AoC_2023;

public sealed class Day12 : Day
{
    [Puzzle(answer: 21)]
    public long Part1Example() => Part1(InputExample);

    [Puzzle(answer: 7251)]
    public long Part1() => Part1(Input);

    public long Part1(string[] lines)
    {
        int sum = 0;
        foreach (var line in lines)
        {
            var state = line.Split()[0];
            var sequence = line.Split()[1].Split(",").Select(x => int.Parse(x)).ToArray();
            var groups = new List<string>();
            //Groups(state, sequence, 0, 0, 
            Permutations(state, 0, new StringBuilder(), groups);
            foreach (var group in groups)
            {
                var split = group
                    .Split('.', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Length)
                    .ToArray();
                if (split.SequenceEqual(sequence)) sum++;
            }
        }
        return sum;
    }

    private void Permutations(string state, int index, StringBuilder sb, List<string> perms)
    {
        if (index == state.Length)
        {
            perms.Add(sb.ToString());
            return;
        }
        if ("?.".Contains(state[index]))
        {
            sb.Append('.');
            Permutations(state, index + 1, sb, perms);
            sb.Remove(sb.Length - 1, 1);
        }
        if ("?#".Contains(state[index]))
        {
            sb.Append('#');
            Permutations(state, index + 1, sb, perms);
            sb.Remove(sb.Length - 1, 1);
        }
    }

    private void Groups(string state, int seq, int stateI, int seqI, List<string> groups)
    {

    }

    [Puzzle(answer: 8410)]
    public long Part2Example() => Part2(InputExample);

    [Puzzle(answer: 622120986954)]
    public long Part2() => Part2(Input);

    public long Part2(string[] lines)
    {
        int sum = 0;
        var dict = new Dictionary<(string, int), int>();
        foreach (var line in lines)
        {
            var state = string.Join("?", Enumerable.Repeat(line.Split()[0], 5));
            var sequence = line.Split()[1].Split(",").Select(x => int.Parse(x)).ToArray();
            sequence = Enumerable.Repeat(sequence, 5).SelectMany(x => x).ToArray();

            var initialGroup = state.Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();
            //var dotsNeeded = sequence.Length - initialGroup.Count;
            var groups = new List<string[]>();
            Permutations2(sequence, 0, initialGroup, 0, groups);

            int count = 0;
            foreach (var group in groups)
            {
                if(count % 1000 == 0) Console.WriteLine(count);
                //var a = groups.Select(group => String.Join("",group.Select(word => word.Length))).ToList();
                int prod = 1;
                for (int i = 0; i < sequence.Length; i++)
                {
                    if (dict.TryGetValue((group[i], sequence[i]), out int val))
                    {
                        prod *= val;
                        continue;
                    }
                    val = PermutationCount(group[i], sequence[i]);
                    prod *= val;
                    dict[(group[i], sequence[i])] = val;
                }
                sum += prod;
                count++;
            }
        }
        return sum;
    }

    private int PermutationCount(string template, int count)
    {
        int sum = 0;
        var perms = new List<string>();
        Permutations(template, 0, new StringBuilder(), perms);
        foreach (var group in perms)
        {
            var split = group
                .Split('.', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Length)
                .ToArray();
            if (split.SequenceEqual([count])) sum++;
        }
        return sum;
    }

    private void Permutations2(
        int[] sequence,
        int sequenceIndex,
        List<string> groups,
        int groupIndex,
        List<string[]> perms)
    {
        if (groups.Count == sequence.Length && sequenceIndex == sequence.Length)
        {
            perms.Add(groups.ToArray());
            return;
        }
        if (sequenceIndex == sequence.Length) return;
        if (groupIndex == groups.Count) return;

        int tagsNeeded = sequence[sequenceIndex];

        //!!! needs to be able to skip all ????
        if (groups.Count > sequence.Length && groups[groupIndex].All(x => x == '?'))
        {
            var copy = groups.ToList();
            copy.RemoveAt(groupIndex);
            Permutations2(
                sequence,
                sequenceIndex,
                copy,
                groupIndex,
                perms);
        }
        if (groups[groupIndex].Length < tagsNeeded) return;
        //till -1 because splitting the last has no effect
        int firstTag = groups[groupIndex].IndexOf('#');
        if (groups[groupIndex].LastIndexOf('#') - firstTag + 1 <= tagsNeeded)
        {
            Permutations2(
                sequence,
                sequenceIndex + 1,
                groups,
                groupIndex + 1,
                perms);
        }
        for (int i = tagsNeeded; i < groups[groupIndex].Length - 1; i++)
        {
            if (groups[groupIndex][i] == '?' && (firstTag == -1 || (i - firstTag) <= tagsNeeded))
            {
                var copy = groups.ToList();
                var first = groups[groupIndex][..i];
                var second = groups[groupIndex][(i + 1)..];
                copy[groupIndex] = second;
                copy.Insert(groupIndex, first);
                Permutations2(
                    sequence,
                    sequenceIndex + 1,
                    copy,
                    groupIndex + 1,
                    perms);
            }
        }
    }
}