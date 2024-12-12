namespace AoC._2021;

public sealed class Day02 : Day
{
    [Puzzle(answer: 1848454425)]
    public int Part2()
    {
        List<int> depthMeasurements = new List<int>();
        int depth = 0;
        int horizontal = 0;
        int aim = 0;

        foreach (string line in Input)
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
            if (depth < 0)
            {
                Console.WriteLine("here");
            }
        }

        return depth * horizontal;
    }
}