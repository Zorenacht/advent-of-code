using FluentAssertions;
using System.Diagnostics;

namespace AoC._2024;

public sealed class Day17 : Day
{
    private record Instruction(int OpCode, int Operand)
    {
        public State Execute(State state)
        {
            return OpCode switch
            {
                //0 => state.Move() with { A = state.A / (2 << Combo(state)) },
                0 => state.Move() with { A = state.A >> Combo(state) },
                1 => state.Move() with { B = state.B ^ Operand },
                2 => state.Move() with { B = Combo(state) % 8 },
                3 => state with { Pointer = state.A == 0 ? state.Pointer + 2 : Operand },
                4 => state.Move() with { B = state.B ^ state.C },
                5 => state.Move() with { Output = state.Output == string.Empty ? (Combo(state) % 8).ToString() : state.Output + "," + Combo(state) % 8 },
                6 => state.Move() with { B = state.A >> Combo(state) },
                7 => state.Move() with { C = state.A >> Combo(state) },
                _ => throw new UnreachableException("Should not be reachable")
            };
        }
        
        private int Combo(State state) => Operand switch
        {
            0 or 1 or 2 or 3 => Operand,
            4 => state.A,
            5 => state.B,
            6 => state.C,
            _ => throw new UnreachableException("Should not be reachable")
        };
    };
    
    private record State(int A, int B, int C, string Output, int Pointer)
    {
        public State Move() => this with { Pointer = Pointer + 2 };
    };
    
    [Puzzle(answer: null)]
    public string Part1()
    {
        var lines = Input;
        return Execute(
            lines[0].Ints()[0],
            lines[1].Ints()[0],
            lines[2].Ints()[0],
            lines[4].Ints()).Output;
    }
    
    [Test]
    public void Example() => Execute(729, 0, 0, [0, 1, 5, 4, 3, 0]).Output.Should().Be("4,6,3,5,6,3,5,2,1,0");
    
    [Test]
    public void Example1() => Execute(0, 0, 9, [2, 6]).B.Should().Be(1);
    
    [Test]
    public void Example2() => Execute(10, 0, 0, [5, 0, 5, 1, 5, 4]).Output.Should().Be("0,1,2");
    
    [Test]
    public void Example3() => Execute(2024, 0, 0, [0, 1, 5, 4, 3, 0]).Output.Should().Be("4,2,5,6,7,7,7,7,3,1,0");
    
    [Test]
    public void Example4() => Execute(0, 29, 0, [1, 7]).B.Should().Be(26);
    
    [Test]
    public void Example5() => Execute(0, 2024, 43690, [4, 0]).B.Should().Be(44354);
    
    private State Execute(int a, int b, int c, int[] instr)
    {
        long result = 0;
        var state = new State(a, b, c, string.Empty, 0);
        var instructions = new List<Instruction>();
        for (int i = 0; i < instr.Length - 1; i++)
        {
            instructions.Add(new Instruction(instr[i], instr[i + 1]));
        }
        
        Console.WriteLine($"A={state.A}, B={state.B}, C={state.C}, Output={state.Output}, Pointer={state.Pointer}");
        while (state.Pointer < instructions.Count)
        {
            state = instructions[state.Pointer].Execute(state);
            Console.WriteLine($"A={state.A}, B={state.B}, C={state.C}, Output={state.Output}, Pointer={state.Pointer}");
        }
        return state;
    }
    
    [Puzzle(answer: null)]
    public long Part2()
    {
        return 0;
    }
};