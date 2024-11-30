namespace ShortestPath;

public interface IState<T> : IEquatable<T> where T : IState<T>
{
    public int Heuristic();
    public IEnumerable<Node<T>> NextNodes(int initialDistance);
}