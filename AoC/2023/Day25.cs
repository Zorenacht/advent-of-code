using System.Data;

namespace AoC_2023;

public sealed class Day25 : Day
{
    [Puzzle(answer: 54)]
    public long Part1Example() => new Snowverload().Part1(InputExample);

    [Puzzle(answer: 580800)]
    public long Part1() => new Snowverload().Part1(Input);

    private class Snowverload
    {
        internal long Part1(string[] input)
        {
            var split = input.Select(x => x.Split(" :".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            var edges = new Dictionary<string, HashSet<string>>();
            foreach (var line in split)
            {
                if (!edges.ContainsKey(line[0])) edges[line[0]] = new HashSet<string>();
                edges[line[0]].UnionWith(line[1..]);
                foreach (var node in line[1..])
                {
                    if (!edges.ContainsKey(node)) edges[node] = new HashSet<string>();
                    edges[node].Add(line[0]);
                }
            }

            var first = edges.First().Key;
            foreach (var end in edges.Skip(1).ToDictionary().Keys)
            {
                var copy = edges
                    .Select(x => new KeyValuePair<string, HashSet<string>>(x.Key, x.Value.ToHashSet()))
                    .ToDictionary();
                var paths = new List<List<string>>();
                while(FindPathAndRemoveEdges(first, end, new HashSet<string>() { first }, paths, copy));
                if (paths.Count == 3)
                {

                    for (int i = 0; i < paths[0].Count - 1; i++) copy[paths[0][i + 1]].Add(paths[0][i]);
                    for (int j = 0; j < paths[1].Count - 1; j++) copy[paths[1][j + 1]].Add(paths[1][j]);
                    for (int k = 0; k < paths[2].Count - 1; k++) copy[paths[2][k + 1]].Add(paths[2][k]);

                    var reachable = Reachable(first, copy);
                    if (reachable < edges.Count && reachable > 0) return reachable * (edges.Count - reachable);
                }
            }
            throw new Exception("No solution found.");
        }

        private int Reachable(
            string start, 
            Dictionary<string, HashSet<string>> edges)
        {
            var queue = new Queue<string>();
            queue.Enqueue(start);
            var reachable = new HashSet<string>();
            while (queue.Count > 0)
            {
                var element = queue.Dequeue();
                if (reachable.Contains(element)) continue;
                foreach (var next in edges[element])
                    queue.Enqueue(next);
                reachable.Add(element);
            }
            return reachable.Count;
        }

        private bool FindPathAndRemoveEdges(string from, string to, 
            HashSet<string> visited,
            List<List<string>> paths, 
            Dictionary<string, HashSet<string>> edges)
        {
            if (from == to)
            {
                paths.Add(new List<string>() { from });
                return true;
            }
            foreach (var next in edges[from])
            {
                if (visited.Contains(next)) continue;
                visited.Add(next);
                var res = FindPathAndRemoveEdges(next, to, visited, paths, edges);
                //visited.Remove(next);
                if (res) 
                {
                    edges[from].Remove(next);
                    edges[next].Remove(from);
                    paths[^1].Insert(0, from);
                    return true;
                }
            }
            return false;
        }
    }
}