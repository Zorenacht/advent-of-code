using System.Linq;
using Tools;

part2();

void part2()
{
    string[] text = Reader.ReadLines("Input.txt");
    List<List<Tuple<int, int>>> Basins = new List<List<Tuple<int, int>>>();
    List<int> counts = new List<int>();
    for (int i = 0; i < text.Length; i++)
    {
        for (int j = 0; j < text[i].Length; j++)
        {
            int count = 0;
            bool [,] alreadyVisited = new bool[text.Length,text[i].Length];
            if (isMinValue(i, j, text))
            {
                findBasin(i, j, text[i][j] - '0', ref count, text, alreadyVisited);
            }
            if (count > 0)
            {
                if (count == 91 || count == 89 || count == 82)
                {
                    for (int p = 0; p < alreadyVisited.GetLength(0); p++)
                    {
                        for (int q = 0; q < alreadyVisited.GetLength(1); q++)
                        {
                            Console.Write(alreadyVisited[p,q] ? text[p][ q] : "X");
                            //Console.Write(alreadyVisited[p, q] ? 1 : 0);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine("----------------------------------------");
                }
                counts.Add(count);
            }
        }
    }
    counts = counts.OrderByDescending(x => x).ToList();
    Console.WriteLine(string.Join(", ",counts));
    Console.WriteLine(counts[0] * counts[1] * counts[2]);
}

void findBasin(int i, int j, int value, ref int count, string[] text, bool[,] alreadyVisited)
{
    if (alreadyVisited[i,j])
    {
        return;
    }
    count++;
    alreadyVisited[i, j] = true;

    //Console.WriteLine("i: " + i + " j: " + j + " value: " + value);
    //Console.WriteLine("left: " + (j > 0 ? text[i][j - 1] - '0' : -1));
    if (i > 0                   && text[i - 1][j    ] - '0' > value && text[i - 1][j] - '0' < 9)
        findBasin(i - 1, j  , value + 1, ref count, text, alreadyVisited);
    if (i < text.Length - 1     && text[i + 1][j    ] - '0' > value && text[i + 1][j] - '0' < 9)
        findBasin(i + 1, j  , value + 1, ref count, text, alreadyVisited);
    if (j > 0                   && text[i    ][j - 1] - '0' > value && text[i][j - 1] - '0' < 9)
        findBasin(i    , j-1, value + 1, ref count, text, alreadyVisited);
    if (j < text[i].Length - 1  && text[i    ][j + 1] - '0' > value && text[i][j + 1] - '0' < 9)
        findBasin(i    , j+1, value + 1, ref count, text, alreadyVisited);
    return;
}



void AddBasin(Tuple<int,int> coord, List<List<Tuple<int, int>>> Basins)
{
    int addedTo = -1;
    for(int i = 0; i < Basins.Count(); i++)
    {
        foreach(var coordinate in Basins[i])
        {
            if(neighbor(coordinate, coord)){
                if(addedTo == -1)
                {
                    Basins[i].Add(coordinate);
                    addedTo = i;
                }
                else
                {
                    Basins[addedTo].AddRange(Basins[i]);
                    Basins.RemoveAt(i);
                }
            }
        }
    }
    if(addedTo == -1)
    {
        Basins.Add(new List<Tuple<int,int>>());
        Basins.Last().Add(coord);
    }
}

bool neighbor(Tuple<int,int> coord1, Tuple<int,int> coord2)
{
    return coord1 == coord2 ||
        (coord1.Item1 == coord2.Item1 - 1 && coord1.Item2 == coord2.Item2) ||
        (coord1.Item1 == coord2.Item1 + 1 && coord1.Item2 == coord2.Item2) ||
        (coord1.Item1 == coord2.Item1 && coord1.Item2 == coord2.Item2 - 1) ||
        (coord1.Item1 == coord2.Item1 && coord1.Item2 == coord2.Item2 + 1);

}

void part1()
{
    string[] text = Reader.ReadLines("Input.txt");
    int count = 0;
    for(int i=0; i<text.Length; i++)
    {
        for(int j=0; j<text[i].Length; j++)
        {
            count += isMinValue(i, j, text) ? text[i][j]-'1' + 2 : 0;
        }
    }
    Console.WriteLine("Count: " + count);
}

bool isMinValue(int i, int j, string[] text)
{
    if (i > 0 && text[i - 1][j] <= text[i][j])
        return false;
    if (i < text.Length-1 && text[i + 1][j] <= text[i][j])
        return false;
    if (j > 0 && text[i][j-1] <= text[i][j])
        return false;
    if (j < text[i].Length-1 && text[i][j+1] < text[i][j])
        return false;
    return true;
}