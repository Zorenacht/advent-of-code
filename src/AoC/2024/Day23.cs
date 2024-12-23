using FluentAssertions;
using System.Linq;

namespace AoC._2024;

public sealed class Day23 : Day
{
    [Puzzle(answer: 1173)]
    public long Part1()
    {
        long result = 0;
        var lines = Input;
        var connections = new Dictionary<string, HashSet<string>>();
        foreach (var line in lines)
        {
            var splitted = line.Split('-');
            if (!connections.TryAdd(splitted[0], [splitted[1]])) connections[splitted[0]].Add(splitted[1]);
            if (!connections.TryAdd(splitted[1], [splitted[0]])) connections[splitted[1]].Add(splitted[0]);
        }
        var tried = new HashSet<string>();
        foreach (var key in connections.Keys)
        {
            var conns = connections[key].Where(x => !tried.Contains(x)).ToList();
            for (int i = 0; i < conns.Count; ++i)
            {
                for (int j = i + 1; j < conns.Count; ++j)
                {
                    if (connections[conns[i]].Contains(conns[j]) &&
                        (key[0] == 't' || conns[i][0] == 't' || conns[j][0] == 't'))
                    {
                        Console.WriteLine($"{key},{conns[i]},{conns[j]}");
                        result++;
                    }
                }
            }
            tried.Add(key);
        }
        return result;
    }

    [Puzzle(answer: "cm,de,ez,gv,hg,iy,or,pw,qu,rs,sn,uc,wq")]
    public string Part2()
    {
        var lines = Input;
        var connections = new Dictionary<string, HashSet<string>>();
        foreach (var line in lines)
        {
            var split = line.Split('-');
            if (!connections.TryAdd(split[0], [split[1]])) 
                connections[split[0]].Add(split[1]);
            if (!connections.TryAdd(split[1], [split[0]])) 
                connections[split[1]].Add(split[0]);
        }
        var cliques = new List<HashSet<string>>();
        BronKerbosch([], connections.Keys.ToHashSet(), [], connections, cliques);
        return string.Join(",", cliques.MaxBy(x => x.Count)!.Order());
    }

    private void BronKerbosch(
        HashSet<string> r, 
        HashSet<string> p, 
        HashSet<string> x,
        Dictionary<string, HashSet<string>> c,
        List<HashSet<string>> cliques)
    {
        if (p.Count == 0 && x.Count == 0)
        {
            cliques.Add(r);
            return;
        }
        foreach(var vertex in p)
        {
            var newR = r.ToHashSet(); newR.UnionWith([vertex]);
            var newP = p.ToHashSet(); newP.IntersectWith(c[vertex]);
            var newX = x.ToHashSet(); newX.IntersectWith(c[vertex]);
            BronKerbosch(newR, newP, newX, c, cliques);
            p.Remove(vertex);
            x.UnionWith([vertex]);
        }
    }
};