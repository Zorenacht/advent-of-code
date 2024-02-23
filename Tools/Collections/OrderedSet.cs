using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Tools.Collections;

/// <summary>
/// A set of unique elements preserving order of addition.
/// Constant average-case complexity for Add, AddOrMoveToEnd, Remove and Contains.
/// </summary>
/// <typeparam name="T">
/// The type of the items in the ordered set.
/// </typeparam>
[DebuggerDisplay("Count = {Count}")]
public sealed class OrderedSet<T> : ICollection<T> where T : notnull
{
    private readonly LinkedList<T> _linkedList;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Dictionary<T, LinkedListNode<T>> _nodes;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int _version = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderedSet{T}"/> class.
    /// </summary>
    public OrderedSet()
    {
        _linkedList = [];
        _nodes = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderedSet{T}"/> class with the items in <paramref name="initial"/>.
    /// </summary>
    /// <param name="initial">The initial values.</param>
    public OrderedSet(IEnumerable<T> initial) : this()
    {
        foreach (var element in initial)
        {
            Add(element);
        }
    }

    /// <inheritdoc/>
    public int Count => _linkedList.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <summary>
    /// If the <paramref name="item"/> exists in the set, add the <paramref name="item"/> to the end of the set.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>True if the <paramref name="item"/> was added. False if not.</returns>
    public bool Add(T item)
    {
        if (!_nodes.ContainsKey(item))
        {
            var node = _linkedList.AddLast(item);
            _nodes.Add(item, node);
            ++_version;
            return true;
        }
        else return false;
    }

    /// <inheritdoc/>
    void ICollection<T>.Add(T item) => Add(item);

    /// <summary>
    /// If the <paramref name="item"/> does not exist in this set, add <paramref name="item"/> to the end of the set. Else, move the existing <paramref name="item"/> to the end of the set.
    /// </summary>
    /// <param name="item">The item to add or move.</param>
    public void AddOrMoveToEnd(T item)
    {
        Remove(item);
        var node = _linkedList.AddLast(item);
        _nodes.Add(item, node);
        ++_version; // Not optimal, can increase version two times in this function
    }

    /// <inheritdoc/>
    public bool Remove(T item)
    {
        if (_nodes.TryGetValue(item, out var node))
        {
            _linkedList.Remove(node);
            _nodes.Remove(node.Value);
            ++_version;
            return true;
        }
        else return false;
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _linkedList.Clear();
        _nodes.Clear();
        ++_version;
    }

    /// <inheritdoc/>
    [Pure]
    public bool Contains(T item) => _nodes.ContainsKey(item);

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex) => _linkedList.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    [Pure]
    public IEnumerator<T> GetEnumerator() => _linkedList.GetEnumerator();

    /// <inheritdoc/>
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Creates a reverse iterator.
    /// </summary>
    /// <returns> The reverse iterator. </returns>
    [Pure]
    public IEnumerable<T> ReverseEnumerable() => new Reverse(this);

    private sealed class Reverse(OrderedSet<T> set) : IEnumerable<T>
    {
        private readonly OrderedSet<T> _set = set;

        [Pure]
        public IEnumerator<T> GetEnumerator() => new ReversedIterator(_set);

        [Pure]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // Largely based on the .Net 8.0 LinkedList.Enumerator implementation
        private struct ReversedIterator : IEnumerator<T>
        {
            private readonly OrderedSet<T> _set;
            private LinkedListNode<T>? _node;
            private readonly int _version;
            private T? _current;

            internal ReversedIterator(OrderedSet<T> set)
            {
                _set = set;
                _node = set._linkedList.Last;
                _version = set._version;
                _current = default;
            }

            public readonly T Current => _current!;

            readonly object? IEnumerator.Current => Current;

            public bool MoveNext()
            {
                PreventModifications();
                if (_node == null)
                {
                    return false;
                }

                _current = _node.Value;
                _node = _node.Previous;
                return true;
            }

            private readonly void PreventModifications()
            {
                if (_version != _set._version)
                {
                    throw new InvalidOperationException(@"Collection was modified after the enumerator was instantiated.");
                }
            }

            void IEnumerator.Reset() => throw new NotSupportedException();

            public readonly void Dispose() { /* Nothing to dispose. */ }
        }
    }
}
