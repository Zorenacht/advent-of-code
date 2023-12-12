using AoC;
using MathNet.Numerics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Tools.Geometry;
using Tools.Shapes;

namespace AoC_2023;

public sealed class Day12 : Day
{
    [Puzzle(answer: 21)]
    public long Part1Example()
        => InputExample.Select(line => new HotSprings(line, 1).Arrangements()).Sum();

    [Puzzle(answer: 7251)]
    public long Part1() 
        => Input.Select(line => new HotSprings(line, 1).Arrangements()).Sum();


    [Puzzle(answer: 525152)]
    public long Part2Example()
        => InputExample.Select(line => new HotSprings(line, 5).Arrangements()).Sum();

    [Puzzle(answer: 2128386729962)]
    public long Part2()
        => Input.Select(line => new HotSprings(line, 5).Arrangements()).Sum();

    public class HotSprings
    {
        private string template;
        private int[] sequence;

        public HotSprings(string line, int repeat)
        {
            var singleTemplate = line.Split()[0];
            var singleSequence = line.Split()[1].Split(",").Select(x => int.Parse(x)).ToArray();
            template = string.Join("?", Enumerable.Repeat(singleTemplate, repeat));
            sequence = Enumerable.Repeat(singleSequence, repeat).SelectMany(x => x).ToArray();
        }

        public long Arrangements()
        {
            var states = sequence.Select(x => Enumerable.Repeat(0L, x + 1).ToList()).ToList();
            states.Add(new List<long>() { 0 });

            states[0][0] = 1;
            for (int templateIndex = 0; templateIndex < template.Length; templateIndex++)
            {
                var next = sequence.Select(x => Enumerable.Repeat(0L, x + 1).ToList()).ToList();
                next.Add(new List<long>() { 0 });
                var ch = template[templateIndex];
                if (ch == '.' || ch == '?')
                {
                    for (int i = 0; i < states.Count; i++)
                    {
                        next[i][0] += states[i][0];
                        if (i < states.Count - 1) next[i + 1][0] += states[i][^1];
                    }
                }
                if (ch == '#' || ch == '?')
                {
                    for (int i = 0; i < states.Count; i++)
                    {
                        for (int j = 0; j < states[i].Count - 1; j++)
                        {
                            next[i][j + 1] += states[i][j];
                        }
                    }
                }
                states = next;
            }
            return states[^2][^1] + states[^1][^1];
        }
    }
}