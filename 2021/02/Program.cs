List<int> depthMeasurements = new List<int>();
Console.WriteLine(parseInputAndExecute("Input.txt"));

int parseInputAndExecute(string filename)
{
    int depth = 0;
    int horizontal = 0;
    int aim = 0;

    string fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\", filename);
    var text = File.ReadLines(fileLocation);
    foreach (string line in text)
    {
        string[] command = line.Split(' ');
        int value = int.Parse(command[1]);
        switch (command[0])
        {
            case "down":
                aim += value;
                break;
            case "up":
                aim -= value;
                break;
            case "forward":
                horizontal += value;
                depth += value * aim;
                break;
        }
        if(depth < 0)
        {
            Console.WriteLine("here");
        }
    }

    return depth * horizontal;
}