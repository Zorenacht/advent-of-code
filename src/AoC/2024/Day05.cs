using FluentAssertions;

namespace AoC_2024;

public sealed class Day05 : Day
{
    [Puzzle(answer: 4637)]
    public int Part1()
    {
        int result = 0;
        var grouped = Input.GroupBy(string.Empty);
        var rules = grouped[0];
        var updates = grouped[1];
        var ordering = new Dictionary<int, HashSet<int>>();
        
        foreach (string t in rules)
        {
            var splitted = t.Split('|');
            var x = int.Parse(splitted[0]);
            var y = int.Parse(splitted[1]);
            if (!ordering.TryGetValue(x, out var set)) ordering[x] = [y];
            else set.Add(y);
        }
        
        foreach (string t in updates)
        {
            var splitted = t.Split(',').Select(int.Parse).ToArray();
            var before = new HashSet<int>();
            var valid = true;
            foreach (int val in splitted)
            {
                foreach (int bf in before)
                {
                    if (ordering.TryGetValue(val, out var reqs) && reqs.Contains(bf))
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid) before.Add(val);
                else break;
            }
            if (valid) result += splitted[splitted.Length / 2];
        }
        return result;
    }
    
    [Puzzle(answer: 6370)]
    public int Part2()
    {
        int result = 0;
        var grouped = Input.GroupBy(string.Empty);
        var rules = grouped[0];
        var updates = grouped[1];
        var ordering = new Dictionary<int, HashSet<int>>();
        
        foreach (string t in rules)
        {
            var splitted = t.Split('|');
            var x = int.Parse(splitted[0]);
            var y = int.Parse(splitted[1]);
            if (!ordering.TryGetValue(x, out var set)) ordering[x] = [y];
            else set.Add(y);
        }
        
        foreach (string t in updates)
        {
            var splitted = t.Split(',').Select(int.Parse).ToArray();
            var newOrdering = new List<int>();
            
            var valid = true;
            foreach (int val in splitted)
            {
                var modified = false;
                foreach (int bf in newOrdering)
                {
                    if (ordering.TryGetValue(val, out var reqs) && reqs.Contains(bf))
                    {
                        valid = false;
                        modified = true;
                        newOrdering.Insert(newOrdering.IndexOf(bf), val);
                        break;
                    }
                }
                
                if (!modified) newOrdering.Add(val);
            }
            if (!valid)
                result += newOrdering[splitted.Length / 2];
        }
        return result;
    }
};