using Tools.Collections;

namespace Collections.OrderedSet_specs;

internal readonly struct Struct(int number)
{
    public int Number { get; } = number;
};

internal sealed record Record(int Number);

internal sealed class Class(int number)
{
    public int Number { get; } = number;
    public override int GetHashCode() => Number.GetHashCode();
    public override bool Equals(object? obj) => obj is Class clss && Number == clss.Number;
}

internal static class TestCases
{
    public static IEnumerable<int[]> Integers() => [[0, 1, 2, 3, 4]];
    public static IEnumerable<Struct[]> Structs() => [new Struct[] { new(0), new(1), new(2), new(3), new(4) }];
    public static IEnumerable<Record[]> Records() => [new Record[] { new(0), new(1), new(2), new(3), new(4) }];
    public static IEnumerable<Class[]> Classes() => [new Class[] { new(0), new(1), new(2), new(3), new(4) }];
}

public sealed class Add
{
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void adds_new_item_to_end<T>(T[] items) where T : notnull
    {
        var set = new OrderedSet<T>(items[..^1]);
        set.Add(items[^1]).Should().BeTrue();
        set.Should().BeEquivalentTo(items, o => o.WithStrictOrdering());
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void does_nothing_for_existing_item<T>(T[] items) where T : notnull
    {
        var set = new OrderedSet<T>(items);
        set.Add(items[0]).Should().BeFalse();
        set.Should().BeEquivalentTo(items, o => o.WithStrictOrdering());
    }
}

public sealed class AddOrMoveToEnd
{
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void adds_new_item_to_end<T>(T[] items) where T : notnull
    {
        var list = new OrderedSet<T>(items[..^1]);
        list.AddOrMoveToEnd(items[^1]);
        list.Should().BeEquivalentTo(items, o => o.WithStrictOrdering());
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void moves_existing_item_to_end<T>(T[] items) where T : notnull
    {
        var list = new OrderedSet<T>(items);
        list.AddOrMoveToEnd(items[0]);
        list.Should().BeEquivalentTo([.. items[1..], items[0]], o => o.WithStrictOrdering());
    }
}

public sealed class Remove
{
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void removes_existing_item<T>(T[] items) where T : notnull
    {
        var list = new OrderedSet<T>(items);
        var result = list.Remove(items[0]);
        result.Should().BeTrue();
        list.Should().BeEquivalentTo(items[1..], o => o.WithStrictOrdering());
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void does_nothing_for_new_item<T>(T[] items) where T : notnull
    {
        var list = new OrderedSet<T>(items[..^1]);
        var result = list.Remove(items[^1]);
        result.Should().BeFalse();
        list.Should().BeEquivalentTo(items[..^1], o => o.WithStrictOrdering());
    }
}

public sealed class Clear
{
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void removes_all_items<T>(T[] items) where T : notnull
    {
        var list = new OrderedSet<T>(items);
        list.Clear();
        list.Should().BeEmpty();
    }
}

public sealed class Contains
{
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void returns_true_for_existing_item<T>(T[] items) where T : notnull
    {
        var list = new OrderedSet<T>(items);
        var result = list.Contains(items[1]);
        result.Should().BeTrue();
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void returns_false_for_new_item<T>(T[] items) where T : notnull
    {
        var list = new OrderedSet<T>(items[..^1]);
        var result = list.Contains(items[^1]);
        result.Should().BeFalse();
    }
}

public sealed class ReverseEnumerable
{
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void loops_in_reverse<T>(T[] items) where T : notnull
    {
        var list = new OrderedSet<T>(items);
        var result = list.ReverseEnumerable();
        result.Should().BeEquivalentTo(items.Reverse(), o => o.WithStrictOrdering());
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void loop_throws_when_item_added<T>(T[] items) where T : notnull
    {
        var set = new OrderedSet<T>(items[..^1]);
        var addNewWhileLooping = () =>
        {
            foreach (var i in set.ReverseEnumerable())
            {
                if (!set.Contains(items[^1])) set.Add(items[^1]);
            }
        };
        addNewWhileLooping.Should().Throw<InvalidOperationException>("Collection was modified after the enumerator was instantiated.");
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void loop_throws_when_item_added_by_AddOrMoveToEnd<T>(T[] items) where T : notnull
    {
        var set = new OrderedSet<T>(items[..^1]);
        var addNewWhileLooping = () =>
        {
            foreach (var i in set.ReverseEnumerable())
            {
                if (!set.Contains(items[^1])) set.AddOrMoveToEnd(items[^1]);
            }
        };
        addNewWhileLooping.Should().Throw<InvalidOperationException>("Collection was modified after the enumerator was instantiated.");
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void loop_throws_when_item_moved_to_back<T>(T[] items) where T : notnull
    {
        var set = new OrderedSet<T>(items);
        var addNewWhileLooping = () =>
        {
            foreach (var i in set.ReverseEnumerable())
            {
                set.AddOrMoveToEnd(i);
            }
        };
        addNewWhileLooping.Should().Throw<InvalidOperationException>("Collection was modified after the enumerator was instantiated.");
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void loop_throws_when_item_removed<T>(T[] items) where T : notnull
    {
        var set = new OrderedSet<T>(items);
        var addNewWhileLooping = () =>
        {
            foreach (var i in set.ReverseEnumerable())
            {
                set.Remove(i);
            }
        };
        addNewWhileLooping.Should().Throw<InvalidOperationException>("Collection was modified after the enumerator was instantiated.");
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void loop_throws_when_cleared<T>(T[] items) where T : notnull
    {
        var set = new OrderedSet<T>(items);
        var addNewWhileLooping = () =>
        {
            foreach (var i in set.ReverseEnumerable())
            {
                set.Clear();
            }
        };
        addNewWhileLooping.Should().Throw<InvalidOperationException>("Collection was modified after the enumerator was instantiated.");
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void loop_doesnt_throw_when_add_does_nothing<T>(T[] items) where T : notnull
    {
        var set = new OrderedSet<T>(items);
        var addNewWhileLooping = () =>
        {
            foreach (var i in set.ReverseEnumerable())
            {
                set.Add(i);
            }
        };
        addNewWhileLooping.Should().NotThrow<Exception>();
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.Integers))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Structs))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Records))]
    [TestCaseSource(typeof(TestCases), nameof(TestCases.Classes))]
    public void loop_doesnt_throw_when_remove_does_nothing<T>(T[] items) where T : notnull
    {
        var set = new OrderedSet<T>(items[..^1]);
        var addNewWhileLooping = () =>
        {
            foreach (var i in set.ReverseEnumerable())
            {
                set.Remove(items[^1]);
            }
        };
        addNewWhileLooping.Should().NotThrow<Exception>();
    }
}