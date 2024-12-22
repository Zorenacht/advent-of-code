using System.Diagnostics;

namespace AoC._2024;

[PuzzleType(PuzzleType.Compute, PuzzleType.Manual)]
public sealed class Day17 : Day
{
    [Puzzle(answer: "7,5,4,3,4,5,3,4,6")]
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
    
    [Test]
    public void ExamplePart2() => FindMinReplicating([0, 3, 5, 4, 3, 0]).Should().Be(117440);
    
    [Puzzle(answer: 164278899142333)]
    public long Part2() => FindMinReplicating(Input[4].Ints());
    
    private static State Execute(long a, long b, long c, int[] intstructions)
    {
        var instructions = new List<Instruction>();
        for (int i = 0; i < intstructions.Length - 1; i++)
            instructions.Add(new Instruction(intstructions[i], intstructions[i + 1]));
        return Execute(a, b, c, instructions);
    }
    
    private static State Execute(long a, long b, long c, List<Instruction> instructions)
    {
        var state = new State(a, b, c, string.Empty, 0);
        while (state.Pointer < instructions.Count)
            state = instructions[state.Pointer].Execute(state);
        return state;
    }
    
    private static long FindMinReplicating(int[] intstructions)
    {
        var instructions = new List<Instruction>();
        for (int i = 0; i < intstructions.Length - 1; i++)
        {
            instructions.Add(new Instruction(intstructions[i], intstructions[i + 1]));
        }
        
        var range = Enumerable.Range(0, 600).ToArray();
        var candidatesAll = range.Select(x => (long)x).ToList();
        for (int k = 1; k < instructions.Count; ++k)
        {
            var goal = string.Join(",", intstructions[..(k + 1)]);
            candidatesAll = range
                .SelectMany(x => candidatesAll.Select(prev => prev + ((long)x << (3 * k))))
                .Distinct()
                .Where(x => Execute(x, 0, 0, instructions).Output.StartsWith(goal))
                .ToList();
        }
        var results = candidatesAll
            .Where(x => Execute(x, 0, 0, intstructions).Output == string.Join(",", intstructions))
            .OrderBy(x => x);
        return results.Min();
    }
    
    private record Instruction(int OpCode, int Operand)
    {
        public State Execute(State state) => OpCode switch
        {
            0 => state.Move() with { A = state.A >> (int)Combo(state) },
            1 => state.Move() with { B = state.B ^ Operand },
            2 => state.Move() with { B = Combo(state) % 8 },
            3 => state with { Pointer = state.A == 0 ? state.Pointer + 2 : Operand },
            4 => state.Move() with { B = state.B ^ state.C },
            5 => state.Move() with { Output = state.Output == string.Empty ? (Combo(state) % 8).ToString() : state.Output + "," + Combo(state) % 8 },
            6 => state.Move() with { B = state.A >> (int)Combo(state) },
            7 => state.Move() with { C = state.A >> (int)Combo(state) },
            _ => throw new UnreachableException("Should not be reachable")
        };
        
        private long Combo(State state) => Operand switch
        {
            0 or 1 or 2 or 3 => Operand,
            4 => state.A,
            5 => state.B,
            6 => state.C,
            _ => throw new UnreachableException("Should not be reachable")
        };
    };
    
    private record State(long A, long B, long C, string Output, int Pointer)
    {
        public State Move() => this with { Pointer = Pointer + 2 };
    };
}