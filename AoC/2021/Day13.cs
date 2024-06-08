namespace AoC_2021;

public sealed class Day13 : Day
{
    [Puzzle(answer: 0)]
    [Ignore("Incomplete")]
    public int Part1()
    {
        List<(string, int)> folds = ReadFolds();
        int[,] paper = ReadPaper();
        foreach (var fold in folds)
        {
            if (fold.Item1 == "x")
            {
                paper = FoldHorizontal(fold.Item2, paper);
            }
            if (fold.Item1 == "y")
            {
                paper = FoldVertical(fold.Item2, paper);
            }
        }
        return -1;

        int[,] FoldHorizontal(int x, int[,] paper)
        {
            int[,] folded = new int[paper.GetLength(0), paper.GetLength(0)];

            string[] lines = Reader.ReadLines("Input.txt");
            foreach (string line in lines)
            {
                var fold = line.Split(',')[2];
                paper[Convert.ToInt32(fold[0]), Convert.ToInt32(fold[1])] = 1;
            }
            return paper;
        }

        int[,] FoldVertical(int y, int[,] paper)
        {
            //int[,] paper = new int[1500, 1500];

            string[] lines = Reader.ReadLines("Input.txt");
            foreach (string line in lines)
            {
                var fold = line.Split(',')[2];
                paper[Convert.ToInt32(fold[0]), Convert.ToInt32(fold[1])] = 1;
            }
            return paper;
        }

        List<(string, int)> ReadFolds()
        {
            List<(string, int)> folds = new List<(string, int)>();

            string[] lines = Reader.ReadLines("Folds.txt");
            foreach (string line in lines)
            {
                var fold = line.Split(' ')[2].Split('=');
                folds.Add((fold[0], Convert.ToInt32(fold[1])));
            }
            return folds;
        }

        int[,] ReadPaper()
        {
            int[,] paper = new int[1500, 1500];

            string[] lines = Reader.ReadLines("Input.txt");
            foreach (string line in lines)
            {
                var fold = line.Split(',')[2];
                paper[Convert.ToInt32(fold[0]), Convert.ToInt32(fold[1])] = 1;
            }
            return paper;
        }
    }
}