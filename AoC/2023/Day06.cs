using MathNet.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Tools.Shapes;

namespace AoC_2023;

public sealed class Day06 : Day
{
    [Puzzle(answer: 316800)]
    public long Part1()
    {
        int[] time = [61, 67, 75, 71];
        int[] dist = [430, 1036, 1307, 1150];
        int[] no = new int[4];
        for(int i=0; i<time.Length; i++)
        {
            for(int j=0; j <= time[i]; j++)
            {
                var d = j * (time[i] - j);
                if (d > dist[i]) no[i]++;
            }
        }
        return no[0]*no[1]*no[2]*no[3];
    }

    [Puzzle(answer: 45647654)]
    public long Part2()
    {
        long time = 61677571;
        long dist = 430103613071150;
        long no = 0;
        for (int j = 0; j <= time; j++)
        {
            var d = j * (time - j);
            if (d > dist) no++;
        }
        return no;
    }
}