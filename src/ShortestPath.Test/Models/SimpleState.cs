namespace ShortestPath.Test.Models;

public sealed record SimpleState : IState<SimpleState>, IEquatable<SimpleState>
{
    public int Pos { get; set; }

    public bool Equals(SimpleState? other)
    {
        return Pos == other?.Pos;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Pos);
    }

    public IEnumerable<Node<SimpleState>> NextNodes(int initialDistance)
    {
        for (int i = 1; i <= 3; i++)
        {
            yield return new Node<SimpleState>(
                state: new SimpleState() { Pos = Pos + i },
                distance: initialDistance + (int)Math.Pow(10, i - 1));
        }
    }

    public int Heuristic() => 0;
}