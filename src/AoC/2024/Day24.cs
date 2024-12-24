using Collections;
using FluentAssertions;
using MathNet.Numerics;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using Tools;
using static AoC._2024.Day24;
using static System.Net.Mime.MediaTypeNames;

namespace AoC._2024;

public sealed class Day24 : Day
{
    [Puzzle(answer: 60714423975686)]
    public long Part1()
    {
        long result = 0;
        var groups = Input.GroupBy(string.Empty);
        var states = new Dictionary<string, int>();
        var links = new List<(string From1, string Type, string From2, string To)>();
        foreach (var line in groups[0])
        {
            var splitted = line.Split(": ");
            states.Add(splitted[0], int.Parse(splitted[1]));
        }
        foreach (var line in groups[1])
        {
            var splitted = line.Split(" -> ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            states.TryAdd(splitted[0], 0);
            states.TryAdd(splitted[2], 0);
            states.TryAdd(splitted[3], 0);
            var type = splitted[1];
            links.Add((splitted[0], splitted[1], splitted[2], splitted[3]));
        }


        for (int i = 0; i < 100; ++i)
        {
            foreach (var link in links)
            {
                states[link.To] = link.Type switch
                {
                    "XOR" => states[link.From1] ^ states[link.From2],
                    "AND" => states[link.From1] & states[link.From2],
                    "OR" => states[link.From1] | states[link.From2],
                    _ => throw new Exception()
                };
            }
        }

        return Long('z', states);

        static long Long(char symbol, Dictionary<string, int> states)
        {
            long result = 0;
            for (int i = 0; i < 64; ++i)
            {
                if (states.TryGetValue($"z{i.ToString().PadLeft(2, '0')}", out var bit))
                    result = result + ((long)bit << i);
                else break;
            }
            return result;
        }
    }

    public record Link(string From1, string Operator, string From2, string To)
    {
        public int Calculate(int from1, int from2)
        {
            return Operator switch
            {
                "XOR" => from1 ^ from2,
                "AND" => from1 & from2,
                "OR" => from1 | from2,
                _ => throw new Exception()
            };

        }
    }

    public record Node(string Symbol)
    {
        public HashSet<string> Next { get; init; } = [];
        public HashSet<string> Previous { get; init; } = [];
    }

    // not cgh,frt,jkm,pmd,rrr,z05,z11,z23
    // not cgh,frt,jkm,pmd,rrr,z05,z11,z23
    [Puzzle(answer: null)]
    public string Part2()
    {
        List<List<(int, int)>> valids = [];

        int bits = 5; int depth = 2;
        var computer = new Computer().Init(InputExample, (a, b) => a & b).Permute(bit: bits, [], valids, depth: depth);

        //int bits = 44; int depth = 4;
        //var computer = new Computer().Init(Input, (a, b) => a + b);

        var original = computer.Links.ToList();

        computer.Permute(bit: bits, [], valids, depth: depth);

        //new Computer().Init(Input).Permute(44, [], valids);
        //Console.WriteLine($"The swap {valids[0].StringJoin(",")} is: {computer.VerifySwaps(valids[0], 5)}");
        var solutions = new List<string>();
        foreach (var valid in valids)
        {
            var vld = computer.VerifySwaps(valid, bits);

            Console.WriteLine($"The swap {valid.StringJoin(",")} is: {vld}");
            if (vld) solutions.Add(valid
                    .SelectMany(x => new string[] { computer.Links[x.Item1].To, computer.Links[x.Item2].To })
                    .Order().StringJoin(","));
        }
        return solutions.First();
    }

    public class Computer()
    {
        private Dictionary<string, int> States = [];
        public List<Link> Links = [];

        private Dictionary<string, Node> Previous = [];
        private List<(Link, Link)> AllPossibleSwaps = [];
        private List<(int, int)> SwapsApplied = [];
        private Func<long, long, long> Operation;

        private int ForwardComputations = 10;

        public Computer Init(string[] input, Func<long, long, long> operation)
        {
            Operation = operation;
            var groups = input.GroupBy(string.Empty);
            foreach (var line in groups[0])
            {
                var splitted = line.Split(": ");
                States.Add(splitted[0], 0);
                Previous.Add(splitted[0], new Node(splitted[0]));
            }
            for (int i = 0; i < 64; ++i)
            {
                States[$"x{i.ToString().PadLeft(2, '0')}"] = 0;
                States[$"y{i.ToString().PadLeft(2, '0')}"] = 0;
            }
            foreach (var line in groups[1])
            {
                var splitted = line.Split(" -> ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                States.TryAdd(splitted[0], 0);
                States.TryAdd(splitted[2], 0);
                States.TryAdd(splitted[3], 0);
                Previous.TryAdd(splitted[0], new Node(splitted[0]));
                Previous.TryAdd(splitted[2], new Node(splitted[2]));
                Previous.TryAdd(splitted[3], new Node(splitted[3]));
                Previous[splitted[3]].Previous.Add(splitted[0]);
                Previous[splitted[3]].Previous.Add(splitted[2]);
                Previous[splitted[0]].Next.Add(splitted[3]);
                Previous[splitted[2]].Next.Add(splitted[3]);
                Links.Add(new(splitted[0], splitted[1], splitted[2], splitted[3]));
            }
            return this;
        }

        private bool CalculationIsValid()
        {
            for (int i = 0; i < ForwardComputations; ++i)
            {
                foreach (var link in Links)
                {
                    States[link.To] = link.Operator switch
                    {
                        "XOR" => States[link.From1] ^ States[link.From2],
                        "AND" => States[link.From1] & States[link.From2],
                        "OR" => States[link.From1] | States[link.From2],
                        _ => throw new Exception()
                    };
                }
            }
            var x = Value('x');
            var y = Value('y');
            var result = Operation(x, y);
            var z = Value('z');
            return result == z;
        }

        public bool AllCalculationsValid(int i)
            => Set(x: 0L << 0, y: 0L << 0) && CalculationIsValid()
            && Set(x: 1L << i, y: 0L << 0) && CalculationIsValid()
            && Set(x: 0L << 0, y: 1L << i) && CalculationIsValid()
            && Set(x: 1L << i, y: 1L << i) && CalculationIsValid();

        public Computer Permute(
            int bit,
            List<(int, int)> permutation,
            List<List<(int, int)>> valids,
            int depth)
        {
            if (depth == 0)
            {
                if (AllCalculationsValid(bit)) valids.Add(permutation.ToList());
                return this;
            }
            //foreach (var perm in swaps)
            //{

            for (int i = bit; i >= 0; --i)
            {
                if (AllCalculationsValid(i)) continue;
                var corrections = CorrectingSwaps(i);
                int index = 0;

                foreach (var correction in corrections)
                {
                    if(permutation.Count < 2)
                        Console.WriteLine($"{new string('-', permutation.Count)}Bit {bit}, dtgo {permutation.Count}, {index} out of {corrections.Count - 1} ");
                    var bak = Links.ToList();
                    var link1 = Links[correction.Item1];
                    var link2 = Links[correction.Item2];
                    Links[correction.Item1] = link1 with { To = link2.To };
                    Links[correction.Item2] = link2 with { To = link1.To };

                    permutation.Add(correction);
                    Permute(i - 1, permutation, valids, depth - 1);
                    permutation.RemoveAt(permutation.Count - 1);

                    Links = bak;
                    index++;
                }
                break;
            }
            return this;
        }

        public bool VerifySwaps(List<(int, int)> swaps, int bit)
        {
            var backup = Links.ToList();
            bool isValid = true;
            foreach (var swap in swaps)
            {
                var link1 = Links[swap.Item1];
                var link2 = Links[swap.Item2];
                Links[swap.Item1] = link1 with { To = link2.To };
                Links[swap.Item2] = link2 with { To = link1.To };
            }
            for (int i = bit; i >= 0; --i)
            {
                if (!AllCalculationsValid(i)) isValid = false;
            }
            if (!(Set(x: (1L << 45) - 1, y: 1L << 0) && CalculationIsValid())) isValid = false;

            Links = backup;
            return isValid;
        }

        // Find all swaps valid for bit i
        private List<(int, int)> CorrectingSwaps(int i)
        {
            var nexts = new HashSet<string>();
            var previous = new HashSet<string>();
            var swaps2 = new List<(int, int)>();

            var queue = new Queue<string>().With(Symbol('x', i), Symbol('y', i));
            while (queue.TryDequeue(out var element))
            {
                if (!Previous.ContainsKey(element)) continue;
                nexts.Add(element);
                foreach (var nxt in Previous[element].Next)
                    queue.Enqueue(nxt);
            }
            queue = new Queue<string>().With(Symbol('z', i), Symbol('z', i + 1));
            while (queue.TryDequeue(out var element))
            {
                if (!Previous.ContainsKey(element)) continue;
                previous.Add(element);
                foreach (var nxt in Previous[element].Previous)
                    queue.Enqueue(nxt);
            }

            int calculations = 0;
            for (int a = 0; a < Links.Count; a++)
            {
                for (int b = 0; b < Links.Count; b++)
                {
                    if (!nexts.Contains(Links[a].To) || !previous.Contains(Links[b].To)) continue;
                    //var bak = Links.ToList();
                    var link1 = Links[a];
                    var link2 = Links[b];
                    Links[a] = link1 with { To = link2.To };
                    Links[b] = link2 with { To = link1.To };


                    if (AllCalculationsValid(i))
                    {
                        swaps2.Add((a, b));
                    }
                    Links[a] = link1;
                    Links[b] = link2;
                    calculations++;
                }
            }
            return swaps2;
        }

        private static string Symbol(char ch, int bit) => $"{ch}{bit.ToString().PadLeft(2, '0')}";

        private bool Set(long x, long y)
        {
            for (int i = 0; i < 64; ++i)
            {
                var numX = (int)((x >> i) & 1);
                var numY = (int)((y >> i) & 1);
                States[$"x{i.ToString().PadLeft(2, '0')}"] = numX;
                States[$"y{i.ToString().PadLeft(2, '0')}"] = numY;
            }
            return true;
        }

        private long Value(char symbol)
        {
            long result = 0;
            for (int i = 0; i < 64; ++i)
            {
                if (States.TryGetValue($"{symbol}{i.ToString().PadLeft(2, '0')}", out var bit))
                    result = result + ((long)bit << i);
                else break;
            }
            return result;
        }

        private void Print(string prefix = "")
        {
            Console.WriteLine($"{prefix}|{GetString()}");
        }

        private string GetString()
        {
            var x = Value('x');
            var y = Value('y');
            var z = Value('z');
            return $"x{x} op y{y}=z{z}";
        }
    }
};