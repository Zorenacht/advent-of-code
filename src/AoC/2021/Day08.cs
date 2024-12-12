namespace AoC._2021;

public sealed class Day08 : Day
{
    [Puzzle(answer: 387)]
    public int Part1()
    {
        List<List<string>> attempts = new List<List<string>>();
        List<List<string>> output = new List<List<string>>();
        parseInput(attempts, output);
        int count = 0;
        foreach (var entry in output)
        {
            foreach (var str in entry)
            {
                if (str.Length == 2 || str.Length == 3 || str.Length == 4 || str.Length == 7)
                {
                    //Console.WriteLine(str);
                    count++;
                }
            }
        }
        return count;
    }

    [Puzzle(answer: 986034)]
    public int Part2()
    {
        List<List<string>> inputs = new List<List<string>>();
        List<List<string>> outputs = new List<List<string>>();
        const int distinguishOccurence = 10;
        parseInput(inputs, outputs);
        int count = 0;
        for (int i = 0; i < inputs.Count; i++)
        {
            List<string> input = inputs[i];
            int[] occurenceCount = new int[7]; //count occurence of 7 possible letters -> a to g
            int[] mapping = new int[7];
            foreach (var str in input)
            {
                foreach (char c in str)
                {
                    occurenceCount[c - 'a']++;
                }
            }
            occurenceCount[findCodingForA(input)] += distinguishOccurence;
            occurenceCount[findCodingForD(input)] += distinguishOccurence;

            for (int j = 0; j < occurenceCount.Length; j++)
            {
                switch (occurenceCount[j])
                {
                    case 4://e will occur 4 times
                        mapping[4] = j;
                        break;
                    case 6://b will occur 6 times
                        mapping[1] = j;
                        break;
                    case 9://f will occur 9 times
                        mapping[5] = j;
                        break;
                    case 7://g and d will occur 7 times but we added 10 to the count of d
                        mapping[6] = j;
                        break;
                    case 17://d
                        mapping[3] = j;
                        break;
                    case 8://c and a will occur 8 times but we added 10 to the count of a
                        mapping[2] = j;
                        break;
                    case 18://a
                        mapping[0] = j;
                        break;
                }
            }


            List<string> output = outputs[i];
            int value = 0;
            foreach (var str in output)
            {
                value = value * 10 + decipherValue(str, mapping);
                //Console.WriteLine("Encoded:" + str + ", Value: " + decipherValue(str, mapping));
            }
            count += value;
            //Console.WriteLine("Value: " + value);
        }
        Console.WriteLine("Total Count: " + count);
        return count;
    }

    void parseInput(List<List<string>> attempts, List<List<string>> output)
    {
        foreach (string line in Input)
        {
            //Console.WriteLine(line);
            attempts.Add(line.Split('|')[0].Split(' ').SkipLast(1).ToList());
            output.Add(line.Split('|')[1].Split(' ').Skip(1).ToList());
        }
        //Console.WriteLine("Count attempts" + attempts.Last().Count());
        //Console.WriteLine("Count output" + output.Last().Count());
    }

    int decipherValue(string coded, int[] mapping)
    {
        if (coded.Length == 2)
            return 1;
        if (coded.Length == 3)
            return 7;
        if (coded.Length == 4)
            return 4;
        if (coded.Length == 7)
            return 8;
        if (coded.Length == 5)
        {
            if (!coded.Contains((char)(mapping[5] + 'a')))
                return 2;
            if (!coded.Contains((char)(mapping[1] + 'a')))
                return 3;
            if (!coded.Contains((char)(mapping[2] + 'a')))
                return 5;
        }
        if (coded.Length == 6)
        {
            if (!coded.Contains((char)(mapping[3] + 'a')))
                return 0;
            if (!coded.Contains((char)(mapping[4] + 'a')))
                return 9;
            if (!coded.Contains((char)(mapping[2] + 'a')))
                return 6;

        }
        return -1;

    }

    int findCodingForA(List<string> input)
    {
        string codingOf1 = "";
        string codingOf7 = "";
        foreach (var str in input)
        {
            if (str.Length == 2)
            {
                codingOf1 = str;
            }
            else if (str.Length == 3)
            {
                codingOf7 = str;
            }
        }
        return findFirstDifference(codingOf7, codingOf1) - 'a';//this is the a line
    }

    int findCodingForD(List<string> input)
    {
        int[] occurenceCount = new int[7];
        foreach (var str in input)
        {
            if (str.Length == 5 || str.Length == 4)
            {
                foreach (char c in str)
                {
                    occurenceCount[c - 'a']++;
                }
            }
        }
        for (int i = 0; i < occurenceCount.Length; i++)
        {
            if (occurenceCount[i] == 4)
            {
                return i;
            }
        }
        return -1;
    }

    char findFirstDifference(string input, string scrambledSubstringOfInput)
    {
        foreach (char c1 in input)
        {
            if (!scrambledSubstringOfInput.Contains(c1))
            {
                return c1;
            }
        }
        return '0';
    }
}