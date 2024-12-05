using FluentAssertions;

namespace AoC_2024;

public sealed class Day05 : Day
{
    [Puzzle(answer: 4637)]
    public int Part1()
    {
        var grouped = Input.GroupBy(string.Empty);
        var rules = grouped[0];
        var updates = grouped[1];
        var ordering = new Dictionary<int, HashSet<int>>();
        int result = 0;
        
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
            var splitted = t.Split(',').Select(int.Parse).ToList();
            var sorted = splitted.ToList();
            sorted.Sort((x, y) => Compare(x,y,ordering));
            
            if (splitted.Zip(sorted).All(pair => pair.First == pair.Second))
                result += splitted[splitted.Count / 2];
        }
        return result;
    }
    
    [Puzzle(answer: 6370)]
    public int Part2()
    {
        var grouped = Input.GroupBy(string.Empty);
        var rules = grouped[0];
        var updates = grouped[1];
        var ordering = new Dictionary<int, HashSet<int>>();
        int result = 0;
        
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
            var splitted = t.Split(',').Select(int.Parse).ToList();
            var sorted = splitted.ToList();
            sorted.Sort((x, y) => Compare(x,y,ordering));
            
            if (splitted.Zip(sorted).Any(pair => pair.First != pair.Second))
                result += sorted[splitted.Count / 2];
        }
        return result;
    }
    
    private int Compare(int x, int y, Dictionary<int, HashSet<int>> rules)
        => rules.TryGetValue(x, out var set) && set.Contains(y)
            ? -1
            : 1;
};
