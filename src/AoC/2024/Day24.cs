using Collections;
using static AoC._2024.Day24;

namespace AoC._2024;

[PuzzleType(PuzzleType.Graph, PuzzleType.Recursion)]
public sealed class Day24 : Day
{
    [Puzzle(answer: 60714423975686)]
    public long Part1()
    {
        var computer = new Computer().Init(Input, (a, b) => 0);
        computer.Calculate();
        return computer.Value('z');
    }

    [Puzzle(answer: "z00,z01,z02,z05")]
    public string Part2Example() => FindCorrectingSwaps(bits: 5, depth: 2, InputExample, (a, b) => a & b);

    [Puzzle(answer: "cgh,frt,pmd,sps,tst,z05,z11,z23")]
    public string Part2() => FindCorrectingSwaps(bits: 44, depth: 4, Input, (a, b) => a + b);

    public string FindCorrectingSwaps(
        int bits,
        int depth,
        string[] input, 
        Func<long, long, long> func)
    {
        var computer = new Computer().Init(input, func);

        List<List<Swap>> valids = [];
        computer.FindValidPermutations(bit: bits, [], valids, depth: depth);

        var solutions = valids
            .Select(valid => new KeyValuePair<string, List<Swap>>(valid
                .SelectMany(x => new string[] { x.Left.To.ToString(), x.Right.To.ToString() })
                .Order().StringJoin(","), valid))
            .GroupBy(x => x.Key)
            .ToDictionary(g => g.Key, g => g.First().Value);
        foreach (var solution in solutions)
        {
            var vld = computer.VerifySwaps(solution.Value, bits);
            if (vld) return solution.Key;
        }
        throw new NotSupportedException();
    }


    public class Computer()
    {
        private Dictionary<string, StateNode> StateNodes = [];
        private Dictionary<string, OperationNode> OperationNodes = [];
        private Func<long, long, long> Operation = null!;
        private string[] EntryPoints { get; set; } = [];

        public Computer Init(string[] input, Func<long, long, long> func)
        {
            Operation = func;
            var groups = input.GroupBy(string.Empty);
            foreach (var line in groups[1])
            {
                var splitted = line.Split(" -> ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var from1 = splitted[0];
                var from2 = splitted[2];
                var op = splitted[1];
                var to = splitted[3];

                var from1Node = new StateNode(from1);
                var from2Node = new StateNode(from2);
                var toNode = new StateNode(to);
                StateNodes.TryAdd(from1, from1Node);
                StateNodes.TryAdd(from2, from2Node);
                StateNodes.TryAdd(to, toNode);
            }
            foreach (var line in groups[0])
            {
                var split = line.Split(": ");
                StateNodes[split[0]].Value = int.Parse(split[1]);
            }
            foreach (var line in groups[1])
            {
                var splitted = line.Split(" -> ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var from1 = splitted[0];
                var from2 = splitted[2];
                var op = splitted[1];
                var to = splitted[3];

                var from1Node = StateNodes[from1];
                var from2Node = StateNodes[from2];
                var toNode = StateNodes[to];

                var operationNode = new OperationNode(from1Node, from2Node, op, toNode);
                OperationNodes.Add(line, operationNode);

                from1Node.Next.Add(operationNode);
                from2Node.Next.Add(operationNode);
                toNode.Previous.Add(operationNode);
            }
            EntryPoints = StateNodes.Where(x => x.Value.Previous.Count == 0).Select(x => x.Key).ToArray();

            return this;
        }

        public bool Calculate()
        {
            var visited = new HashSet<string>();
            var visitedOperation = new Dictionary<string, int>();
            var queue = new Queue<string>().With(EntryPoints);
            while (queue.TryDequeue(out var current))
            {
                var operations = StateNodes[current].Next;
                foreach (var node in operations)
                {
                    if (!visitedOperation.TryAdd(node.KeyString, 1))
                    {
                        if (visitedOperation[node.KeyString] == 1)
                        {
                            visitedOperation[node.KeyString]++;
                            node.To.Value = node.Operation switch
                            {
                                "XOR" => node.From1.Value ^ node.From2.Value,
                                "AND" => node.From1.Value & node.From2.Value,
                                "OR" => node.From1.Value | node.From2.Value,
                                _ => throw new Exception()
                            };
                            queue.Enqueue(node.To.ToString());
                        }
                        else return false;
                    }
                }
            }
            return true;
        }

        public Computer FindValidPermutations(
            int bit,
            List<Swap> swaps,
            List<List<Swap>> valids,
            int depth)
        {
            if (depth == 0)
            {
                if (AllBitCalculationsAreValid(bit))
                {
                    valids.Add(swaps.ToList());
                }
                return this;
            }

            // Starting with the most significant bits, as this means they won't have any effect on the lower significant ones, may or may not matter
            for (int i = bit; i >= 0; --i)
            {
                if (AllBitCalculationsAreValid(i)) continue;
                var corrections = ValidSwaps(i);
                int index = 0;

                foreach (var correction in corrections)
                {
                    if (swaps.Count < 1)
                        Console.WriteLine($"{new string('-', swaps.Count)}Bit {i}, depth {swaps.Count}, {index} out of {corrections.Count - 1} ");

                    correction.Do();
                    swaps.Add(correction);
                    FindValidPermutations(i - 1, swaps, valids, depth - 1);
                    swaps.RemoveAt(swaps.Count - 1);
                    correction.Undo();

                    index++;
                }
                break;
            }
            return this;
        }

        public bool VerifySwaps(List<Swap> swaps, int bit)
        {
            bool isValid = true;
            for (int i = 0; i < swaps.Count; ++i) swaps[i].Do();
            for (int i = bit; i >= 0; --i)
            {
                if (!AllBitCalculationsAreValid(i)) isValid = false;
            }
            if (!(Set(x: (1L << 45) - 1, y: 1L << 0) && IntegerCalculationIsValid())) isValid = false;

            for (int i = swaps.Count - 1; i >= 0; --i) swaps[i].Undo();
            return isValid;
        }

        private bool IntegerCalculationIsValid()
        {
            if (!Calculate()) return false;
            var x = Value('x');
            var y = Value('y');
            var result = Operation(x, y);
            var z = Value('z');
            return result == z;
        }

        private bool AllBitCalculationsAreValid(int i)
            => Set(x: 0L << 0, y: 0L << 0) && IntegerCalculationIsValid()
            && Set(x: 1L << i, y: 0L << 0) && IntegerCalculationIsValid()
            && Set(x: 0L << 0, y: 1L << i) && IntegerCalculationIsValid()
            && Set(x: 1L << i, y: 1L << i) && IntegerCalculationIsValid();

        private List<Swap> ValidSwaps(int i)
        {
            // Apparently it is enough to only check the first and few few nodes
            int nodes = 4;

            // Get swap candidates from the start forwards
            //                 and from the end backwards
            var forward = new HashSet<string>();
            var queue = new Queue<string>().With(Symbol('x', i), Symbol('y', i));
            while (queue.TryDequeue(out var current) && forward.Count() < nodes)
            {
                var operations = StateNodes[current].Next;
                foreach (var node in operations)
                {
                    if (forward.Add(node.KeyString))
                        queue.Enqueue(node.To.ToString());
                }
            }
            var backward = new HashSet<string>();
            queue = new Queue<string>().With(Symbol('z', i), Symbol('z', i + 1));
            while (queue.TryDequeue(out var current) && backward.Count() < nodes)
            {
                if (!StateNodes.TryGetValue(current, out var nde)) continue;
                var operations = nde.Previous;
                foreach (var node in operations)
                {
                    if (backward.Add(node.KeyString))
                    {
                        queue.Enqueue(node.From1.ToString());
                        queue.Enqueue(node.From2.ToString());
                    }
                }
            }

            var swaps = new List<Swap>();
            foreach (var fw in forward)
            {
                foreach (var bw in backward)
                {
                    var swap = new Swap(OperationNodes[fw], OperationNodes[bw]);
                    swap.Do();
                    if (AllBitCalculationsAreValid(i))
                        swaps.Add(swap);
                    swap.Undo();
                }
            }
            return swaps;
        }

        private static string Symbol(char ch, int bit) => $"{ch}{bit.ToString().PadLeft(2, '0')}";

        private bool Set(long x, long y)
        {
            for (int i = 0; i < 64; ++i)
            {
                var numX = (int)((x >> i) & 1);
                var numY = (int)((y >> i) & 1);
                var keyX = $"x{i.ToString().PadLeft(2, '0')}";
                var keyY = $"y{i.ToString().PadLeft(2, '0')}";
                if (StateNodes.TryGetValue(keyX, out var nodeX)) nodeX.Value = numX;
                else return true;
                if (StateNodes.TryGetValue(keyY, out var nodeY)) nodeY.Value = numY;
                else return true;
            }
            return true;
        }

        public long Value(char symbol)
        {
            long result = 0;
            for (int i = 0; i < 64; ++i)
            {
                var key = $"{symbol}{i.ToString().PadLeft(2, '0')}";
                if (StateNodes.TryGetValue(key, out var node)) result = result + ((long)node.Value << i);
                else break;
            }
            return result;
        }

        private string GetString()
        {
            var x = Value('x');
            var y = Value('y');
            var z = Value('z');
            return $"x{x} op y{y}=z{z}";
        }
    }

    public record StateNode(string Symbol)
    {
        public int Value { get; set; } = 0;
        public HashSet<OperationNode> Previous { get; init; } = [];
        public List<OperationNode> Next { get; init; } = [];
        public override string ToString() => Symbol;
    }

    public class OperationNode
    {
        public OperationNode(StateNode from1, StateNode from2, string operation, StateNode to)
        {
            From1 = from1;
            From2 = from2;
            Operation = operation;
            To = to;
            KeyString = $"{From1} {Operation} {From2} -> {To}";
        }

        public StateNode From1 { get; set; }
        public StateNode From2 { get; set; }
        public string Operation { get; init; }
        public StateNode To { get; set; }
        public string KeyString { get; }
        public override string ToString() => $"{To}";

    }

    public record Swap(OperationNode Left, OperationNode Right)
    {
        private StateNode LeftTo = Left.To;
        private StateNode RightTo = Right.To;

        public void Do()
        {
            LeftTo.Previous.Remove(Left);
            LeftTo.Previous.Add(Right);
            Left.To = RightTo;
            RightTo.Previous.Remove(Right);
            RightTo.Previous.Add(Left);
            Right.To = LeftTo;
        }

        public void Undo()
        {
            LeftTo.Previous.Remove(Right);
            LeftTo.Previous.Add(Left);
            Left.To = LeftTo;
            RightTo.Previous.Remove(Left);
            RightTo.Previous.Add(Right);
            Right.To = RightTo;
        }

        public override string ToString() => $"({Left},{Right})";
    }
};