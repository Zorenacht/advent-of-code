namespace AoC_2022;


public sealed partial class Day20 : Day
{
    [Test]
    public void Example() => Mixir.Parse(InputExample, 1).Mix(1).Top3().Should().Be(3);
    [Test]
    public void Part1() => Mixir.Parse(InputPart1, 1).Mix(1).Top3().Should().Be(4151);

    [Test]
    public void ExampleP2() => Mixir.Parse(InputExample, 811589153).Mix(10).Top3().Should().Be(1623178306L);
    [Test]
    public void Part2() => Mixir.Parse(InputPart1, 811589153).Mix(10).Top3().Should().Be(7848878698663L);

    private class Mixir
    {
        //private readonly List<Node> Sequence;
        private readonly List<Node> OriginalOrder;

        public Mixir(List<Node> original)
        {
            OriginalOrder = original;
        }

        public long Top3()
        {
            Node node = OriginalOrder.First(x => x.Value == 0);
            long sum = 0;
            for (int i = 1; i <= 3000; i++)
            {
                node = node.Next!;
                if (i % 1000 == 0)
                {
                    sum += node.Value;
                }
            }
            return sum;
        }

        public Mixir Mix(int times)
        {
            for (int j = 0; j < times; j++)
            {
                Mix();
            }
            return this;
        }

        private void Mix()
        {
            foreach (Node node in OriginalOrder)
            {
                Node current = node;
                var sign = Math.Sign(node.Value);
                if (node.Value == 0)
                {
                    continue;
                }
                for (int i = 0; i < Math.Abs(node.Value) % (OriginalOrder.Count - 1); i += 1)
                {
                    if (sign > 0) current = current.Next!;
                    else if (sign < 0) current = current.Prev!;
                }
                Remove(node);
                if (Math.Sign(node.Value) > 0) InsertBefore(current.Next!, node);
                if (Math.Sign(node.Value) < 0) InsertBefore(current, node);
            }
        }

        private void Remove(Node node)
        {
            node.Next!.Prev = node.Prev;
            node.Prev!.Next = node.Next;
        }

        private void InsertBefore(Node before, Node insert)
        {
            insert.Next = before;
            insert.Prev = before.Prev;
            before.Prev!.Next = insert;
            before.Prev = insert;
        }

        public static Mixir Parse(string[] lines, int key)
        {
            var head = new Node() { Value = long.Parse(lines[0]) * key };
            var original = new List<Node>() { head };
            Node? old = head;
            Node? current = head;
            foreach (var line in lines.Skip(1))
            {
                current = new Node() { Value = long.Parse(line) * key, Prev = old };
                old.Next = current;
                original.Add(current);
                old = current;
            }
            current.Next = head;
            head.Prev = current;

            return new Mixir(original);
        }
    }

    private class Node
    {
        public Node? Prev { get; set; }
        public Node? Next { get; set; }
        public long Value;
    }
}
