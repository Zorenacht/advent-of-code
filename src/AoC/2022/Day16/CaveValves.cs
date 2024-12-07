using ShortestPath;

namespace AoC_2022;

public class CaveValves
{
    public Dictionary<int, ValveNode> Valves { get; set; } = [];
    public Dictionary<string, int> NameMap { get; set; } = [];

    public static CaveValves Parse(string[] lines)
    {
        var valves = new Dictionary<int, ValveNode>();
        var nameMap = new Dictionary<string, int>();
        int no = 0;
        //Parse all nodes 
        foreach (var line in lines)
        {
            var split = line.Split(new string[] { "Valve ", " has flow rate=", "; tunnels lead to valves ", "; tunnel leads to valve ", ", " }, StringSplitOptions.None);
            valves.Add(no, new ValveNode() { Name = split[1], No = no, Pressure = int.Parse(split[2]) });
            nameMap.Add(split[1], no);
            no++;
        }
        //Parse connections
        foreach (var line in lines)
        {
            var split = line.Split(new string[] { "Valve ", " has flow rate=", "; tunnels lead to valves ", "; tunnel leads to valve ", ", " }, StringSplitOptions.None);
            valves[nameMap[split[1]]].Next = split[3..].Select(name => valves[nameMap[name]]).ToList();
        }
        return new CaveValves() { Valves = valves, NameMap = nameMap };
    }

    //Create a graph with distances between all the non-zero pressure valves
    public ReducedCaveValves Reduce()
    {
        var pressureValves = Valves.Where(p => p.Value.Pressure > 0).Append(Valves.First(p => p.Value.Name == "AA")).Select((pair, Index) => (Index, pair.Value));
        var reducedNodes = pressureValves.ToDictionary(
            pair => pair.Index,
            pair => new ReducedValveNode() { Name = pair.Value.Name, No = pair.Index, Pressure = pair.Value.Pressure });
        var nameMap = pressureValves.ToDictionary(
            pair => pair.Value.Name,
            pair => pair.Index);


        foreach (var (fKey, from) in pressureValves)
        {
            var list = new List<DistanceNode<ReducedValveNode>>();
            foreach (var (tKey, to) in pressureValves)
            {
                if (!from.Equals(to))
                {
                    var aStar = new AStarPath<ValveNode>(from, to);
                    aStar.Run();

                    list.Add(new DistanceNode<ReducedValveNode>() { Node = reducedNodes[tKey], Distance = aStar.ShortestPath });
                }
            }
            reducedNodes[fKey].Next = list;
        }
        return new ReducedCaveValves() { Valves = reducedNodes, NameMap = nameMap };
    }
}

public class ReducedCaveValves
{
    public Dictionary<int, ReducedValveNode> Valves { get; set; } = [];
    public Dictionary<string, int> NameMap { get; set; } = [];
    public Dictionary<int, int> Memoization { get; set; } = new Dictionary<int, int>(10_000_000);

    public int Max(int time) => RecursiveElephant(
        self: Valves[NameMap["AA"]],
        time: time,
        opened: 0);

    public int MaxWithElephant(int time)
    {
        int full = (1 << 15) - 1;
        int max = 0;
        for (int opened = 0; opened < (full + 1) / 2; opened++)
        {
            var me = RecursiveElephant(
                self: Valves[NameMap["AA"]],
                time: time,
                opened: opened);
            var elephant = RecursiveElephant(
                self: Valves[NameMap["AA"]],
                time: time,
                opened: opened ^ full);
            if (max <= me + elephant) max = me + elephant;
        }
        return max;
    }

    public int RecursiveElephant(
        ReducedValveNode self,
        int time,
        int opened)
    {
        int memoizeState = opened + ((time + (self.No << 8)) << 16);
        if (Memoization.TryGetValue(memoizeState, out int memoized)) return memoized;

        var pressure = self.Pressure * time;

        var list = self.Next
            .Where(valve => time - valve.Distance > 0 && (opened & (1 << valve.Node.No)) == 0)
            .Select(distNode => RecursiveElephant(
                distNode.Node,
                time - distNode.Distance - 1,
                opened + (1 << distNode.Node.No)))
            .ToList();

        var max = list.Count > 0 ? list.Max() : 0;
        Memoization.Add(memoizeState, pressure + max);
        return pressure + max;
    }
}
