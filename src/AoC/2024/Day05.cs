using FluentAssertions;

namespace AoC_2024;

public sealed class Day05 : Day
{
    [Puzzle(answer: 4637)]
    public int Part1()
    {
        int result = 0;
        var reqsLine = true;
        var notAllowed = new Dictionary<int, HashSet<int>>();
        var lines = Input;
        
        for (int i = 0; i < lines.Length; ++i)
        {
            if (lines[i] == string.Empty)
            {
                reqsLine = false;
                continue;
            }
            if (reqsLine)
            {
                var splitted = lines[i].Split('|');
                var x = int.Parse(splitted[0]);
                var y = int.Parse(splitted[1]);
                if (!notAllowed.TryGetValue(x, out var set)) notAllowed[x] = [y];
                else set.Add(y);
            }
            else
            {
                var splitted = lines[i].Split(',').Select(int.Parse).ToArray();
                var before = new HashSet<int>();
                var valid = true;
                foreach (int val in splitted)
                {
                    foreach (int bf in before)
                    {
                        if (notAllowed.TryGetValue(val, out var reqs) && reqs.Contains(bf))
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
        }
        return result;
    }
    
    [Puzzle(answer: null)]
    public int Part2()
    {
        int result = 0;
        var reqsLine = true;
        var notAllowed = new Dictionary<int, HashSet<int>>();
        var lines = Input;
        
        for (int i = 0; i < lines.Length; ++i)
        {
            if (lines[i] == string.Empty)
            {
                reqsLine = false;
                continue;
            }
            if (reqsLine)
            {
                var splitted = lines[i].Split('|');
                var x = int.Parse(splitted[0]);
                var y = int.Parse(splitted[1]);
                if (!notAllowed.TryGetValue(x, out var set)) notAllowed[x] = [y];
                else set.Add(y);
            }
            else
            {
                var splitted = lines[i].Split(',').Select(int.Parse).ToArray();
                var newOrdering = new List<int>();
                
                var valid = true;
                var modified = false;
                foreach (int val in splitted)
                {
                    modified = false;
                    foreach (int bf in newOrdering)
                    {
                        if (notAllowed.TryGetValue(val, out var reqs) && reqs.Contains(bf))
                        {
                            valid = false;
                            modified = true;
                            newOrdering.Insert(newOrdering.IndexOf(bf), val);
                            break;
                        }
                    }
                    
                    if(!modified) newOrdering.Add(val);
                }
                if (!valid)
                    result += newOrdering[splitted.Length / 2];
            }
        }
        return result;
    }
};