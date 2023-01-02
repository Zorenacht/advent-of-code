using ShortestPath;

namespace AoC_2022;

public class ReducedValveNode : ValveNode
{
    public new List<DistanceNode<ReducedValveNode>> Next { get; set; }
}

public class ValveNode : IEquatable<ValveNode>, IState<ValveNode>
{
    public List<ValveNode> Next { get; set; } = new List<ValveNode>();
    public string Name { get; set; }
    public int No { get; set; }
    public int Pressure { get; set; }

    public IEnumerable<Node<ValveNode>> NextNodes(int initialDistance) => Next.Select(node => new Node<ValveNode>(node, initialDistance + 1));
    public int Heuristic() => 0;
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        return Equals(obj as ValveNode);
    }
    public bool Equals(ValveNode? other) => Name == other?.Name;
    public static bool operator ==(ValveNode? first, ValveNode? second) => first is null && second is null || first is { } && first.Equals(second);
    public static bool operator !=(ValveNode? first, ValveNode? second) => !(first == second);
    public override int GetHashCode() => Name.GetHashCode();

}

public class DistanceNode<T>
{
    public int Distance { get; set; }
    public T Node { get; set; }
}