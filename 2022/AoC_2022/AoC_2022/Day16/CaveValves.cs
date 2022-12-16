using ShortestPath;

namespace AoC_2022;

public class CaveValves
{
    public Dictionary<int, ValveNode> Valves { get; set; }
    public Dictionary<string, int> NameMap { get; set; }

    public static CaveValves Parse(string[] lines)
    {
        var valves = new Dictionary<int, ValveNode>();
        var nameMap = new Dictionary<string, int>();
        int no = 0;
        //Parse all nodes
        foreach (var line in lines)
        {
            var split = line.Split(new string[] { "Valve ", " has flow rate=", "; tunnels lead to valves ", "; tunnel leads to valve ", ", " }, StringSplitOptions.None);
            valves.Add(no, new ValveNode() { Name = split[1], No = no, Value = int.Parse(split[2]) });
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
        var pressureValves = Valves.Where(p => p.Value.Value > 0).Append(Valves.First(p => p.Value.Name == "AA"));
        var reducedNodes = pressureValves.ToDictionary(
            v => v.Key,
            v => new ReducedValveNode() { Name = v.Value.Name, No = v.Value.No, Value = v.Value.Value });


        foreach (var (fKey, from) in pressureValves)
        {
            var list = new List<DistanceNode<ReducedValveNode>>();
            foreach (var (tKey, to) in pressureValves)
            {
                if (!from.Equals(to))
                {
                    var aStar = new AStarPath<ValveNode>(from, to);
                    aStar.Run();

                    list.Add(new DistanceNode<ReducedValveNode>() { Node = reducedNodes[to.No], Distance = aStar.ShortestPath });
                }
            }
            reducedNodes[fKey].Next = list;
        }
        return new ReducedCaveValves() { Valves = reducedNodes, NameMap = NameMap };
    }
}

public class ReducedCaveValves
{
    public Dictionary<int, ReducedValveNode> Valves { get; set; }
    public Dictionary<string, int> NameMap { get; set; }

    public int Max(int time) => RecursiveElephant(
        self: Valves[NameMap["AA"]],
        selfTime: time,
        elephant: Valves[NameMap["AA"]],
        elephantTime: 0,
        opened: 0,
        time: time,
        pressure: 0);

    public int MaxWithElephant(int time) => RecursiveElephant(
        self: Valves[NameMap["AA"]],
        selfTime: time,
        elephant: Valves[NameMap["AA"]],
        elephantTime: time,
        opened: 0, 
        time: time, 
        pressure: 0);


    public static int RecursiveElephant(
        ReducedValveNode self,
        int selfTime,
        ReducedValveNode elephant,
        int elephantTime,
        long opened,
        int time,
        int pressure)
    {
        if (time == 0) return pressure;

        //create all next possibilities
        var selfNext = selfTime == time
            ? self.Next.Where(valve => time - valve.Distance > 0 && (opened & (1L << valve.Node.No)) == 0).ToList()
            : new List<DistanceNode<ReducedValveNode>>() { null };
        var elephantNext = elephantTime == time
            ? elephant.Next.Where(valve => time - valve.Distance > 0 && (opened & (1L << valve.Node.No)) == 0).ToList()
            : new List<DistanceNode<ReducedValveNode>>() { null };

        var comb = selfNext.SelectMany(self => elephantNext
            .Where(elephant => elephant?.Node != self?.Node)
            .Select(elephant => new { Self = self, Elephant = elephant })).ToList();
        var list = comb.Select(pair => UseOldValuesIfNull(self, selfTime, elephant, elephantTime, opened, pressure, time, pair.Self, pair.Elephant))
            .ToList();
        return list.Count > 0 ? list.Max() : pressure;
    }

    private static int UseOldValuesIfNull(
        ReducedValveNode oldSelf,
        int oldSelfTime,
        ReducedValveNode oldElephant,
        int oldElephantTime,
        long opened,
        int pressure,
        int time,
        DistanceNode<ReducedValveNode> newSelf,
        DistanceNode<ReducedValveNode> newElephant)
    {
        var self = newSelf is { } ? newSelf.Node : oldSelf;
        var selfTime = newSelf is { } ? oldSelfTime - newSelf.Distance - 1 : oldSelfTime;
        var elephant = newElephant is { } ? newElephant.Node : oldElephant;
        var elephantTime = newElephant is { } ? oldElephantTime - newElephant.Distance - 1 : oldElephantTime;
        opened = opened 
            + (newSelf is { } ? (1L << newSelf.Node.No) : 0) 
            + (newElephant is { } ? (1L << newElephant.Node.No) : 0);
        var a = (newSelf is { } ? (1L << newSelf.Node.No) : 0);
        var b= (newElephant is { } ? (1L << newElephant.Node.No) : 0);
        if (a == b)
        {
            throw new Exception();
        }
        pressure = pressure
            + (newSelf is { } ? newSelf.Node.Value * (time - newSelf.Distance - 1) : 0)
            + (newElephant is { } ? newElephant.Node.Value * (time - newElephant.Distance - 1) : 0);
        time = Math.Max(selfTime, elephantTime);

        return RecursiveElephant(
            self,
            selfTime,
            elephant,
            elephantTime,
            opened,
            time,
            pressure);
    }

    public static int Recursive(ReducedValveNode current, long opened, int time, int pressure)
    {
        if (time == 0) return pressure;
        var comb = current.Next
            .Where(valve => time - valve.Distance > 0 && (opened & (1 << valve.Node.No)) == 0)
            .ToList();
        var list = comb  
            .Select(distNode => Recursive(
                distNode.Node,
                opened + (1 << distNode.Node.No),
                time - distNode.Distance - 1,
                pressure + distNode.Node.Value * (time - distNode.Distance - 1))).ToList();
        return list.Count > 0 ? list.Max() : pressure;
    }
}
