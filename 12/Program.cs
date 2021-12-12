using Tools;

part1();
Console.WriteLine("----------------");
part2();

void part2()
{
    Dictionary<string, List<string>> adjecencyList = initGrid("Input.txt");
    int count = 0;
    foreach (var connection in adjecencyList.Where(x => x.Key == "start").First().Value)
    {
        travelNext2(connection, new List<string>() { "start", connection }, false, ref count, adjecencyList);
    }
    Console.WriteLine("Count: " + count);
}

void travelNext2(string currentNode, List<string> smallVisited, bool extraVisited, ref int count, Dictionary<string, List<string>> adjecencyList)
{
    if (currentNode == "end")
    {
        count++;
        Console.WriteLine(string.Join(",", smallVisited));
        return;
    }
    foreach (var connection in adjecencyList.Where(x => x.Key == currentNode).FirstOrDefault().Value ?? new List<string>())
    {
        if (!isSmall(connection) || 
            (isSmall(connection) && 
                (!smallVisited.Contains(connection) || !extraVisited)))
        {
            var smallVisitedCopy = smallVisited.ToList();
            smallVisitedCopy.Add(connection);
            travelNext2(connection, smallVisitedCopy, extraVisited || isSmall(connection) && smallVisited.Contains(connection), ref count, adjecencyList);
        }
    }
}


void part1()
{
    Dictionary<string, List<string>> adjecencyList = initGrid("Input.txt");
    int count = 0;
    foreach (var connection in adjecencyList.Where(x => x.Key == "start").First().Value)
    {
        travelNext(connection, new List<string>() {"start", connection}, ref count, adjecencyList);
    }
    Console.WriteLine("Count: " + count);
}

void travelNext(string currentNode, List<string> smallVisited, ref int count, Dictionary<string, List<string>> adjecencyList)
{
    if (currentNode == "end")
    {
        count++;
        Console.WriteLine(string.Join(",", smallVisited));
        return;
    }
    foreach (var connection in adjecencyList.Where(x => x.Key == currentNode).FirstOrDefault().Value ?? new List<string>())
    {
        if (!isSmall(connection) || (isSmall(connection) && !smallVisited.Contains(connection)))
        {
            var smallVisitedCopy = smallVisited.ToList();
            smallVisitedCopy.Add(connection);
            travelNext(connection, smallVisitedCopy, ref count, adjecencyList);
        }
    }
}

bool isSmall(string node)
{
    if (node == node.ToUpper())
    {
        return false;
    }
    return true;
}

Dictionary<string, List<string>> initGrid(string filename)
{
    string[] lines = Reader.ReadLines(filename);
    Dictionary<string,List<string>> nodes = new Dictionary<string,List<string>>();
    foreach(string line in lines)
    {
        string[] split = line.Split('-');
        if (!nodes.ContainsKey(split[0]))
            nodes.Add(split[0], new List<string>());
        if (!nodes.ContainsKey(split[1]))
            nodes.Add(split[1], new List<string>());
        if(split[1] != "start")
            nodes.Where(x => x.Key == split[0]).First().Value.Add(split[1]);
        if (split[0] != "start")
            nodes.Where(x => x.Key == split[1]).First().Value.Add(split[0]);
    }
    nodes.Remove("end");
    return nodes;
}