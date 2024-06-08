namespace AoC_2021;

public sealed class Day25 : Day
{
    [Puzzle(answer: 571)]
    public int Part1()
    {
        var sea = ParseInput(Input);
        int count = 0;
        int iteration = 0;
        do
        {
            count = 0;
            TryMoveRight(ref sea, ref count);
            TryMoveDown(ref sea, ref count);

            iteration++;
        } while (count != 0);
        Print2d(sea);
        PrintStats(count, iteration);
        return iteration;

        void TryMoveRight(ref char[][] sea, ref int count)
        {
            var seaCopy = new char[sea.Length][];
            for (int i = 0; i < sea.Length; i++)
            {
                var cols = sea[i].Length;
                seaCopy[i] = new char[cols];
                for (int j = 0; j < cols; j++)
                {
                    if (sea[i][j] == '>')
                    {
                        var nextCol = j < cols - 1 ? j + 1 : 0;
                        if (sea[i][nextCol] == '.')
                        {
                            seaCopy[i][j] = '.';
                            seaCopy[i][nextCol] = '>';
                            count++;
                        }
                        else
                        {
                            seaCopy[i][j] = sea[i][j];
                        }
                    }
                    else if (seaCopy[i][j] == '\0')
                    {
                        seaCopy[i][j] = sea[i][j];
                    }
                }
            }
            sea = seaCopy;
        }

        void TryMoveDown(ref char[][] sea, ref int count)
        {
            var seaCopy = new char[sea.Length][];
            var rows = sea.Length;
            for (int i = 0; i < rows; i++)
            {
                seaCopy[i] = new char[sea[i].Length];
            }

            for (int i = 0; i < rows; i++)
            {
                var cols = sea[i].Length;
                for (int j = 0; j < cols; j++)
                {
                    if (sea[i][j] == 'v')
                    {
                        var nextRow = i < rows - 1 ? i + 1 : 0;
                        if (sea[nextRow][j] == '.')
                        {
                            seaCopy[nextRow][j] = 'v';
                            seaCopy[i][j] = '.';
                            count++;
                        }
                        else
                        {
                            seaCopy[i][j] = sea[i][j];
                        }
                    }
                    else if (seaCopy[i][j] == '\0')
                    {
                        seaCopy[i][j] = sea[i][j];
                    }
                }
            }
            sea = seaCopy;
        }



        char[][] ParseInput(string[] input)
        {
            var text = input;

            var rows = text.Count();
            var sea = new char[rows][];
            for (int i = 0; i < text.Count(); i++)
            {
                var line = text.ElementAt(i);
                var cols = line.Count();
                sea[i] = new char[cols];
                for (int j = 0; j < cols; j++)
                {
                    var ch = line[j];
                    sea[i][j] = ch;
                }
            }
            return sea;
        }


        void Print2d(char[][] sea)
        {
            Console.WriteLine("--------------------------------------");
            for (int i = 0; i < sea.Length; i++)
            {
                for (int j = 0; j < sea[i].Length; j++)
                {
                    Console.Write(sea[i][j]);
                }
                Console.WriteLine();
            }
        }

        void PrintStats(int count, int iteration) => Console.WriteLine($"Count: {count}, Iteration: {iteration}");
    }
}