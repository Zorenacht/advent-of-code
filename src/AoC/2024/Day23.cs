using Collections;
using System.Data;

namespace AoC._2024;

public sealed class Day23 : Day
{
    [Puzzle(answer: 1173)]
    public long Part1()
        => LAN
            .Parse(Input)
            .ThreeCliques('t');

    [Puzzle(answer: "cm,de,ez,gv,hg,iy,or,pw,qu,rs,sn,uc,wq")]
    public string Part2()
        => LAN
            .Parse(Input)
            .MaximalCliques()
            .MaxBy(x => x.Count)
            !.Order()
            .StringJoin(",");

    public class LAN
    {
        public Dictionary<string, HashSet<string>> Connections { get; set; } = [];

        public static LAN Parse(string[] lines)
        {
            var connections = new Dictionary<string, HashSet<string>>();
            foreach (var line in lines)
            {
                var splitted = line.Split('-');
                if (!connections.TryAdd(splitted[0], [splitted[1]])) connections[splitted[0]].Add(splitted[1]);
                if (!connections.TryAdd(splitted[1], [splitted[0]])) connections[splitted[1]].Add(splitted[0]);
            }
            return new LAN() { Connections = connections };
        }

        public int ThreeCliques(char anyStartsWith)
        {
            int result = 0;
            var tried = new HashSet<string>();
            foreach (var key in Connections.Keys)
            {
                var nbs = Connections[key].Where(x => !tried.Contains(x)).ToList();
                for (int i = 0; i < nbs.Count; ++i)
                {
                    for (int j = i + 1; j < nbs.Count; ++j)
                    {
                        if (Connections[nbs[i]].Contains(nbs[j]) &&
                            (key[0] == anyStartsWith || nbs[i][0] == anyStartsWith || nbs[j][0] == anyStartsWith))
                        {
                            result++;
                        }
                    }
                }
                tried.Add(key);
            }
            return result;
        }

        public List<HashSet<string>> MaximalCliques()
        {
            var cliques = new List<HashSet<string>>();
            BronKerbosch([], Connections.Keys.ToHashSet(), [], Connections, cliques);
            return cliques;
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

            // Because we will check the pivot itself, the neighbors exluded here will be checked later
            var pivot = p.FirstOrDefault() ?? x.First();
            var pWithoutPivotNeighbors = p.ToHashSet(); pWithoutPivotNeighbors.ExceptWith(c[pivot]);

            foreach (var vertex in pWithoutPivotNeighbors)
            {
                var newR = r.ToHashSet(); newR.UnionWith([vertex]);
                var newP = p.ToHashSet(); newP.IntersectWith(c[vertex]);
                var newX = x.ToHashSet(); newX.IntersectWith(c[vertex]);
                BronKerbosch(newR, newP, newX, c, cliques);
                p.Remove(vertex);
                x.UnionWith([vertex]);
            }
        }
    }

};