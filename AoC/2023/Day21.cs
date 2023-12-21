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
    public long Part1Example() => new Airship().Part1(InputExample, 6, 3);

    public int Copies = 1 + 0 * 2;

    [Puzzle(answer: 3677)]
    public long Part1() => new Airship().Part1(Input, 65, 1);

    [Puzzle(answer: null)]
    public long Part2Example() => new Airship().Part2(InputExample, 5000);

    [Puzzle(answer: 609585229256084)]
    public long Part2() => new Airship().Part2(Input, 26501365);

    [Test]
    public void OriginalSequenceTest()
    {
        long[] input = [1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23];
        long[] expectedEven = [1, 1, 9, 9, 25, 25, 49, 49, 81, 81, 121, 121];
        long[] expectedUneven = [0, 4, 4, 16, 16, 36, 36, 64, 64, 100, 100, 144];

        var obj = new Airship();
        input.Select(x => obj.EvenOriginal(x)).Should().BeEquivalentTo(expectedEven);
        input.Select(x => obj.UnevenOriginal(x)).Should().BeEquivalentTo(expectedUneven);
    }

    private class Airship
    {
        internal long Part1(string[] input, long steps, int copies)
        {
            var temp = input.Select(x => string.Join("", Enumerable.Repeat(x, copies))).ToArray();
            input = new string[input.Length * copies];
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = temp[i % temp.Length].Replace('S', '.');
            }
            int row = input.Length / 2;
            int col = input[0].Length / 2;
            var replaced = input[row].ToCharArray();
            replaced[col] = 'S';
            input[row] = new string(replaced);
            var firstStepReached = new int[input.Length][];
            for (int i = 0; i < input.Length; i++)
            {
                firstStepReached[i] = new int[input[0].Length];
                for (int j = 0; j < input[0].Length; j++)
                {
                    firstStepReached[i][j] = -1;
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
                    //Console.Write($"{input[i][j]}");
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


            /*long a = (blocks / 2) * uneven[1];
            long b = (blocks / 2 + 1) * uneven[0];*/
            return steps % 2 == 1
                ? EvenOriginal(blocksDiagonal) * uneven[0] + UnevenOriginal(blocksDiagonal) * even[0]
                    + blocks / 4 * (even[1] + uneven[1]) - toEdge
                : EvenOriginal(blocksDiagonal) * even[0] + UnevenOriginal(blocksDiagonal) * uneven[0]
                    + blocks / 4 * (even[1] + uneven[1]) - toEdge;
        }

        public long EvenOriginal(long length)
        {
            if ((length - 1) % 4 == 0) return ((length + 1) / 2) * ((length + 1) / 2);
            else if ((length - 1) % 4 == 2) return (length / 2) * (length / 2);
            else throw new NotSupportedException();
        }

        public long UnevenOriginal(long length)
        {
            if ((length - 1) % 4 == 0) return (length / 2) * (length / 2);
            else if ((length - 1) % 4 == 2) return ((length + 1) / 2) * ((length + 1) / 2);
            else throw new NotSupportedException();
        }

    }
}