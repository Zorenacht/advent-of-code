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


    public class HotSprings
    {
        private string template;
        private int[] sequence;

        public HotSprings(string line, int repeat)
        {
            template = string.Join("?", Enumerable.Repeat(line.Split()[0], repeat));
            sequence = line.Split()[1].Split(",").Select(x => int.Parse(x)).ToArray();
            sequence = Enumerable.Repeat(sequence, repeat).SelectMany(x => x).ToArray();
        }

        public long Arrangements()
        {
            var states = sequence.Select(x => Enumerable.Repeat(0, x + 1).ToList()).ToList();
            states.Add(new List<int>() { 0 });
            //need to add another row to catch ending ....
            states[0][0] = 1;
            for(int templateIndex = 0; templateIndex < template.Length; templateIndex++)
            {
                var next = sequence.Select(x => Enumerable.Repeat(0, x + 1).ToList()).ToList();
                next.Add(new List<int>() { 0 });
                var ch = template[templateIndex];
                if (ch == '.' || ch == '?')
                {
                    for(int i = 0; i < states.Count - 1; i++)
                    {
                        for(int j = 0; j < states[i].Count; j++)
                        {
                            if (j == 0) next[i][j] += states[i][j];
                            else next[i + 1][0] += states[i][j];
                        }
                    }
                }
                if (ch == '#' || ch == '?')
                {
                    for (int i = 0; i < states.Count; i++)
                    {
                        for (int j = 0; j < states[i].Count - 1; j++)
                        {
                            next[i][j+1] += states[i][j];
                        }
                    }
                }
                states = next;
            }
            return states[^1][^1];
        }

        /*public IEnumerable<string[]> Splits(string line, int count)
        {
            for(int i=count; i < line.Length; i++)
            {
                if (Valid(line[..i],count))
            }
        }*/

        private static bool Valid(string template, int count)
        {
            int leftTag = template.IndexOf('#');
            int rightTag = template.LastIndexOf("#");
            if (template[leftTag..(rightTag + 1)].Any(x => x == '.')) return false;
            if (rightTag - leftTag + 1 > count) return false;
            //not checking enough ?'s yet
            return true;
        }
    }

    [TestCase("???.### 1,1,3", ExpectedResult =  1)]
    [TestCase(".??..??...?##. 1,1,3", ExpectedResult = 16384)]
    [TestCase("?#?#?#?#?#?#?#? 1,3,1,6", ExpectedResult = 1)]
    [TestCase("????.#...#... 4,1,1", ExpectedResult = 16)]
    [TestCase("????.######..#####. 1,6,5", ExpectedResult = 2500)]
    [TestCase("?###???????? 3,2,1", ExpectedResult = 506250)]
    [TestCase(".###.##.#. 3,2,1", ExpectedResult = 1)]
    public long Part2Example(string input) => new HotSprings(input, 5).Arrangements();

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
            var a = groups.Select(group => String.Join("", group.Select(word => word.Length))).ToList();
            foreach (var group in groups)
            {
                if(count % 1000 == 0) Console.WriteLine(count);
                //
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
        if (sequenceIndex == sequence.Length && groups.Count == sequence.Length)
        {
            perms.Add(groups.ToArray());
            return;
        }
        if (sequenceIndex == sequence.Length) return;
        if (groupIndex == groups.Count) return;

        int tagsNeeded = sequence[sequenceIndex];

        if (groups[groupIndex].Length < tagsNeeded) return;
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