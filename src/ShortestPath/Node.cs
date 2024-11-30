namespace ShortestPath;

public sealed record Node<T> where T : IState<T>
{
    public T State { get; }
    public int Distance { get; }
    public int Heuristic { get; }

    public Node(T state, int distance)
    {
        State = state;
        Distance = distance;
        Heuristic = state.Heuristic();
    }

    public IEnumerable<Node<T>> NextNodes()
        => State.NextNodes(Distance);
}