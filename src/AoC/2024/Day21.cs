using FluentAssertions;
using System.Collections.Generic;
using System.Text;
using Tools.Geometry;

namespace AoC._2024;

[PuzzleType(PuzzleType.DP, PuzzleType.Manual)]
public sealed class Day21 : Day
{
    [Puzzle(answer: 224326)]
    public long Part1() => ComplexitiesV2(Input, 2);

    [Puzzle(answer: 279638326609472)]
    public long Part2() => ComplexitiesV2(Input, 25);

    // Determined rules:
    // first by running the algorithm and taking the lower result
    // later through logic, determined that < is far away and should be grouped
    // <^ bt ^<, because [<^]=[v<<,>^,>] and [^<]=[<,v<,>>^] where the latter contains an ungrouped <
    // v> bt >v, because [v>]=[<v,>,^  ] and [>v]=[v,<,^>  ] where the latter contains an ungrouped <
    // ^> bt >^, because [^>]=[<,v>,^  ] and [>^]=[v,<^,>  ] the logic doesnt seem to hold here :(,
    //           probably gets overruled due to <^ being more expensive than v> 
    // <v bt v<, because [<v]=[v<<,>,^>] and [v<]=[<v,<,>>^] where the latter contains an ungrouped <
    public long ComplexitiesV2(string[] codes, int pads)
    {
        var numbers = codes.Select(line => line.Ints().First());
        var numpad = "789\n456\n123\n 0A".Split('\n').ToMaze();
        var dpad = " ^A\n<v>".Split('\n').ToMaze();
        var cds = new string[]
        {
            "A<^AvA^^^Avvv>A", //208A
            "A<^^A<A>vvA>A", //540A
            "A^^A<^AvAvv>A", //685A
            "A<^^^A<A>>AvvvA", //879A
            "A<^^^AvvA^>AvvA", //826A
        };
        //var cds = new string[]
        //{
        //    "A<A^A>^^AvvvA", //029A
        //    "A^^^A<AvvvA>A", //980A
        //    "A^<<A^^A>>AvvvA", //179A
        //    "A^^<<A>A>AvvA", //456A
        //    "A^A<<^^A>>AvvvA", //379A apparently <<^^ preferred over ^^<<
        //};
        long result = 0;
        for (int k = 0; k < cds.Length; ++k)
        {
            var counts = new Dictionary<string, long>();
            foreach (var cd in cds[k].Split('A', StringSplitOptions.RemoveEmptyEntries)
                                     .Select(y => $"A{y}A"))
            {
                if (!counts.TryAdd(cd, 1)) counts[cd]++;
            }


            for (int i = 0; i < pads; ++i)
            {
                var next = new Dictionary<string, long>();
                foreach (var count in counts)
                {
                    var sb = new StringBuilder();
                    for (int j = 0; j < count.Key.Length - 1; ++j)
                    {
                        sb.Append(Dpads[count.Key[j..(j + 2)]]);
                    }
                    sb.Insert(0, 'A');
                    var strs = sb.ToString().Split('A')[1..^1].Select(y => $"A{y}A").ToArray();
                    foreach (var str in strs)
                    {
                        if (!next.TryAdd(str, count.Value)) next[str] += count.Value;
                    }
                }
                counts = next;
            }
            long length = 0;
            foreach (var kv in counts)
            {
                length += kv.Value * (kv.Key.Length - 1);
            }
            result += length * codes[k].Ints().First();
        }
        return result;
    }
    
    // Map for optimal required button pressed (Value),
    //  to make the robot go from (Key="ab") a to b and press b
    Dictionary<string, string> Dpads = new Dictionary<string, string>()
    {
        { "A<", "v<<A" },
        { "A^", "<A" },
        { "A>", "vA" },
        { "Av", "<vA" }, //optimized
        { "<A", ">>^A" },
        { "^A", ">A" },
        { ">A", "^A" },
        { "vA", "^>A" }, //optimized

        { ">^", "<^A" }, //optimized
        { ">v", "<A" },
        { "><", "<<A" },
        { "^>", "v>A" }, //optimized
        { "v>", ">A" },
        { "<>", ">>A" },

        { "<^", ">^A" }, //enforced
        { "<v", ">A" },
        { "v<", "<A" },
        { "^<", "v<A" }, //enforced

        { "v^", "^A" },
        { "^v", "vA" },

        { "^^", "A" },
        { "AA", "A" },
        { "vv", "A" },
        { ">>", "A" },
        { "<<", "A" },
    };
};