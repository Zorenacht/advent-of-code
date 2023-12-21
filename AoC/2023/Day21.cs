using FluentAssertions.Equivalency.Steps;
using MathNet.Numerics;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Constraints;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using Tools.Shapes;

namespace AoC_2023;

public sealed class Day21 : Day
{
    [Puzzle(answer: 16)]
    public long Part1Example() => new Airship().Part1(InputExample, 6);

    [Puzzle(answer: 3677)]
    public long Part1() => new Airship().Part1(Input, 65);

    [Puzzle(answer: null)]
    public long Part2Example() => new Airship().Part2(InputExample, 5000);

    //??? 605697286043784
    //??? 605697286043616

    //not 605697286043784
    //not 610690196104277 too high
    //not 1221380392212445
    [Puzzle(answer: null)]
    public long Part2() => new Airship().Part2(Input, 65);

    private class Airship
    {
        internal long Part1(string[] input, long steps)
        {
            int row = -1;
            int col = -1;
            var firstStepReached = new int[input.Length][];
            for (int i = 0; i < input.Length; i++)
            {
                firstStepReached[i] = new int[input[0].Length];
                for (int j = 0; j < input[0].Length; j++)
                {
                    firstStepReached[i][j] = -1;
                    if (input[i][j] == 'S')
                    {
                        row = i;
                        col = j;
                    }
                }
            }

            int reachable = 0;
            int unreachable = 0;
            Complex[] dirs = [
                Complex.ImaginaryOne,
                -Complex.One,
                -Complex.ImaginaryOne,
                Complex.One];

            var queue = new Queue<Complex>();
            queue.Enqueue(new Complex(col, row));
            var visited = new HashSet<Complex>();
            for (int i = 0; i <= Math.Min(steps, 10000); i++)
            {
                var next = new Queue<Complex>();
                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    if (input[(int)current.Imaginary][(int)current.Real] == '#')
                    {
                        continue;
                    }
                    if (firstStepReached[(int)current.Imaginary][(int)current.Real] == -1)
                    {
                        firstStepReached[(int)current.Imaginary][(int)current.Real] = i;
                    }
                    for (int r = 0; r < 4; r++)
                    {
                        var nb = current + dirs[r];
                        if (nb.Real >= 0 && nb.Imaginary >= 0 && nb.Real < input[0].Length && nb.Imaginary < input.Length && !visited.Contains(nb))
                        {
                            visited.Add(nb);
                            next.Enqueue(nb);
                        }
                    }
                }
                queue = next;
            }
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[0].Length; j++)
                {
                    if (firstStepReached[i][j] <= steps)
                    {
                        if (input[i][j] == '#' || steps % 2 != firstStepReached[i][j] % 2)
                        {
                            unreachable++;
                        }
                        else if (steps % 2 == firstStepReached[i][j] % 2) reachable++;
                        else { throw new Exception(); }
                    }
                }
            }

            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[0].Length; j++)
                {
                    Console.Write($"{(firstStepReached[i][j] % 2 == steps % 2 ? $"{firstStepReached[i][j] % 10:0}" : input[i][j])}");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            //Console.WriteLine($"{input.Length * input[0].Length}");
            return reachable;
        }

        internal long Part2(string[] input, long steps)
        {
            int row = -1;
            int col = -1;
            var firstStepReached = new int[input.Length][];
            for (int i = 0; i < input.Length; i++)
            {
                firstStepReached[i] = new int[input[0].Length];
                for (int j = 0; j < input[0].Length; j++)
                {
                    firstStepReached[i][j] = -1;
                    if (input[i][j] == 'S')
                    {
                        row = i;
                        col = j;
                    }
                }
            }

            int[] even = [0, 0];
            int[] uneven = [0, 0];
            Complex[] dirs = [
                Complex.ImaginaryOne,
                -Complex.One,
                -Complex.ImaginaryOne,
                Complex.One];

            var queue = new Queue<Complex>();
            queue.Enqueue(new Complex(col, row));
            var visited = new HashSet<Complex>();
            int radius = (input.Length - 1) / 2;
            for (int i = 0; i <= input.Length + 10000; i++)
            {
                var next = new Queue<Complex>();
                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    if (input[(int)current.Imaginary][(int)current.Real] == '#')
                    {
                        continue;
                    }
                    if (firstStepReached[(int)current.Imaginary][(int)current.Real] == -1)
                    {
                        firstStepReached[(int)current.Imaginary][(int)current.Real] = i;
                        if (i <= (input.Length - 1) / 2)
                        {
                            if (i % 2 == 0) even[0]++;
                            else uneven[0]++;
                        }
                        else
                        {
                            if (i % 2 == 0) even[1]++;
                            else uneven[1]++;
                        }
                    }
                    for (int r = 0; r < 4; r++)
                    {
                        var nb = current + dirs[r];
                        if (nb.Real >= 0 && nb.Imaginary >= 0 && nb.Real < input[0].Length && nb.Imaginary < input.Length && !visited.Contains(nb))
                        {
                            visited.Add(nb);
                            next.Enqueue(nb);
                        }
                    }
                }
                queue = next;
            }
            /*for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[0].Length; j++)
                {
                    if (firstStepReached[i][j] <= steps)
                    {
                        if (firstStepReached[i][j] == -1) continue;
                        if (firstStepReached[i][j] % 2 != 0)
                            uneven++;
                        else if (firstStepReached[i][j] % 2 == 0)
                            even++;
                        else { throw new Exception(); }
                    }
                }
            }*/

            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[0].Length; j++)
                {
                    Console.Write($"{(firstStepReached[i][j] % 2 == steps % 2 ? $"{firstStepReached[i][j] % 10:0}" : input[i][j])}");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            //Console.WriteLine($"{input.Length * input[0].Length}");
            long toEdge = (steps - (input.Length - 1) / 2) / input.Length;
            long blocksDiagonal = ((steps - (input.Length - 1) / 2) / input.Length) * 2 + 1;
            long blocks = blocksDiagonal * blocksDiagonal;
            return (blocks / 2) * uneven[1] + (blocks / 2 + 1) * uneven[0];
        }


    }
}