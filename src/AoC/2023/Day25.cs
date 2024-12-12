namespace AoC._2023;

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
            //parse
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

            //fix the first edge as START, try any other edge as END till there can only be 3 shortest paths from START to END without common edges
            //as they are the shortest paths, apparently removing all edges in the paths creates two disconnected graphs
            var start = edges.First().Key;
            foreach (var end in edges.Skip(1).ToDictionary().Keys)
            {
                var copy = edges
                    .Select(x => new KeyValuePair<string, HashSet<string>>(x.Key, x.Value.ToHashSet()))
                    .ToDictionary();
                var paths = new List<List<string>>();
                while (ShortestPath(start, end, copy, out var path))
                {
                    paths.Add(path);
                    for (int i = 0; i < paths[^1].Count - 1; i++)
                    {
                        copy[paths[^1][i]].Remove(paths[^1][i + 1]);
                        copy[paths[^1][i + 1]].Remove(paths[^1][i]);
                    }
                };
                if (paths.Count == 3)
                {
                    var reachable = ReachableVertices(start, copy);
                    return reachable * (edges.Count - reachable);
                }
            }
            throw new Exception("No solution found.");
        }

        private int ReachableVertices(
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

        private bool ShortestPath(
            string from,
            string to,
            Dictionary<string, HashSet<string>> edges,
            out List<string> path)
        {
            path = new List<string>();
            var dictionary = new Dictionary<string, (string Previous, int Distance)>();
            var pq = new PriorityQueue<(string Previous, string Current), int>();
            pq.Enqueue((string.Empty, from), 0);
            while (pq.Count > 0)
            {
                pq.TryDequeue(out var vertex, out int priority);
                if (dictionary.ContainsKey(vertex.Current!)) continue;
                dictionary[vertex.Current] = (vertex.Previous, priority);
                if (vertex.Current == to)
                {
                    var current = vertex.Current;
                    while (current != string.Empty)
                    {
                        path.Insert(0, current);
                        current = dictionary[current].Previous;
                    }
                    return true;
                }
                foreach (var next in edges[vertex.Current]) pq.Enqueue((vertex.Current, next), priority + 1);
            }
            return false;
        }
    }
}