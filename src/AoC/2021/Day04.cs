namespace AoC._2021;

public sealed class Day04 : Day
{
    [Puzzle(answer: 6592)]
    public int Part1()
    {
        int[] rolled = new int[] { 93, 18, 74, 26, 98, 52, 94, 23, 15, 2, 34, 75, 13, 31, 39, 76, 96, 16, 84, 12, 38, 27, 8, 85, 86, 43, 4, 79, 57, 19, 40, 59, 14, 21, 35, 0, 90, 11, 32, 17, 78, 83, 54, 42, 66, 82, 99, 45, 55, 63, 24, 5, 89, 46, 80, 49, 3, 48, 67, 47, 50, 60, 81, 51, 71, 33, 72, 6, 9, 30, 56, 20, 77, 29, 28, 69, 25, 36, 91, 92, 65, 22, 62, 58, 64, 88, 10, 7, 87, 41, 44, 37, 73, 70, 68, 97, 61, 95, 53, 1 };
        //int[] rolled = new int[] { 7, 4, 9, 5, 11, 17, 23, 2, 0, 14, 21, 24, 10, 16, 13, 6, 15, 25, 12, 22, 18, 20, 8, 19, 3, 26, 1 };
        List<Bingo> bingos = new List<Bingo>();
        List<Bingo> completed = new List<Bingo>();
        parseInput(bingos);

        foreach (var roll in rolled)
        {
            foreach (var bingo in bingos)
            {
                int result = bingo.Apply(roll);
                if (result > 0)
                {
                    Console.WriteLine("Bingo at roll: " + roll + " with board");
                    bingo.Print();
                    completed.Add(bingo);
                    Console.WriteLine("Multiplication: " + result * roll);
                    return result * roll;
                }
            }
            foreach (var comp in completed)
            {
                bingos.Remove(comp);
            }
            completed.Clear();
        }
        return -1;

        void parseInput(List<Bingo> bingos)
        {
            var text = InputAsText;
            string[] delim = new string[] { " ", Environment.NewLine };
            string[] result = text.Split(delim, StringSplitOptions.None);
            List<int> fillBoard = new List<int>();
            foreach (string s in result)
            {
                if (!s.Contains(Environment.NewLine) && !string.IsNullOrEmpty(s))
                {
                    int o = int.Parse(s);
                    fillBoard.Add(o);
                }
                if (fillBoard.Count == 25)
                {
                    bingos.Add(new Bingo(fillBoard));
                    fillBoard.Clear();
                }
            }
        }
    }

    public class Bingo
    {
        public int[,] Board { get; private set; } = new int[5, 5];
        private bool[,] Rolled { get; set; } = new bool[5, 5];

        public Bingo(List<int> board)
        {
            for (int i = 0; i < board.Count; i++)
            {
                Board[i / 5, i % 5] = board[i];
            }
        }

        private int CountNotRolled()
        {
            int count = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (!Rolled[i, j])
                    {
                        count += Board[i, j];
                    }
                }
            }
            return count;
        }

        public int Apply(int rolled)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Board[i, j] == rolled)
                    {
                        Rolled[i, j] = true;
                    }
                }
            }
            return Done() ? CountNotRolled() : 0;
        }

        private bool Done()
        {
            for (int i = 0; i < 5; i++)
            {
                if (Rolled[0, i] == true && Rolled[1, i] == true && Rolled[2, i] == true &&
                    Rolled[3, i] == true && Rolled[4, i] == true ||
                   Rolled[i, 0] == true && Rolled[i, 1] == true && Rolled[i, 2] == true &&
                   Rolled[i, 3] == true && Rolled[i, 4] == true)
                {
                    return true;
                }
            }
            return false;
        }

        public void Print()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    string before = Board[i, j] < 10 ? " " : "";
                    string after = Rolled[i, j] ? "|" : " ";
                    Console.Write(before + Board[i, j] + after);
                }
                Console.WriteLine();
            }
        }
    }
}