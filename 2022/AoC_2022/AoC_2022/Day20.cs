using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace AoC_2022;


public sealed partial class Day20 : Day
{
    [Test]
    public void Example() => Geodes.Parse(InputExample).Mix().Top3().Should().Be(3);
    [Test]
    public void Part1() => Geodes.Parse(InputPart1).Mix().Top3().Should().Be(0);

    [Test]
    public void ExampleP2() => Geodes.Parse(InputExample).Mix().Top3().Should().Be(-100);
    [Test]
    public void Part2() => Geodes.Parse(InputPart1).Mix().Top3().Should().Be(-100);

    private class Geodes
    {
        //private readonly List<Node> Sequence;
        private readonly List<Node> OriginalOrder;

        public Geodes(List<Node> original)
        {
            OriginalOrder = original;
        }

        public int Top3()
        {
            var node = OriginalOrder.First(x => x.Value == 0);
            var list = new List<int>() { 
                1000, 
                2000, 
                3000
            }.Order();
            int count = 0;
            var sum = new List<int>();
            while(sum.Count != 3)
            {
                count = (count + 1);
                node = node.Next;
                if (list.Contains(count))
                {
                    sum.Add(node.Value);
                }
            }
            return sum.Sum();
        }

        public bool LoopTest(Node head)
        {
            var current = head;
            for(int i =0; i< OriginalOrder.Count * 10; i++)
            {
                current = current.Prev;
            }
            return current == head;
        }

        public int[] LinkedListToArray()
        {
            var list = new List<int>();
            var current = OriginalOrder[0];
            for (int i = 0; i < OriginalOrder.Count; i++)
            {
                list.Add(current.Value);
                current = current.Next;
            }
            return list.ToArray();
        }

        public Geodes Mix()
        {
            foreach (var node in OriginalOrder)
            {
                int counter = 0;
                var current = node;
                if (node.Value == 0) continue;
                while (counter != node.Value)
                {
                    var sign = Math.Sign(node.Value);
                    if (sign > 0) current = current.Next;
                    else if (sign < 0) current = current.Prev;
                    counter += Math.Sign(node.Value);
                }
                Remove(node);
                if (Math.Sign(node.Value) > 0) InsertBefore(current.Next!, node);
                if (Math.Sign(node.Value) < 0) InsertBefore(current, node);
            }
            return this;
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

        public static Geodes Parse(string[] lines)
        {
            var head = new Node() { Value = int.Parse(lines[0]) };
            var original = new List<Node>() { head };
            Node? old = head;
            Node? current = head;
            foreach (var line in lines.Skip(1))
            {
                current = new Node() { Value = int.Parse(line), Prev = old };
                old.Next = current;
                original.Add(current);
                old = current;
            }
            current.Next = head;
            head.Prev = current;

            return new Geodes(original);
        }
    }

    private class Node
    {
        public Node? Prev { get; set; }
        public Node? Next { get; set; }
        public int Value;
    }
}
