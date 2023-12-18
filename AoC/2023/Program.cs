using AoC_2023;
using System.Diagnostics;

int tries = 100;
long accu = 0;
var sw = new Stopwatch();
for (int i = 0; i < tries; i++)
{
    var start = sw.ElapsedMilliseconds;
    sw.Start();
    var result = new Day12().Part2();
    sw.Stop();
    accu += sw.ElapsedMilliseconds - start;
}
Console.WriteLine(accu / tries);