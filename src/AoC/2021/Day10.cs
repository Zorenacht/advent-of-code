namespace AoC._2021;

public sealed class Day10 : Day
{
    [Puzzle(answer: 166191)]
    public int Part1()
    {
        var text = Input;
        Stack<char> symbols = new Stack<char>();
        int count = 0;
        foreach (var line in text)
        {
            foreach (char c in line)
            {
                if (openSymbol(c))
                {
                    symbols.Push(c);
                }
                else if (symbolOpposite(symbols.Peek()) == c)
                {
                    symbols.Pop();
                }
                else
                {
                    count += symbolValue(c);
                    break;
                }
            }
        }
        Console.WriteLine(count);
        return count;
    }

    [Puzzle(answer: 1152088313)]
    public long Part2()
    {
        var text = Input;
        List<Tuple<string, long>> completionList = new List<Tuple<string, long>>();
        foreach (var line in text)
        {
            long count = 0;
            string sequence = "";
            bool invalid = false;
            Stack<char> symbols = new Stack<char>();

            foreach (char c in line)
            {
                if (openSymbol(c))
                {
                    symbols.Push(c);
                }
                else if (symbolOpposite(symbols.Peek()) != c)
                {
                    invalid = true;
                    break;
                }
                else
                {
                    symbols.Pop();
                }
            }
            if (!invalid)
            {
                while (symbols.Count > 0)
                {
                    char c = symbolOpposite(symbols.Peek());
                    count = 5 * count + symbolValueP2(c);
                    sequence += c;
                    symbols.Pop();
                }
                completionList.Add(new Tuple<string, long>(sequence, count));
            }
        }
        completionList = completionList.OrderBy(x => x.Item2).ToList();
        int length = completionList.Count();
        var median = completionList[length / 2];
        //Console.WriteLine(String.Join(' ', completionList));
        Console.WriteLine(string.Join(' ', median));

        return median.Item2;
    }

    int symbolValueP2(char s)
    {
        if (s == ')')
            return 1;
        if (s == ']')
            return 2;
        if (s == '}')
            return 3;
        if (s == '>')
            return 4;
        return 0;
    }

    bool openSymbol(char s)
    {
        return s == '{' || s == '(' || s == '[' || s == '<';
    }

    char symbolOpposite(char s)
    {
        if (s == '(')
            return ')';
        if (s == '[')
            return ']';
        if (s == '{')
            return '}';
        if (s == '<')
            return '>';
        return ' ';
    }


    int symbolValue(char s)
    {
        if (s == ')')
            return 3;
        if (s == ']')
            return 57;
        if (s == '}')
            return 1197;
        if (s == '>')
            return 25137;
        return 0;
    }

}