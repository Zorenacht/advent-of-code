List<int> oxygenList = Enumerable.Range(0,1000).ToList();
List<int> co2List = Enumerable.Range(0, 1000).ToList();
string data = parseInput("Input.txt");

getMostCommon(oxygenList);
getLeastCommon(co2List);

Console.WriteLine(String.Join(",", oxygenList));
Console.WriteLine(String.Join(",", co2List));

double oxygen = binaryToInt(data.Substring(14 * oxygenList[0], 12));
double co2 = binaryToInt(data.Substring(14 * co2List[0], 12));

Console.WriteLine(oxygen + " " + co2);
Console.WriteLine(oxygen*co2);

double binaryToInt(string input)
{
    double result = 0;
    for(int i=0; i<input.Length; i++)
    {
        result += Math.Pow(2, input.Length - i - 1) * (int)char.GetNumericValue(input[i]);
    }
    return result;
}


void getMostCommon(List<int> oxygenList)
{
    for (int i = 0; i < 12; i++)
    {
        int count = 0;
        foreach (int index in oxygenList)
        {
            count += (int)char.GetNumericValue(data[i + 14 * index]);
        }
        int mostCommon = (2 * count >= oxygenList.Count ? 1 : 0);
        Console.WriteLine("1 has occurred: " + count + " vs avg " + (double)oxygenList.Count/2 +
            ", thus " + mostCommon + " is most common");
        for (int index = oxygenList.Count-1; index>=0; index--)
        {
            if((int)char.GetNumericValue(data[i + 14 * oxygenList[index]]) != mostCommon)
            {
                oxygenList.RemoveAt(index);
            }
        }
        if(oxygenList.Count == 1)
        {
            break;
        }
    }
}

void getLeastCommon(List<int> oxygenList)
{
    for (int i = 0; i < 12; i++)
    {
        int count = 0;
        foreach (int index in oxygenList)
        {
            count += (int)char.GetNumericValue(data[i + 14 * index]);
        }
        int leastCommon = (2*count >= oxygenList.Count ? 0 : 1);
        Console.WriteLine("1 has occurred: " + count + " vs avg " + (double)oxygenList.Count / 2 +
            ", thus " + leastCommon + " is least common");
        for (int index = oxygenList.Count - 1; index >= 0; index--)
        {
            if ((int)char.GetNumericValue(data[i + 14 * oxygenList[index]]) != leastCommon)
            {
                oxygenList.RemoveAt(index);
            }
        }
        if (oxygenList.Count == 1)
        {
            break;
        }
    }
}


string parseInput(string filename)
{
    string fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\", filename);
    return File.ReadAllText(fileLocation);
}