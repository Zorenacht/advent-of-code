using Collections;
using FluentAssertions;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Channels;
using static AoC._2024.Day24;

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


    [Puzzle(answer: null)]
    public long Part2()
    {
        new Computer().Init(Input).Determine();
        return 0;
    }

    public class Computer()
    {
        private Dictionary<string, int> States = [];
        private List<Link> Links = [];

        private Dictionary<string, Node> Previous = [];
        private List<(Link, Link)> AllPossibleSwaps = [];
        private List<(int, int)> SwapsApplied = [];

        public Computer Init(string[] input)
        {
            var groups = input.GroupBy(string.Empty);
            foreach (var line in groups[0])
            {
                var splitted = line.Split(": ");
                States.Add(splitted[0], 0);
                Previous.Add(splitted[0], new Node(splitted[0]));
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

        public void Determine()
        {
            var swaps = Simulate((j) => (1L << j));
            var swaps2 = Simulate((j) => (1L << (j + 1)) - 1);
            var flat1 = swaps.Values.SelectMany(x => x).ToHashSet();
            var flat2 = swaps2.Values.SelectMany(x => x).ToHashSet();
            var counter = new Dictionary<(int, int), int>();
            foreach (var entry in flat1) { if (!counter.TryAdd(entry, 1)) counter[entry]++; }
            foreach (var entry in flat2) { if (!counter.TryAdd(entry, 1)) counter[entry]++; }

            Set((1 << 50) - 1, 1);

            List<List<(int, int)>> valids = [];
            Permute(swaps, [], valids);

            void Permute(Dictionary<string, List<(int, int)>> swaps, List<(int, int)> permutation, List<List<(int, int)>> valids)
            {
                if (permutation.Count == 4)
                {
                    Set((1 << 50) - 1, 0);
                    if (!StateIsValid()) return;
                    Set((1 << 50) - 1, 1);
                    if (!StateIsValid()) return;
                    Set(0, (1 << 50) - 1);
                    if (!StateIsValid()) return;
                    Set(1, (1 << 50) - 1);
                    if (!StateIsValid()) return;



                    valids.Add(permutation.ToList());
                    return;
                }
                foreach (var perm in swaps.Skip(permutation.Count).First().Value)
                {
                    var bak = Links.ToList();
                    var link1 = Links[perm.Item1];
                    var link2 = Links[perm.Item2];
                    Links[perm.Item1] = link1 with { To = link2.To };
                    Links[perm.Item2] = link2 with { To = link1.To };

                    permutation.Add(perm);
                    Permute(swaps, permutation, valids);
                    permutation.RemoveAt(permutation.Count - 1);

                    Links = bak;
                }
            }
        }

        private bool StateIsValid()
        {
            for (int i = 0; i < 10; ++i)
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
            var z = Value('z');
            return x + y == z;
        }

        public Dictionary<string, List<(int, int)>> Simulate(Func<int, long> func)
        {
            var swaps = new List<((int, Link), (int, Link))>();
            var swaps2 = new Dictionary<string, List<(int, int)>>();
            for (int j = 44; j >= 0; --j)
            {
                Set(x: func(j), y: func(j));
                //Set(x: (1L << (j + 1)) - 1, y: 1);
                //if (StateIsValid()) continue;

                Print($"Before {j}");

                // Get all previous
                var nexts = new HashSet<string>();
                var queue = new Queue<string>().With(Symbol('x', j), Symbol('y', j));
                while (queue.TryDequeue(out var element))
                {
                    nexts.Add(element);
                    foreach (var nxt in Previous[element].Next)
                        queue.Enqueue(nxt);
                }




                var changed = new List<(Link, Link)>();
                var lnks = Links.Where(x => nexts.Contains(x.To)).ToArray();
                var change = DetermineValid();
                //AllPossibleSwaps.Add(change);

                (Link, Link) DetermineValid()
                {
                    for (int a = 0; a < Links.Count; a++)
                    {
                        for (int b = a + 1; b < Links.Count; b++)
                        {
                            if (!nexts.Contains(Links[a].To)) continue;
                            var bak = Links.ToList();
                            var link1 = Links[a];
                            var link2 = Links[b];
                            Links[a] = link1 with { To = link2.To };
                            Links[b] = link2 with { To = link1.To };


                            if (StateIsValid())
                            {
                                swaps.Add(((a, link1), (b, link2)));
                                if (!swaps2.TryAdd(GetString(), [(a, b)]))
                                    swaps2[GetString()].Add((a, b));
                                Print($"After {j}");
                                //return (link1, link2);//throw new Exception("FOUDN IT");
                            };
                            Links = bak;
                        }
                    }
                    return (null, null)!;
                }
            }

            return swaps2;

        }

        private static string Symbol(char ch, int num) => $"{ch}{num.ToString().PadLeft(2, '0')}";

        private void Set(long x, long y)
        {
            for (int i = 0; i < 64; ++i)
            {
                var numX = (int)((x >> i) & 1);
                var numY = (int)((y >> i) & 1);
                States[$"x{i.ToString().PadLeft(2, '0')}"] = numX;
                States[$"y{i.ToString().PadLeft(2, '0')}"] = numY;
            }
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
            return $"x{x}+y{y}=z{z}";
        }
    }
};