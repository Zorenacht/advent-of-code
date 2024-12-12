namespace AoC._2021;

public sealed class Day01 : Day
{
    [Puzzle(answer: 1653)]
    public int Part2()
    {
        List<int> depthMeasurements = new List<int>();
        foreach (string line in Input)
        {
            depthMeasurements.Add(int.Parse(line));
        }
        int count = 0;


        int prevAvg = depthMeasurements[0] + depthMeasurements[1] + depthMeasurements[2];

        for (int i = 1; i < depthMeasurements.Count - 2; i++)
        {
            int avg = depthMeasurements[i] + depthMeasurements[i + 1] + depthMeasurements[i + 2];
            if (avg > prevAvg)
                count++;
            prevAvg = avg;
        }
        return count;
    }
}