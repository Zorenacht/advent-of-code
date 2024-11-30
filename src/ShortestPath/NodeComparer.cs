using System.Diagnostics.CodeAnalysis;

namespace ShortestPath;

public class NodeComparer<T> : IEqualityComparer<Node<T>> where T : IState<T>
{
    public NodeComparer()
    {
        var getHashCodeOverridden = typeof(T).GetMethod("GetHashCode")?.DeclaringType == typeof(T);
        if (!getHashCodeOverridden)
        {
            throw new NotImplementedException($"Model {typeof(T).Name} did not override GetHashCode().");
        }
    }

    public bool Equals(Node<T>? x, Node<T>? y)
        => x is null && y is null ||
           x is { } && y is { } && x.State.Equals(y.State);

    public int GetHashCode([DisallowNull] Node<T> obj) => obj.State.GetHashCode();
}